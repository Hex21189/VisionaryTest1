using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the state of the current game.
/// </summary>
[RequireComponent(typeof(EndGame))]
public class GameManager : MonoBehaviour
{
    public List<PlayerData> players;
    private EndGame endGame;

    /// <summary>
    /// Verify enough players are initialized in the game.
    /// </summary>
    protected void Awake()
    {
        if (players.Count <= 1)
        {
            Debug.LogError("Not enough players.");
        }

        endGame = GetComponent<EndGame>();
    }

    /// <summary>
    /// Enabled any needed player AIs.
    /// </summary>
    protected void Start()
    {
        // Note: this is just an example loop. this will not handle multiple AIs yet.
        for (int i = 0; i < players.Count; i++)
        {
            PlayerAi ai = players[i].ship.GetComponent<PlayerAi>();

            if (ai != null)
            {
                ai.enabled = PlayerPrefs.HasKey(LoadLevel.VS_AI_KEY) && PlayerPrefs.GetInt(LoadLevel.VS_AI_KEY) == i;
            }
        }
    }

    /// <summary>
    /// Removes a player from the game by name.
    /// </summary>
    /// <param name="name">Name of the player to remove.</param>
    public void RemovePlayerByName(string name)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].name == name)
            {
                players.RemoveAt(i);
                break;
            }
        }

        TryEndGame();
    }   

    /// <summary>
    /// Removes a player from the game by their ship.
    /// </summary>
    /// <param name="ship">Ship controlled by the player to remove.</param>
    public void RemovePlayerByShip(Transform ship)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].ship == ship)
            {
                players.RemoveAt(i);
                break;
            }
        }

        TryEndGame();
    }

    /// <summary>
    /// Removes a player from the game by their planet.
    /// </summary>
    /// <param name="planet">Planet guarded by the player to remove.</param>
    public void RemovePlayerByPlanet(Transform planet)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].planet == planet)
            {
                players.RemoveAt(i);
                break;
            }
        }

        TryEndGame();
    }

    /// <summary>
    /// Checks to see if the game is over and ends it if nesessecary.
    /// </summary>
    private void TryEndGame()
    {
        if (players.Count == 1)
        {
            endGame.StartEndGame(players[0].name);
        }
    }
}

/// <summary>
/// Class is used to allow player data to be shown on the inspector in a single managable list.
/// </summary>
[System.Serializable]
public class PlayerData
{
    public string name;
    public Transform ship;
    public Transform planet;
}
