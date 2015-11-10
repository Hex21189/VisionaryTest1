using UnityEngine;
using System.Collections;

/// <summary>
/// Logic for controlling spawned bullets.
/// </summary>
public class Bullet : MonoBehaviour
{
    private Transform myTransform;

	/// <summary>
	/// Initialize bullet in random direction.
	/// </summary>
	protected void Start()
    {
        myTransform = transform;
        myTransform.eulerAngles = new Vector3(myTransform.eulerAngles.z, myTransform.eulerAngles.y, Random.Range(-45, 45));
	}
	
	/// <summary>
	/// Move this bullet forward.
    /// TODO: make speed adjustable.
    /// TODO: seperate bullet logic for more interesting bullet movement models.
	/// </summary>
	protected void Update()
    {
        myTransform.position += myTransform.right * 10 * Time.deltaTime; // here
	}

    /// <summary>
    /// Destory this bullet when it collides with an enemy.
    /// </summary>
    /// <param name="collider">Enemy collider.</param>
    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
