using UnityEngine;

/// <summary>
/// Attach to spawner to create new enemies for the game after every spawn interval has been reached.
/// </summary>
public class Spawn : MonoBehaviour
{
    public SimplePool enemyPool;

    [Header("Spawner Boundaries")]
    public float maxY;
    public float minY;
    public float maxXDist;
    public float minXDist;

    [Header("Wave Stats")]
    public int maxSpawn;
    public int minSpawn;

    [Range(0.0f, 1.0f)]
    public float spawnDirectionProbability;
    [Range(0.0f, 0.1f)]
    public float directionChangePerKill;

    [Range(0.1f, 100.0f)]
    public float spawnInterval = 1.0f;
    private float timer = 0;

    /// <summary>
    /// Draws the possible spawn area to the inspector window.
    /// </summary>
    protected void OnDrawGizmos()
    {
        Vector2 size = new Vector2(Mathf.Abs(maxXDist - minXDist), Mathf.Abs(maxY - minY));
        Gizmos.DrawWireCube(transform.position, size);
    }

    /// <summary>
    /// Spawn a new wave of enemies every time the timer completes.
    /// </summary>
    protected void Update()
    {
        if (timer >= spawnInterval)
        {
            int numEnemies = Random.Range(minSpawn, maxSpawn + 1);

            for (var i = 0; i < numEnemies; ++i)
            {
                SpawnEnemy();
            }

            timer = 0;
        }

        timer += Time.deltaTime;
	}
	
    /// <summary>
    /// Adjusts the direction we spawn objects heading in based on which way this object was going when it was killed.
    /// This makes enemies seem smarter by heading in the path of least resistance.
    /// </summary>
    /// <param name="victim">The killed game object/enemy.</param>
    /// <param name="killer">The bullet that killed this object.</param>
    /// <param name="timedOut">True if we timed out and should ignore this death.</param>
    private void AdjustSpawnDirection(Transform victim, Transform killer, bool timedOut)
    {
        DeathTrigger deathTrigger = victim.GetComponent<DeathTrigger>();
        deathTrigger.Death -= AdjustSpawnDirection;

        if (!timedOut && killer != null && killer.tag == "Bullet")
        {
            Movement movement = victim.GetComponent<Movement>();
            spawnDirectionProbability = Mathf.Clamp01(spawnDirectionProbability + (movement.Direction.x < 0 ? directionChangePerKill : -directionChangePerKill));
        }
    }

    /// <summary>
    /// Spawns a single random enemy.
    /// </summary>
    private void SpawnEnemy()
    {
        GameObject spawned = enemyPool.GetAvailableObject();
        Vector2 direction = new Vector2(Random.Range(2.0f, 3.0f), Random.Range(-1.0f, 1.0f));

        // Go towards player 1
        if (Random.Range(0.0f, 1.0f) > spawnDirectionProbability)
        {
            direction.x *= -1;
        }

        spawned.GetComponent<Enemy>().Initialize(direction, enemyPool);
        spawned.transform.position = new Vector3(transform.position.x + Mathf.Sign(direction.x) * Random.Range(minXDist, maxXDist), 
		                                         transform.position.y + Random.Range(minY, maxY));
        spawned.transform.parent = null;

        DeathTrigger deathTrigger = spawned.GetComponent<DeathTrigger>();
        deathTrigger.Death += AdjustSpawnDirection;
    }
}
