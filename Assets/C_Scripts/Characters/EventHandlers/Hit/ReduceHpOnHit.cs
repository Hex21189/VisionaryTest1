using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Reduces an objects hit points when it hits something else with Hp.
/// </summary>
[RequireComponent(typeof(Hp))]
public class ReduceHpOnHit : MonoBehaviour
{
    public List<string> ignoreTags;

    private DeathTrigger deathTrigger;
    private Hp hp;

	/// <summary>
    /// Loads the scripts required for this function.
    /// </summary>
	protected void Awake()
    {
        hp = GetComponent<Hp>();
	}
	
    /// <summary>
    /// Reduces HP when it collides with another object that tracks HP.
    /// </summary>
    /// <param name="collider">Object that may or may not have an HP component.</param>
	protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (!ignoreTags.Contains(collider.tag))
        {
            Hp enemyHp = collider.GetComponent<Hp>();

            if (enemyHp != null)
            {
                hp.ReduceHp(enemyHp.transform, enemyHp.CurrentHp);
            }
        }
    }
}
