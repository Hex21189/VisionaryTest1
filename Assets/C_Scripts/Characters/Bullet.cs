using UnityEngine;
using System.Collections;

/// <summary>
/// Logic for controlling spawned bullets.
/// </summary>
[RequireComponent(typeof(DeathTrigger))]
[RequireComponent(typeof(Movement))]
public class Bullet : MonoBehaviour
{
    public Shooter owner;
    public float initialSpeed;

    private Transform myTransform;
    private Movement movement;
    private DeathTrigger deathTrigger;

    /// <summary>
    /// Load all components required for this script.
    /// </summary>
    protected void Awake()
    {
        deathTrigger = GetComponent<DeathTrigger>();
        movement = GetComponent<Movement>();
    }

    /// <summary>
    /// Initialize bullet in random direction.
    /// </summary>
    public void Initialize(Shooter owner, float scatterAngle)
    {
        this.owner = owner;
        deathTrigger.Pool = owner.bulletPool;
        deathTrigger.OnInitialize();
        movement.speed = initialSpeed;
        movement.Lock = false;

        if (scatterAngle > 0)
        {
            float randomAngle = Mathf.Deg2Rad * Random.Range(-scatterAngle, scatterAngle);
            movement.Direction =  new Vector2(owner.transform.right.x * Mathf.Cos(randomAngle), transform.up.y * Mathf.Sin(randomAngle));
        }
        else
        {
            movement.Direction = owner.transform.right;
        }
    }
}
