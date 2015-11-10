using UnityEngine;
using System.Collections;

/// <summary>
/// Attach to spawner to create new enemies for the game after every spawn interval has been reached.
/// </summary>
public class Spawn : MonoBehaviour
{
    public GameObject enemy;

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
            int numEnemies = Random.Range(4, 30);

            for (var i = 0; i < numEnemies; ++i)
            {
                GameObject spawned = GameObject.Instantiate(enemy);
                spawned.transform.position = transform.position + Vector3.down * Random.Range(0, 10) + Vector3.left * Random.Range(-2, 2);
            }

            timer = 0;
        }

        timer += Time.deltaTime;
	}
    
    /// <summary>
    /// Destories the Spawner when colliding with a bullet.
    /// TODO: Remove this because it's dumb.
    /// </summary>
    /// <param name="collider">Colliding bullet.</param>
    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Bullet")
        {
            Destroy(collider.gameObject);
        }
    }
}
