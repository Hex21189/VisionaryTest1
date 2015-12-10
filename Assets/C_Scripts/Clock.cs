using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays the game time on screen.
/// </summary>
public class Clock : MonoBehaviour
{
    public Text clockDisplay;
    private float time = 0;

    /// <summary>
    /// Update the clock display.
    /// </summary>
	void Update()
    {
        time += Time.deltaTime;
        clockDisplay.text = string.Format("{0:00}:{1:00}", time / 60, time % 60);
	}
}
