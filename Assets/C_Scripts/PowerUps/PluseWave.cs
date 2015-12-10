using UnityEngine;

/// <summary>
/// Creates a wave that makes all enemies face the opposing player.
/// </summary>
[RequireComponent(typeof(Movement))]
public class PluseWave : MonoBehaviour, IPowerUp
{
    public Sprite pulseWaveIcon;
    private Movement movement;

    /// <summary>
    /// Load the components required for this powerup.
    /// </summary>
    protected void Awake()
    {
        movement = GetComponent<Movement>();
    }

    /// <summary>
    /// Redirects any enemies on collion with.
    /// </summary>
    /// <param name="collider">Enemy that collided with the wave.</param>
    protected void OnTriggerEnter2D(Collider2D collider)
    {
        Movement enemyMovement = collider.transform.GetComponent<Movement>();

        if (enemyMovement != null)
        {
            Vector2 direction = enemyMovement.Direction;
            direction.x = Mathf.Abs(direction.x) * Mathf.Sign(movement.Direction.x);
            enemyMovement.Direction = direction;
        }
    }

    /// <summary>
    /// Activates this powerup by setting its movement direction and collision information.
    /// </summary>
    /// <param name="owner">The person using this powerup.</param>
    public void Activate(Transform owner)
    {
        movement.Direction = owner.right;

        transform.position = owner.position;
        gameObject.layer = owner.gameObject.layer;
    }

    /// <summary>
    /// Returns the icon for the pulse wave powerup.
    /// </summary>
    /// <returns>Pulse wave icon.</returns>
    public Sprite GetIconSprite()
    {
        return pulseWaveIcon;
    }
}
