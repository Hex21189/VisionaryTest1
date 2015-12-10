using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Destroies or releases this object when it is killed.
/// </summary>
[RequireComponent(typeof(DeathTrigger))]
public class DestroyOnDeath : MonoBehaviour
{
	public List<AudioClip> deathSounds;
    public AnimationClip deathAnimation;

    private DeathTrigger deathTrigger;
	private AudioSource audioSource;
	private Animator animator;
	private Movement movement;

    /// <summary>
    /// Load all components required to run this script.
    /// </summary>
    protected void Awake()
    {
        deathTrigger = GetComponent<DeathTrigger>();
        audioSource = GetComponent<AudioSource>();
        movement = GetComponent<Movement>();
        animator = GetComponent<Animator>();

        deathTrigger.Death += OnDeath;
    }

    /// <summary>
    /// Kills this object by stopping its movement, playing its death animations, playing a random death sounds associated
    /// with it, and finally releasing it back to its object pool or simplying destroying it. All effects are skipped if the
    /// object has timed out.
    /// </summary>
    /// <param name="timedOut">True if timed out.</param>
    /// <returns>Time delays.</returns>
    private IEnumerator DieEffects(bool timedOut)
    {
        if (!timedOut)
        {
            if (movement != null)
            {
                movement.Lock = true;
            }

            float waitTime = 0;

            if (animator != null && deathAnimation != null)
            {
                animator.SetBool("Dead", true);
                waitTime = Mathf.Max(waitTime, deathAnimation.length);
            }

            if (audioSource != null && deathSounds.Count > 0)
            {
                AudioClip deathSound = deathSounds[Random.Range(0, deathSounds.Count)];
                audioSource.PlayOneShot(deathSound);
                waitTime = Mathf.Max(waitTime, deathSound.length);
            }

            yield return new WaitForSeconds(waitTime);
        }

        if (deathTrigger.Pool != null)
        {
            deathTrigger.Pool.ReleaseObject(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Begins displaying all visual and logical effects required for showing this object dieing.
    /// </summary>
    /// <param name="victim">This object. Unused.</param>
    /// <param name="killer">The object that killed this one. Unused.</param>
    /// <param name="timedOut">True if this object timed out.</param>
    private void OnDeath(Transform victim, Transform killer, bool timedOut)
    {
        StartCoroutine(DieEffects(timedOut));
    }
}
