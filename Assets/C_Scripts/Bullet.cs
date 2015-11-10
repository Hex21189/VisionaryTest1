using UnityEngine;
using System.Collections;

/// <summary>
/// Logic for controlling spawned bullets.
/// </summary>
[RequireComponent(typeof(Movement))]
public class Bullet : MonoBehaviour
{
    public Shooter owner; // TODO: point to weapon
    public float lifeSpan = 2.0f;

    private Transform myTransform;
    private Movement movement;
    private float timer;
	
    /// <summary>
    /// Manage bullet life time.
    /// </summary>
    protected void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            owner.bulletPool.ReleaseObject(gameObject);
        }
    }

    /// <summary>
    /// Destory this bullet when it collides with an enemy.
    /// </summary>
    /// <param name="collider">Enemy collider.</param>
    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy")
        {
            owner.bulletPool.ReleaseObject(gameObject);
        }
    }

    /// <summary>
    /// Initialize bullet in random direction.
    /// </summary>
    public void Initialize(Shooter owner)
    {
        this.owner = owner;
        timer = lifeSpan;

        float randomAngle = Mathf.Deg2Rad * Random.Range(-45, 45);

        movement = GetComponent<Movement>();
        movement.Direction = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
    }
}
