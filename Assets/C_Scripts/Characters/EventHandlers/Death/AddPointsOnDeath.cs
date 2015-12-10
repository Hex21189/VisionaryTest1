using UnityEngine;

/// <summary>
/// Adds points to the score of the player that killed this object.
/// </summary>
[RequireComponent(typeof(DeathTrigger))]
public class AddPointsOnDeath : MonoBehaviour
{
    public int points;

    private DeathTrigger deathTrigger;

    /// <summary>
    /// Load all components required to run this script.
    /// </summary>
    protected void Awake()
    {
        deathTrigger = GetComponent<DeathTrigger>();
        deathTrigger.Death += OnDeath;
    }

    /// <summary>
    /// Add points to the owner of the bullet object that killed this object.
    /// </summary>
    /// <param name="victim">This object. Unused.</param>
    /// <param name="killer">The bullet that killed this object.</param>
    /// <param name="timedOut">True if the death was caused by a timeout.</param>
    private void OnDeath(Transform victim, Transform killer, bool timedOut)
    {
        if (!timedOut && killer != null)
        {
            if (killer.tag == "Bullet")
            {
                Bullet bullet = killer.GetComponent<Bullet>();
                PlayerScore score = bullet.owner.GetComponent<PlayerScore>();
                score.AddPoints(points);
            }
        }
    }
}

