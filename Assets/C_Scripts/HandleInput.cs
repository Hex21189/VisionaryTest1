using UnityEngine;
using System.Collections;

public class HandleInput : MonoBehaviour
{
    public Movement playerOneMovement;
    public Shooter  playerOneShooter;
    public PlayerPowerUpManager playerOnePowerUpManager;

    public Movement playerTwoMovement;

    public GameObject magnetPowerUpPrefab;

	// Update is called once per frame
	void Update()
    {
        if (playerOneMovement != null)
        {
            Vector2 playerOneDirection = Vector2.zero;

            if (Input.GetButton("Up"))
            {
                playerOneDirection.y = 1;
            }
            if (Input.GetButton("Down"))
            {
                playerOneDirection.y = -1;
            }
            if (Input.GetButton("Left"))
            {
                playerOneDirection.x = -1;
            }
            if (Input.GetButton("Right"))
            {
                playerOneDirection.x = 1;
            }

            playerOneShooter.shoot = Input.GetButton("fire");
            playerOneMovement.Direction = playerOneDirection;

            if (Input.GetKeyDown(KeyCode.A))
            {
                playerOnePowerUpManager.StoredPowerUp = magnetPowerUpPrefab;
                playerOnePowerUpManager.ActivatePowerUp(playerOneMovement.transform);
            }
        }
	}
}
