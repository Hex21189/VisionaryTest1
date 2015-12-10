using UnityEngine;

/// <summary>
/// Changes the stats on the target players weapon
/// </summary>
public class ChangeWeaponStats : MonoBehaviour, IPowerUp
{
    public float rateOfFireChange;
    public float scatterChange;

    /// <summary>
    /// Activates this power up by applying changes to ROF and scatter angle.
    /// </summary>
    /// <param name="owner">Player to apply powerup to.</param>
    public void Activate(Transform owner)
    {
        Shooter playerShootingScript = owner.GetComponent<Shooter>();

        if (playerShootingScript != null)
        {
            playerShootingScript.RateOfFire += rateOfFireChange;
            playerShootingScript.ScatterAngle += scatterChange;
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// Unused. This is an instant effect so no iconis needed.
    /// </summary>
    /// <returns>Null.</returns>
    public Sprite GetIconSprite()
    {
        return null;
    }
}
