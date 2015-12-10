using UnityEngine;

/// <summary>
/// Increases the spawn rate of the attached spawner every time the timer hits 0. 
/// Timer resets after it is hit.
/// </summary>
[RequireComponent(typeof(Spawn))]
public class IncreaseSpawnRateOnTick : MonoBehaviour
{
    public float increaseTimer = 30;
    public float increaseMultiplier = 1.5f;

    private Spawn spawner;
    private float timer;
	
    /// <summary>
    /// Load all components required to run this script.
    /// </summary>
	protected void Awake()
    {
        spawner = GetComponent<Spawn>();
        timer = increaseTimer;
	}
	
	/// <summary>
    /// Count down the timer and increase the minimum and maximum spawn count when if it hits 0.
    /// Timer will then reset and the count down begins again.
    /// </summary>
	protected void Update()
    {
        timer -= Time.deltaTime;

	    if (timer <= 0)
        {
            spawner.maxSpawn = Mathf.RoundToInt(increaseMultiplier * spawner.maxSpawn + 0.5f);
            spawner.minSpawn = Mathf.RoundToInt(increaseMultiplier * spawner.minSpawn + 0.5f);

            timer = increaseTimer;
        }
	}
}
