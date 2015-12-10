using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Pushes this game object along the x coordinate plane when it collides with another game object.
/// </summary>
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(DeathTrigger))]
[RequireComponent(typeof(Movement))]
public class PushBackOnHit : MonoBehaviour
{
    public string tagEffected = "Bullet";
    public List<AudioClip> hitSounds;
    public float pushbackPerHit;
    public float pushBackFalloff;

    private AudioSource audioSource;
    private DeathTrigger deathTrigger;
    private Movement movement;

    /// <summary>
    /// Initialize all components this script requires
    /// </summary>
    protected void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        deathTrigger = GetComponent<DeathTrigger>();
        movement = GetComponent<Movement>();
    }

    /// <summary>
    /// Decay pushback until this object returns to pure y coordinate plane movement.
    /// </summary>
    protected void Update()
    {
        if (!deathTrigger.Dieing)
        {
            Vector2 direction = movement.Direction;
            direction.x = Mathf.Clamp(direction.x - Mathf.Sign(direction.x) * Time.deltaTime * pushBackFalloff, -1, 1);
            movement.Direction = direction;
        }
    }

    /// <summary>
    /// On hit, if the objects tag matches the tag listed in the inspector then push the object towards the 
    /// direction the colliding object was traveling in.
    /// </summary>
    /// <param name="collider">Object colliding with this one. Requires a movement component for successful pushback.</param>
    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (!deathTrigger.Dieing)
        {
            if (collider.tag == tagEffected)
            {
                audioSource.PlayOneShot(hitSounds[Random.Range(0, hitSounds.Count)]);
                Movement bulletMovement = collider.GetComponent<Movement>();

                if (movement)
                {
                    Vector2 direction = movement.Direction;
                    direction.x += Mathf.Sign(bulletMovement.Direction.x) * pushbackPerHit;
                    movement.Direction = direction;
                }
            }
        }
    }
}
