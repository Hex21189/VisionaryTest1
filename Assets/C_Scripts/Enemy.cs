using UnityEngine;
using System.Collections;

/// <summary>
/// Script for controlling enemies AI and stats.
// TODO: seperate this into multiple parts for easy enemy creation
/// </summary>
[RequireComponent(typeof(Movement))]
public class Enemy : MonoBehaviour
{
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
    /// Desotry this enemy when it collides with a bullet object.
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
