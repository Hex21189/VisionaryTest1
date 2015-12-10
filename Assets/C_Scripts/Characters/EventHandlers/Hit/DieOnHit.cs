using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Triggers the death event when this object collides with another object.
/// </summary>
[RequireComponent(typeof(DeathTrigger))]
public class DieOnHit : MonoBehaviour
{
    public List<string> ignoreTags;

    private DeathTrigger deathTrigger;
    private bool dieing;   
    
    /// <summary>
    /// Load the components required for this script.
    /// </summary>
    protected void Awake()
    {
        deathTrigger = GetComponent<DeathTrigger>();
    }

    /// <summary>
    /// Destroy this enemy when it collides with a bullet object.
    /// </summary>
    /// <param name="collider">Bullet collider.</param>
    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (!string.IsNullOrEmpty(collider.tag) && !ignoreTags.Contains(collider.tag))
        {
            deathTrigger.OnDeath(collider.transform, false);
        }
    }
}
