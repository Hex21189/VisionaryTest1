using UnityEngine;
using System.Collections;

/// <summary>
/// Attach to entities that are capable of shooting bullets.
/// </summary>
[RequireComponent(typeof(Movement))]
public class Shooter : MonoBehaviour
{
    public GameObject bulletPrefab;

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
            if (Input.GetButton("fire"))
            {
                GameObject spawnedBullet = GameObject.Instantiate(bulletPrefab);
                spawnedBullet.transform.position = myTransform.position + (1.05f * myTransform.right);
            }

            timer = 0;
        }

        timer += Time.deltaTime;
        Vector2 moveDirection = new Vector2();

        if (Input.GetButton("Up"))
        {
            moveDirection.y = 1;
        }
        if (Input.GetButton("Down"))
        {
            moveDirection.y = -1;
        }
        if (Input.GetButton("Left"))
        {
            moveDirection.x = -1;
        }
        if (Input.GetButton("Right"))
        {
            moveDirection.x = 1;
        }

        movement.Direction = moveDirection;
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
}
