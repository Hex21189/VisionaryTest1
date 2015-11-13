using UnityEngine;
using System.Collections;

public class StunOnHit : MonoBehaviour
{
    public HandleInput playerInput;
    public float stunLength = 1.0f;
    public float invincibleRecoveryLength = 1.0f;

    private bool isStunned;

    /// <summary>
    /// Destory this object if it collides with an enemy object. TODO: this 
    /// needs to be generalized to work on multiple characters or objects (for 
    /// example bullets from the enemy).
    /// </summary>
    /// <param name="collider">Enemy collider.</param>
    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (!isStunned)
        {
            StartCoroutine(Stun());
        }
    }

    private IEnumerator Stun()
    {
        isStunned = true;
        playerInput.LockInput = true;

        // TODO: stun anim
        yield return new WaitForSeconds(stunLength);

        // TODO: invincible animation
        playerInput.LockInput = false;

        yield return new WaitForSeconds(invincibleRecoveryLength);
        isStunned = false;
    }
}

