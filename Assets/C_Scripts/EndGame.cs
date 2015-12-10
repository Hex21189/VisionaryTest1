using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Ends the game and transitions back to the main menu.
/// </summary>
public class EndGame : MonoBehaviour
{
    public Text gameOverText;
    public Spawn motherShipSpawner;

    /// <summary>
    /// Kicks off the end game coroutine and announces the winner.
    /// </summary>
    /// <param name="winnerName">Name of the winner.</param>
    public void StartEndGame(string winnerName)
    {
        StartCoroutine(EndGameLogic(winnerName));
    }

    /// <summary>
    /// Ends the game by displaying the winners name and reloading the main menu.
    /// </summary>
    /// <param name="winnerName">Name of the winner.</param>
    /// <returns>Time delay.</returns>
    private IEnumerator EndGameLogic(string winnerName)
    {
        RemoveEnemies();

        yield return new WaitForSeconds(2.0f);
        gameOverText.text = winnerName + " Wins!";
        gameOverText.enabled = true;

        // wait a few seconds
        yield return new WaitForSeconds(5.0f);

        // go to menu scene
        Application.LoadLevel(0);
    }

    /// <summary>
    /// Destroys all enemies on screen.
    /// </summary>
    private void RemoveEnemies()
    {
        motherShipSpawner.enabled = false;

        foreach (GameObject enemyObject in motherShipSpawner.enemyPool.GetAllObjectsInUse())
        {
            if (enemyObject)
            {
                DeathTrigger enemyKiller = enemyObject.GetComponent<DeathTrigger>();
                enemyKiller.OnDeath(null, false);
            }
        }

        DeathTrigger motherShipKiller = motherShipSpawner.GetComponent<DeathTrigger>();
        motherShipKiller.OnDeath(null, false);
    }
}
