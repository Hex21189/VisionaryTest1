using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Logic for managing the magnetic shield power up effect.
/// </summary>
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(RepeateTexture))]
public class MagnetShield : MonoBehaviour, IPowerUp
{
    #region Fields

    public Transform player;
    public Sprite icon;

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

    private List<Transform> capturedEnemies;
    private bool triggerMagnetEffects;
    private bool triggerEffectsFininished;
    private Movement movement;
    private RepeateTexture repeatTexture;

    #endregion

    #region Unity Life Cycle Functions

    /// <summary>
    /// Initialize the magnet power and launch the behavior logic.
    /// </summary>
    protected void Awake()
    {
        capturedEnemies = new List<Transform>();
        triggerMagnetEffects = false;
        triggerEffectsFininished = false;

        movement = GetComponent<Movement>();
        repeatTexture = GetComponent<RepeateTexture>();
	}

    /// <summary>
    /// Draws the line which enemies are placed on.
    /// </summary>
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
			DeathTrigger trigger = collider.transform.GetComponent<DeathTrigger>();

			// Don't capture someone who is dieing
			if (trigger != null && !trigger.Dieing)
			{
                triggerMagnetEffects = true;
			}
        }
    }

    /// <summary>
    /// Check to see if we can destory the shield on every frame incase the on death function doesn't work properly.
    /// </summary>
    protected void Update()
    {
        if (triggerEffectsFininished && capturedEnemies.Count <= 0)
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Activates all effects required for this powerup. The powerup will send out a magnet object that will
    /// grab all enemies nearby when it collides with an enemy. After it grabs enemies it will retract to the
    /// front of the player. If nothing could be grabbed it is just destroied.
    /// </summary>
    /// <param name="owner">The player using this powerup.</param>
    public void Activate(Transform owner)
    {
        player = owner;
        repeatTexture.target = player;

        transform.position = player.position + spawnDistance * player.right; // spawn behind  
        transform.right = player.right;

        StartCoroutine(ActivatePowerUp());
    }

    /// <summary>
    /// Returns the ui icon for this power.
    /// </summary>
    /// <returns>Returns powerup icon.</returns>
    public Sprite GetIconSprite()
    {
        return icon;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Starts up the powerups logic. The magnet power up will fist extend until it hits an enemy or goes to far. 
    /// It will then retract itself until it is near the player and will capture enemies to use as a shield for the player.
    /// </summary>
    /// <returns>Time delay.</returns>
    private IEnumerator ActivatePowerUp()
    {
        Vector3 startPosition = transform.position;

        // Wait to be in range
        while (!triggerMagnetEffects && Vector3.Distance(startPosition, transform.position) < maxTravelDistance)
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
            Hp enemyHp = hit.transform.GetComponent<Hp>();

            if (enemyHp != null && enemyHp.CurrentHp > 0)
            {
                capturedEnemies.Add(hit.transform);
            }
        }

        foreach (Transform enemy in capturedEnemies)
        {
            // Stop timeout
            DieOnTimeout timeout = enemy.GetComponent<DieOnTimeout>();
            timeout.enabled = false;

            // Reduce shield strength on death
            DeathTrigger onDeath = enemy.GetComponent<DeathTrigger>();
            onDeath.Death += ReduceShieldStrength;

            // Shielding enemies can't drop power ups
            DropPowerUpOnDeath dropPowerUp = enemy.GetComponent<DropPowerUpOnDeath>();
            dropPowerUp.Disable();

            string myTag = "Shield" + player.name;

            // Don't reduce hp when collidingwith other captured enemies
            ReduceHpOnHit hp = enemy.GetComponent<ReduceHpOnHit>();
            hp.ignoreTags.Add(myTag);

            // Don't collide with other captured enemies
            DieOnHit hitDetection = enemy.GetComponent<DieOnHit>();
            hitDetection.ignoreTags.Add(myTag);

            enemy.tag = myTag;
            enemy.gameObject.layer = player.gameObject.layer;

            Movement enemyMovement = enemy.GetComponent<Movement>();
            Vector2 randomOffset = bottomAttractPoint + Random.Range(0.0f, 1.0f) * direction;
            StartCoroutine(MoveEnemyToShieldPoint(enemyMovement, randomOffset));
        }     

        triggerEffectsFininished = true;

        if (capturedEnemies.Count > 0)
        {
            // Move magnet to front of player
            bool wasPositive = Mathf.Sign(Vector2.Distance(transform.position, player.position) - distanceFromPlayer) >= 0;
            bool isPositive = wasPositive;

            while ((wasPositive && isPositive) || (!wasPositive && !isPositive))
            {
                Vector3 offsetPosition = player.position;
                offsetPosition.x += Mathf.Sign(player.transform.right.x) * distanceFromPlayer;
                movement.Direction = (offsetPosition - transform.position).normalized;
                wasPositive = isPositive;
                yield return null;

                float dist = Vector2.Distance(transform.position, player.position) - distanceFromPlayer;
                isPositive = Mathf.Sign(dist) >= 0;
            }

            transform.GetComponent<Collider2D>().enabled = false;
            movement.Direction = Vector2.zero;

            transform.SetParent(player);
        }
        else
        {
            // Destroy the shield since it has no enemies attached.
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
    /// <returns>Time delay.</returns>
    private IEnumerator MoveEnemyToShieldPoint(Movement enemyMovement, Vector2 magnetOffset)
    {
        enemyMovement.transform.parent = transform;
        Vector2 targetPosition = (Vector2)transform.position + Mathf.Sign(player.transform.right.x) * magnetOffset;

        while (Vector2.Distance(enemyMovement.transform.position, targetPosition) > 0.5f && enemyMovement.gameObject.activeSelf)
        {
            enemyMovement.Direction = targetPosition - (Vector2)enemyMovement.transform.position;
            enemyMovement.speed = attractSpeed;

            targetPosition = (Vector2)transform.position + Mathf.Sign(player.transform.right.x) * magnetOffset;
            yield return null;
        }

        enemyMovement.Direction = Vector2.zero;
        enemyMovement.speed = 0;
    }

    /// <summary>
    /// Reduced the strenght of the shield and will despawn the magnet if there is no strength left.
    /// </summary>
    private void ReduceShieldStrength(Transform victim, Transform killer, bool timedOut)
    {
        capturedEnemies.Remove(victim);
        victim.tag = "Enemy";

        // Restart timeout
        DieOnTimeout timeout = victim.GetComponent<DieOnTimeout>();
        timeout.enabled = true;

        // Reduce shield strength on death
        DeathTrigger onDeath = victim.GetComponent<DeathTrigger>();
        onDeath.Death -= ReduceShieldStrength;
    }

    #endregion
}
