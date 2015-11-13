using UnityEngine;
using System.Collections;

/// <summary>
/// Attach to entities that are capable of shooting bullets.
/// </summary>
[RequireComponent(typeof(Movement))]
public class Shooter : MonoBehaviour
{
    [Header("Bullet Data")]
    public SimplePool bulletPool;   // TODO: place in weapon class
    public bool shoot;

    [Range(0.0f, 45.0f)]
    public float scatterAngle;

    [Range(0.0f, 2.0f)]
    public float bulletSpawnDelay = 0.08f;

    private float timer = 0;
    private Transform myTransform; // Cached transform for quick look ups in the update loop.
    private Movement movement;

    /// <summary>
    /// Load initial object settings.
    /// </summary>
    protected void Start()
    {
        myTransform = transform;
        movement = GetComponent<Movement>();
    }

    /// <summary>
    /// Handle input and spawn bullets. TODO: Move movement logic to seperate script for moving any character and
    /// move input logic to seperate script.
    /// </summary>
    protected void Update()
    {
        if (timer >= bulletSpawnDelay)
        { 
            if (shoot)
            {
                GameObject spawnedBullet = bulletPool.GetAvailableObject();
                spawnedBullet.GetComponent<Bullet>().Initialize(this, scatterAngle);
                spawnedBullet.transform.position = myTransform.position + (1.05f * myTransform.right);
            }

            timer = 0;
        }

        timer += Time.deltaTime;
      
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnMagnetPowerUp();
        }
    }

    /// <summary>
    /// Destory this object if it collides with an enemy object. TODO: this 
    /// needs to be generalized to work on multiple characters or objects (for 
    /// example bullets from the enemy).
    /// </summary>
    /// <param name="collider">Enemy collider.</param>
    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }

    public GameObject magnetPowerPrefab;
    public float spawnDistance;

    private void SpawnMagnetPowerUp()
    {

    }
}
