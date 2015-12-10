using UnityEngine;

/// <summary>
/// A simple player ai system. This ai will chase down the closest enemy or the mother ship and 
/// constantly shoot at it. It will also activate all powerups immediately.
/// </summary>
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Shooter))]
public class PlayerAi : MonoBehaviour
{
    public ScreenLayout layout;
    public Spawn motherShip;
    public Transform defendTarget;

    private Transform myTransform;
    private Movement movement;
    private Shooter shooter;
    private PlayerPowerUpManager powerUpManager;

	/// <summary>
    /// Load all components required to run this script.
    /// </summary>
	protected void Awake()
    {
        myTransform = transform;

        movement = GetComponent<Movement>();
        shooter = GetComponent<Shooter>();
        powerUpManager = GetComponent<PlayerPowerUpManager>();
	}
	
	/// <summary>
    /// Reassess this AIs priorities after each update frame. Note that this is only acceptable with a small amount
    /// of AI agents in play and can be extremely slow in large scale conflicts.
    /// </summary>
	protected void Update()
    {
        shooter.shoot = true; // never stop firing
        Transform closestTarget = null;
        float closestDistance = -1;
        bool isClosestTargetEnemy = false;

        // find closest enemy
        foreach (GameObject enemyObject in motherShip.enemyPool.GetAllObjectsInUse())
        {
            if (enemyObject == null)
            {
                continue;
            }

            Movement enemyMovement = enemyObject.GetComponent<Movement>();

            if (enemyMovement == null || Mathf.Sign(enemyMovement.direction.x) == Mathf.Sign(defendTarget.position.x))
            {
                float distance = Vector3.Distance(enemyObject.transform.position, defendTarget.position);
                if (closestDistance < 0 || distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = enemyObject.transform;
                    isClosestTargetEnemy = true;
                } 
            }
        }

        // find distance to mothership
        if (motherShip != null)
        {
            float motherShipDistance = Vector3.Distance(motherShip.transform.position, defendTarget.position);
            if (closestDistance < 0 || motherShipDistance < closestDistance)
            {
                closestTarget = motherShip.transform;
                isClosestTargetEnemy = false;
            }
        }

        // Align with target
        Vector2 moveDirection = Vector2.zero;

        if (closestTarget != null)
        {
            if (closestTarget.position.y - myTransform.position.y > movement.speed * Time.deltaTime)
            {
                moveDirection.y = 1;
            }
            else if (closestTarget.position.y - myTransform.position.y < movement.speed * Time.deltaTime)
            {
                moveDirection.y = -1;
            }
        }

        if ((transform.position.y > layout.size.y / 2 && moveDirection.y > 0) || 
            (transform.position.y < -layout.size.y / 2 && moveDirection.y < 0))
        {
            moveDirection.y = 0;
        }

        movement.direction = moveDirection;

        // Use power ups
        if (powerUpManager.StoredPowerUp != null && isClosestTargetEnemy)
        {
            powerUpManager.ActivatePowerUp(myTransform);
        }
	}
}
