using UnityEngine;

/// <summary>
/// Loads the assigned to this script. Also sets AI requirements. Should only be used when loading the 
/// battle scene however it could be used with any scene.
/// </summary>
public class LoadLevel : MonoBehaviour
{
    public static string VS_AI_KEY = "VS AI";

    public string gameLevelName = "Battle Scene";

    /// <summary>
    /// Reloads the game level and configures the oppenents AI.
    /// </summary>
    /// <param name="vsAi">True if we should use an AI enemy.</param>
    public void LoadGame(bool vsAi)
    {
        if (vsAi)
        {
            PlayerPrefs.SetInt(VS_AI_KEY, 1);
        }
        else
        {
            PlayerPrefs.DeleteKey(VS_AI_KEY);
        }

        Application.LoadLevel(gameLevelName);
    }
}
