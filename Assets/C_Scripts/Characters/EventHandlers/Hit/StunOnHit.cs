using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Allows an object to be temporarily stunned when an object enters it's trigger.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class StunOnHit : MonoBehaviour
{
    public float stunLength = 1.0f;
    public float invincibleRecoveryLength = 1.0f;

    public AudioClip stunSound;
    public List<string> ignoreTags;
    public ParticleSystem stunParticleEmitter;

    private Animator animator;
    private AudioSource audioSource;
    private bool isStunned;
    private Movement shipMovement;
    private Shooter shipShooter;

    /// <summary>
    /// Load the components required for this script.
    /// </summary>
    protected void Awake()
	{
        animator     = GetComponent<Animator>();
		audioSource  = GetComponent<AudioSource>();
        shipMovement = GetComponent<Movement>();
        shipShooter  = GetComponent<Shooter>();
	}

    /// <summary>
    /// Destory this object if it collides with an enemy object. TODO: this 
    /// needs to be generalized to work on multiple characters or objects (for 
    /// example bullets from the enemy).
    /// </summary>
    /// <param name="collider">Enemy collider.</param>
    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (!isStunned && !ignoreTags.Contains(collider.tag))
        {
            StartCoroutine(Stun());
        }
    }

    /// <summary>
    /// Stuns the player temporarily and then grants them a short period of 
    /// invincibility to get out of the way of enemy fire.
    /// </summary>
    /// <returns>Time delays.</returns>
    private IEnumerator Stun()
    {
        isStunned = true;

        if (shipMovement != null)
        {
            shipMovement.Lock = true;
        }

        if (shipShooter != null)
        {
            shipShooter.Lock = true;
        }

		audioSource.PlayOneShot(stunSound);

        if (stunParticleEmitter != null)
        {
            stunParticleEmitter.Play();
        }

        if (animator != null)
        {
            animator.SetBool("Invincible", true);
        }

        yield return new WaitForSeconds(stunLength);

        if (shipMovement != null)
        {
            shipMovement.Lock = false;
        }

        if (shipShooter != null)
        {
            shipShooter.Lock = false;
        }

        yield return new WaitForSeconds(invincibleRecoveryLength);

        if (animator != null)
        {
            animator.SetBool("Invincible", false);
        }

        isStunned = false;
    }
}

