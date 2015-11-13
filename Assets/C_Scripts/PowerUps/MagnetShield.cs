using UnityEngine;
using System.Collections;

/// <summary>
/// Logic for managing the magnetic shield power up effect.
/// </summary>
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(RepeateTexture))]
public class MagnetShield : MonoBehaviour, IPowerUp
{
    public Transform player;

    [Header("Magnet Settings")]
    public float distanceFromPlayer = 1.0f;
    [Space(10.0f)]
    public LayerMask enemyLayerMask;
    public float spawnDistance;
    public float maxTravelDistance = 10.0f;

    [Header("Attract Stats")]
    public float attractRadius;
    public float attractSpeed;
    public Vector2 topAttractPoint;
    public Vector2 bottomAttractPoint;
    
    private bool hasGrabbedEnemies;
    private Movement movement;
    private int shieldStrength;
    private RepeateTexture repeatTexture;

	/// <summary>
	/// Initialize the magnet power and launch the behavior logic.
	/// </summary>
	protected void Awake()
    {
        hasGrabbedEnemies = false;
        shieldStrength = 0;
        movement = GetComponent<Movement>();
        repeatTexture = GetComponent<RepeateTexture>();
	}

    protected void OnDrawGizmos()
    {
        // Move all enemies onto magnet
        Vector2 midPoint = (topAttractPoint + bottomAttractPoint) / 2;
        midPoint.x *= transform.right.x;
        midPoint.y *= transform.right.y;
        Vector2 castPosition = (Vector2)transform.position + midPoint;

        Gizmos.DrawWireCube(castPosition, topAttractPoint - bottomAttractPoint);
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
        Vector3 startPosition = transform.position;

        // Wait to be in range
        while (!hasGrabbedEnemies && Vector3.Distance(startPosition, transform.position) < maxTravelDistance)
        {
            movement.Direction = player.right;
            yield return null;
        }

        // Move all enemies onto magnet
        Vector2 direction = topAttractPoint - bottomAttractPoint;
        Vector2 midPoint = (topAttractPoint + bottomAttractPoint) / 2;
        midPoint.x *= transform.right.x;
        midPoint.y *= transform.right.y;
        Vector2 castPosition = (Vector2)transform.position + midPoint;

        foreach (RaycastHit2D hit in Physics2D.CircleCastAll(castPosition, attractRadius, direction, direction.magnitude, enemyLayerMask.value))
        {
            shieldStrength++;

            // TODO: tie into enemies on death event to reduce shield strength.
            // TODO: set strenght number on magnet label.
            Movement enemyMovement = hit.transform.GetComponent<Movement>();
            Vector2 randomOffset = bottomAttractPoint + Random.Range(0.0f, 1.0f) * direction;
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
            while (Mathf.Sign(player.transform.right.x) > 0 ? 
                   player.transform.position.x < transform.position.x : 
                   player.transform.position.x > transform.position.x)
            {
                Vector3 offsetPosition = player.position;
                offsetPosition.x += Mathf.Sign(-player.transform.right.x) * 0.1f;
                movement.Direction = (offsetPosition - transform.position).normalized;
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
        enemyMovement.gameObject.layer = gameObject.layer;
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

    public Sprite GetIconSprite()
    {
        throw new System.NotImplementedException();
    }

    public void Activate(Transform owner)
    {
        player = owner;
        repeatTexture.target = player;

        transform.position = player.position + spawnDistance * player.right; // spawn behind  
        transform.right = player.right;

        StartCoroutine(ActivatePowerUp());
    }
}
