using UnityEngine;

/// <summary>
/// Activates a powerup instantly when it collides with this object.
/// </summary>
[RequireComponent(typeof(IPowerUp))]
public class ActivatePowerUpOnHit : MonoBehaviour
{
    private IPowerUp powerUp;
    private bool used;
    
    /// <summary>
    /// Load the components required for this script.
    /// </summary>
    protected void Awake()
    {
        powerUp = GetComponent("IPowerUp") as IPowerUp;

        used = false;        
    }

    /// <summary>
    /// Add powerup to players inventory on a collision.
    /// </summary>
    /// <param name="collider">Bullet collider.</param>
    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (!used)
        {
            PlayerPowerUpManager powerUpManager = collider.GetComponent<PlayerPowerUpManager>();

            // Only use on objects that can accept powerups.
            if (powerUpManager != null)
            {
                powerUp.Activate(collider.transform);
                used = true;
            }
        }
    }
}
