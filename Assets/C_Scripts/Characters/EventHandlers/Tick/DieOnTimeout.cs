using UnityEngine;

/// <summary>
/// Destory this object after a certain time limit has been reached.
/// </summary>
[RequireComponent(typeof(DeathTrigger))]
public class DieOnTimeout : MonoBehaviour
{
    public float initialLifeSpan;

    private DeathTrigger deathTrigger;
    private float timer;
    
    /// <summary>
    /// Load all components required to run the time out logic.
    /// </summary>
    protected void Awake()
    {
        deathTrigger = GetComponent<DeathTrigger>();

        deathTrigger.Initialize += Initialize;
    }

    /// <summary>
    /// Counts down the timer and triggers the death function if the timer has reached 0.
    /// </summary>
    protected void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            deathTrigger.OnDeath(null, true);
        }
    }

    /// <summary>
    /// Initializes the timer.
    /// </summary>
    public void Initialize()
    {
        timer = initialLifeSpan;
    }
}
