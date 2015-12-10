using UnityEngine;

/// <summary>
/// Removes a player from the game when their home planet is destroied.
/// </summary>
[RequireComponent(typeof(DeathTrigger))]
[RequireComponent(typeof(Planet))]
public class RemovePlayerOnPlanetDeath : MonoBehaviour
{
    public GameManager gameManager;
    public HandleInput playerInput;
    public Animator playerAnimator;

    private DeathTrigger deathTrigger;
    private PlayerAi ai;

    /// <summary>
    /// Load all components required for this script and validate the parameters.
    /// </summary>
    protected void Awake()
    {
        if (gameManager == null)
        {
            Debug.LogError("Cannot end game without a game manager linked to all planets.");
        }

        deathTrigger = GetComponent<DeathTrigger>();
        deathTrigger.Death += OnDeath;

        ai = playerInput.GetComponent<PlayerAi>();
    }

    /// <summary>
    /// Remove the player from the game when this object is killed.
    /// </summary>
    /// <param name="victim">This object.</param>
    /// <param name="killer">The object that killed this object.</param>
    /// <param name="timedOut">True if the death was timeout related.</param>
    private void OnDeath(Transform victim, Transform killer, bool timedOut)
    {
        if (!timedOut)
        {
            playerInput.LockInput = true;
            playerAnimator.SetBool("Dead", true);

            if (ai != null)
            {
                ai.enabled = false;
            }

            gameManager.RemovePlayerByPlanet(transform);
        }
    }
}
