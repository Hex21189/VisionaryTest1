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
    private Spawn owner;
    private float timer;    // CONSIDER: it may be better to destory the enemies when they enter a trigger that catches them rather than lifespan.

    /// <summary>
    /// Load initial object settings.
    /// </summary>
    protected void Awake()
    {
        movement = GetComponent<Movement>();
	}

    /// <summary>
    /// Manage bullet life time.
    /// TODO: create object pooling to avoid destorying and reinitiating.
    /// </summary>
    protected void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && owner != null)
        {
            owner.enemyPool.ReleaseObject(gameObject);
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
        if (owner != null)
        {
            owner.enemyPool.ReleaseObject(gameObject);
        }
    }

    public void Initialize(Spawn owner, Vector3 direction)
    {
        this.owner = owner;

        // TODO: this should be randmized later to make this game less boring. Also move to AI script
        movement.Direction = direction;

        timer = lifeSpan;
    }
}
