using UnityEngine;
using System.Collections;

/// <summary>
/// Logic for managing the magnetic shield power up effect.
/// </summary>
[RequireComponent(typeof(Movement))]
public class MagnetShield : MonoBehaviour
{
    public Transform player;

    [Header("Magnet Settings")]
    public float distanceFromPlayer = 1.0f;
    public bool moveRight;
    [Space(10.0f)]
    public LayerMask enemyLayerMask;
    public int shieldLayer;  

    [Header("Attract Stats")]
    public float attractRadius;
    public float attractSpeed;
    public Vector2 topAttractPoint;
    public Vector2 bottomAttractPoint;
    
    private bool hasGrabbedEnemies;
    private Movement movement;
    private int shieldStrength;

	/// <summary>
	/// Initialize the magnet power and launch the behavior logic.
	/// </summary>
	protected void Start()
    {
        hasGrabbedEnemies = false;
        shieldStrength = 0;
        movement = GetComponent<Movement>();

        StartCoroutine(ActivatePowerUp());
	}  

    /// <summary>
    /// Retract shield when it hits an enemy.
    /// </summary>
    /// <param name="collider">Enemy collider.</param>
    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy")
        {
            hasGrabbedEnemies = true;
        }
    }

    /// <summary>
    /// Starts up the powerups logic. The magnet power up will fist extend until it hits an enemy or goes to far. 
    /// It will then retract itself until it is near the player and will capture enemies to use as a shield for the player.
    /// </summary>
    /// <returns>Null.</returns>
    private IEnumerator ActivatePowerUp()
    {
        // Wait to be in range
        while (!hasGrabbedEnemies)
        {
            // TODO: give up if gone to far
            movement.Direction = moveRight ? Vector2.right : Vector2.left;
            yield return null;
        }

        // Move all enemies onto magnet
        Vector2 direction = topAttractPoint - bottomAttractPoint;
        Vector2 castPosition = new Vector2(transform.position.x, transform.position.y) + (topAttractPoint + bottomAttractPoint) / 2;

        foreach (RaycastHit2D hit in Physics2D.CircleCastAll(castPosition, attractRadius, direction, direction.magnitude, enemyLayerMask.value))
        {
            shieldStrength++;

            // TODO: tie into enemies on death event to reduce shield strength.
            // TODO: set strenght number on magnet label.
            Movement enemyMovement = hit.transform.GetComponent<Movement>();
            Vector2 randomOffset = bottomAttractPoint + Random.Range(0.0f, 1.0f) * (topAttractPoint - bottomAttractPoint);
            StartCoroutine(MoveEnemyToShieldPoint(enemyMovement, randomOffset));
        }

        if (shieldStrength > 0)
        {
            // Move magnet to front of player
            while (Vector2.Distance(transform.position, player.position) > distanceFromPlayer)
            {
                movement.Direction = player.position - transform.position;
                yield return null;
            }

            transform.GetComponent<Collider2D>().enabled = false;
            movement.Direction = Vector2.zero;
            transform.parent = player;
        }
        else
        {
            // TODO: find a proper place to destroy the magnet
            while (transform.position.x > 0)
            {
                movement.Direction = Vector2.left;
                yield return null;
            }

            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Moves the taget enemy enemy to a position on the magnets attract area. The area should 
    /// be assigned randomly so the shield looks more scattered.
    /// </summary>
    /// <param name="enemyMovement">Movement logic for target enemy.</param>
    /// <param name="magnetOffset">Random offset from magets center.</param>
    /// <returns>Null</returns>
    private IEnumerator MoveEnemyToShieldPoint(Movement enemyMovement, Vector2 magnetOffset)
    {
        enemyMovement.transform.parent = transform;
        Vector2 targetPosition = (Vector2)transform.position + magnetOffset;

        while (Vector2.Distance(enemyMovement.transform.position, targetPosition) > 0.5f)
        {
            enemyMovement.Direction = targetPosition - (Vector2)enemyMovement.transform.position;
            enemyMovement.speed = attractSpeed;

            targetPosition = (Vector2)transform.position + magnetOffset;
            yield return null;
        }

        enemyMovement.Direction = Vector2.zero;
        enemyMovement.speed = 0;
        enemyMovement.gameObject.layer = shieldLayer;
    }

    /// <summary>
    /// Reduced the strenght of the shield and will despawn the magnet if there is no strength left.
    /// </summary>
    public void ReduceShieldStrength()
    {
        shieldStrength--;
        
        if (shieldStrength <= 0)
        {
            //TODO: Despawn
            GameObject.Destroy(gameObject);
        }
    }
}
