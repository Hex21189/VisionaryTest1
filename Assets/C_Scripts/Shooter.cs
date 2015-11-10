using UnityEngine;
using System.Collections;

/// <summary>
/// Attach to entities that are capable of shooting bullets.
/// </summary>
public class Shooter : MonoBehaviour
{
    public GameObject bulletPrefab;

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
            if (Input.GetButton("fire"))
            {
                GameObject spawnedBullet = GameObject.Instantiate(bulletPrefab);
                spawnedBullet.transform.position = myTransform.position + (1.05f * myTransform.right);
            }

            timer = 0;
        }

        timer += Time.deltaTime;

        if (Input.GetButton("Up"))
        {
            myTransform.position += new Vector3(0, 2 * Time.deltaTime, 0);
        }
        if (Input.GetButton("Down"))
        {
            myTransform.position -= new Vector3(0, 2 * Time.deltaTime, 0);
        }
    }

    /// <summary>
    /// Destory this object if it collides with an enemy object. TODO: this needs to be generalized to work on multiple characters.
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
