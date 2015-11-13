using UnityEngine;
using System.Collections;

public class HandleInput : MonoBehaviour
{
    public Transform player;

    [Header("Input Axies/Buttons")]
    public string horizontalAxis;
    public string verticalAxis;
    public string shootButton;
    public string powerUpButton;

    private Movement movement;
    private Shooter shooter;
    private PlayerPowerUpManager powerUpManager;

    // TODO: delete
    public GameObject magnetPowerUpPrefab;

    protected void Awake()
    {
        movement = player.GetComponent<Movement>();
        shooter = player.GetComponent<Shooter>();
        powerUpManager = player.GetComponent<PlayerPowerUpManager>();

        if (movement == null)
        {
            Debug.LogWarning(string.Format("No movement script attached to player {0}. Cannot process movement logic", transform.name));
        }

        if (shooter == null)
        {
            Debug.LogWarning(string.Format("No shooter script attached to player {0}. Cannot process shooting logic", transform.name));
        }

        if (powerUpManager == null)
        {
            Debug.LogWarning(string.Format("No power up management script attached to player {0}. Cannot process powerup logic", transform.name));
        }
    }

	// Update is called once per frame
	protected void Update()
    {
        if (!LockInput)
        {
            if (movement != null)
            {
                Vector2 direction = new Vector2(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis));
                movement.Direction = direction;
            }

            if (shooter != null)
            {
                shooter.shoot = Input.GetButton(shootButton);
            }

            if (powerUpManager != null)
            {
                if (Input.GetButtonDown(powerUpButton))
                {
                    powerUpManager.StoredPowerUp = magnetPowerUpPrefab;
                    powerUpManager.ActivatePowerUp(player);
                }
            }
        }
        else
        {
            if (movement != null)
                movement.Direction = Vector2.zero;

            if (shooter != null)
                shooter.shoot = false;
        }
	}

    public bool LockInput { get; set; }
}
