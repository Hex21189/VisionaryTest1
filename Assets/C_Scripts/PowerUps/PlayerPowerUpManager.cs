using UnityEngine;

/// <summary>
/// Manages the players powerup inventory.
/// </summary>
public class PlayerPowerUpManager : MonoBehaviour
{
    public PlayerUi ui;

    private GameObject storedPowerUp;

    /// <summary>
    /// Activates the stored powerup on the given player.
    /// </summary>
    /// <param name="player">Player that is affected by the powerup.</param>
    public void ActivatePowerUp(Transform player)
    {
        if (storedPowerUp != null)
        {
            GameObject powerUpObject = Instantiate(storedPowerUp);
            IPowerUp powerUp = powerUpObject.GetComponent("IPowerUp") as IPowerUp;

            if (powerUp != null)
            {
                powerUp.Activate(player);
            }

            StoredPowerUp = null;
        }
    }

    /// <summary>
    /// Getter and setter for the powerup the player is storing in their inventory. Overwrites existing their previously held powerup.
    /// </summary>
    public GameObject StoredPowerUp
    {
        get
        {
            return storedPowerUp;
        }
        set
        {
            storedPowerUp = value;

            if (storedPowerUp != null)
            {
                IPowerUp uninitializedPowerUp = storedPowerUp.GetComponent<IPowerUp>();
                ui.PlayerPowerUpIcon = uninitializedPowerUp.GetIconSprite();
            }
            else
            {
                ui.PlayerPowerUpIcon = null;
            }
        }
    }
}
