using UnityEngine;
using System.Collections;

/// <summary>
/// Script for controlling enemies AI and stats.
// TODO: seperate this into multiple parts for easy enemy creation
/// </summary>
[RequireComponent(typeof(Movement))]
public class Enemy : MonoBehaviour
{
    public float lifeSpan = 2.0f;
    private Movement movement;

    /// <summary>
    /// Load initial object settings.
    /// </summary>
    protected void Start()
    {
        movement = GetComponent<Movement>();

        Vector2 tempDirection = new Vector2(-3, -1);
        movement.Direction = tempDirection;
	}

    /// <summary>
    /// Manage bullet life time.
    /// TODO: create object pooling to avoid destorying and reinitiating.
    /// </summary>
    protected void Update()
    {
        lifeSpan -= Time.deltaTime;

        if (lifeSpan <= 0)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Desotry this enemy when it collides with a bullet object.
    /// TODO: create object pooling to avoid destorying and reinitiating.
    /// TODO: destory enemies that are too far away and don't add their score.
    /// </summary>
    /// <param name="collider">Bullet collider.</param>
    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Bullet")
        {
            Destroy(gameObject);
        }
    }
}
