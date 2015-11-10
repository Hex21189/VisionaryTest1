using UnityEngine;
using System.Collections;

/// <summary>
/// Script for controlling enemies AI and stats.
// TODO: seperate this into multiple parts for easy enemy creation
/// </summary>
public class Enemy : MonoBehaviour
{
    private Transform myTransform; // Cached transform for quick look ups in the update loop.
    private float randomOffset; // TODO: looks unnecessary

    /// <summary>
    /// Load initial object settings.
    /// </summary>
    protected void Start()
    {
        myTransform = transform;
        randomOffset = Random.Range(0f, 1f);
	}
	
	/// <summary>
	/// Move this Enemy south west.
    /// TODO: Seperate movement logic.
    /// TODO: Make the direction configurable and not fixed so it the movement logic can be used for multiple enemies.
	/// </summary>
	protected void Update()
    {
        myTransform.position -= myTransform.right * 3 * Time.deltaTime;
        myTransform.position += Mathf.Sign(Time.time + randomOffset) * Vector3.down * Time.deltaTime;
	}

    /// <summary>
    /// Desotry this enemy when it collides with a bullet object.
    /// </summary>
    /// <param name="collider">Bullet collider.</param>
    protected void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Bullet")
        {
            Destroy(gameObject);
        }
    }
}
