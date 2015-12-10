using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Spawns a new powerup object when this object is killed.
/// </summary>
[RequireComponent(typeof(DeathTrigger))]
public class DropPowerUpOnDeath : MonoBehaviour
{
    public int missChance;
    public List<GameObject> spawnablePowerUps;
    public List<int> dropChances;
    public List<string> ignoreTags;

    private DeathTrigger deathTrigger;
    private Movement movement;
    private bool added;

    /// <summary>
    /// Load all components required to run this script. Validate all parameters.
    /// </summary>
    protected void Awake()
    {
        deathTrigger = GetComponent<DeathTrigger>();
        movement = GetComponent<Movement>();

        if (dropChances.Count != spawnablePowerUps.Count)
        {
            Debug.LogError("There must be a drop chance for every spawnable powerup.");
        }

        added = false;
        deathTrigger.Initialize += Enable;
    }

    /// <summary>
    /// Disables this scripts functionality.
    /// </summary>
    public void Disable()
    {
        if (added)
        {
            added = false;
            deathTrigger.Death -= OnDeath;
        }
    }

    /// <summary>
    /// Enables this scripts functionality.
    /// </summary>
    private void Enable()
    {
        if (!added)
        {
            added = true;
            deathTrigger.Death += OnDeath;
        }
    }

    /// <summary>
    /// Drops a powerup item heading in the same direction at the same speed as the object that dropped it.
    /// </summary>
    /// <param name="victim">The object that was killed.</param>
    /// <param name="killer">The object that killed it. Unused.</param>
    /// <param name="timedOut">True if the object was killed by a timeout event.</param>
    private void OnDeath(Transform victim, Transform killer, bool timedOut)
    {
        if (!timedOut && killer != null && !ignoreTags.Contains(killer.tag))
        {
            int totalWeight = missChance;

            foreach (int chance in dropChances)
            {
                totalWeight += chance;
            }

            int weight = Random.Range(0, totalWeight);

            for (int i = 0; i < dropChances.Count; i++)
            {
                weight -= dropChances[i];

                if (weight <= 0)
                {
                    GameObject spawnedObject = Instantiate(spawnablePowerUps[i]);
                    Movement powerUpMovement = spawnedObject.GetComponent<Movement>();

                    if (powerUpMovement != null && movement != null)
                    {
                        powerUpMovement.transform.position = transform.position;
                        powerUpMovement.Direction = movement.Direction;
                        powerUpMovement.speed = movement.speed;
                    }

                    break;
                }
            }
        }
    }
}
