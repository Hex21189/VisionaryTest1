using UnityEngine;
using System.Collections;

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

    [Range(0.1f, 100.0f)]
    public float spawnInterval = 1.0f;
    private float timer = 0;

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

    protected void OnTrigger2DEnter(Collider2D collider)
    {
        // TODO: if collides with bullet, push back.
        // TODO: if collides with planet destroy ship and planet
    }

    private void SpawnEnemy()
    {
        GameObject spawned = enemyPool.GetAvailableObject();
        Vector2 direction = new Vector2(Random.Range(2.0f, 3.0f), Random.Range(-1.0f, 1.0f));

        // Go towards player 1
        if (Random.Range(0.0f, 1.0f) > spawnDirectionProbability)
        {
            direction.x *= -1;
        }

        spawned.GetComponent<Enemy>().Initialize(this, direction);
        spawned.transform.position = transform.position + 
                                     new Vector3(Mathf.Sign(direction.x) * Random.Range(minXDist, maxXDist), Random.Range(minY, maxY));
    }
}
