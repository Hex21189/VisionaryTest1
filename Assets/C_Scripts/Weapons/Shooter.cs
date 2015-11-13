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

    /// <summary>
    /// Load initial object settings.
    /// </summary>
    protected void Start()
    {
        myTransform = transform;
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
    }
}
