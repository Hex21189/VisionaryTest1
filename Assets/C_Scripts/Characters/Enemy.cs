using UnityEngine;

/// <summary>
/// Script for controlling enemy setup.
/// Note: Remove requirements if animation and movement is optional in the future.
///       For now it's required because things look better this way.
/// </summary>
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    public float initialSpeed;
    public int enemyLayer = 8; // initial collision layer to intialize enemy on.

	private Animator animator;
    private Movement movement;
    private DeathTrigger deathTrigger; // death is optional since some enemies could be invincible

    /// <summary>
    /// Load objects required components.
    /// </summary>
    protected void Awake()
    {
		animator     = GetComponent<Animator>();
        deathTrigger = GetComponent<DeathTrigger>();
        movement     = GetComponent<Movement>();
    }

    /// <summary>
    /// Initialize this enemy and set its movement to the target direction.
    /// </summary>
    /// <param name="direction">Which direction should we be traveling in at the start.</param>
    /// <param name="pool">The optional pool to release this enemy too. Using this can increase performance.</param>
    public void Initialize(Vector3 direction, SimplePool pool = null)
    {
		animator.SetBool("Dead", false);
        gameObject.layer = enemyLayer;

        movement.Direction = direction;
        movement.speed = initialSpeed;
        movement.Lock = false;

        // Reset death events if necessary
        if (deathTrigger != null)
        {
            deathTrigger.Pool = pool;
            deathTrigger.OnInitialize();
        }
    }
}
