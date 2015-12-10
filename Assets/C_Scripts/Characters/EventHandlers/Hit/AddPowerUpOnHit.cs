using UnityEngine;

/// <summary>
/// Adds a powerup to the players inventory on hit.
/// </summary>
public class AddPowerUpOnHit : MonoBehaviour
{
    public GameObject powerUpPrefab;
    private bool used;

    /// <summary>
    /// Initialize the script.
    /// </summary>
    protected void Awake()
    {
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

            if (powerUpManager != null)
            {
                powerUpManager.StoredPowerUp = powerUpPrefab;
                used = true;
            }
        }
    }
}
