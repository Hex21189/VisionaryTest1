using UnityEngine;
using System.Collections;

/// <summary>
/// Logic for controlling spawned bullets.
/// </summary>
[RequireComponent(typeof(Movement))]
public class Bullet : MonoBehaviour
{
    private Transform myTransform;
    private Movement movement;

	/// <summary>
	/// Initialize bullet in random direction.
	/// </summary>
	protected void Start()
    {
        float randomAngle = Mathf.Deg2Rad * Random.Range(-45, 45);
        
        movement = GetComponent<Movement>();
        movement.Direction = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
        //myTransform.eulerAngles = new Vector3(myTransform.eulerAngles.z, myTransform.eulerAngles.y, Random.Range(-45, 45));
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
