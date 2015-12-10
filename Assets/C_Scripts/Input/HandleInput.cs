using UnityEngine;

/// <summary>
/// Script for processing user input on one specific player object.
/// </summary>
public class HandleInput : MonoBehaviour
{
    [Header("Input Axies/Buttons")]
    public string horizontalAxis;
    public string verticalAxis;
    public string shootButton;
    public string powerUpButton;

	public ScreenLayout screenLayout;

    private Movement movement;
    private Shooter shooter;
    private PlayerPowerUpManager powerUpManager;
    private PlayerAi ai;

    /// <summary>
    /// Loads all the components required for this script.
    /// </summary>
    protected void Awake()
    {
        movement = transform.GetComponent<Movement>();
        shooter = transform.GetComponent<Shooter>();
        powerUpManager = transform.GetComponent<PlayerPowerUpManager>();
        ai = transform.GetComponent<PlayerAi>();

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

	/// <summary>
    /// Processes all pending player input.
    /// </summary>
	protected void Update()
    {
        if (ai != null && ai.enabled)
        {
            return;
        }

        if (!LockInput)
        {
            if (movement != null)
            {
				float horz = Input.GetAxis(horizontalAxis);
				float vert = Input.GetAxis(verticalAxis);

				if ((horz < 0 && transform.position.x < screenLayout.transform.position.x - screenLayout.playerMoveableArea.x / 2) ||
				    (horz > 0 && transform.position.x > screenLayout.transform.position.x + screenLayout.playerMoveableArea.x / 2))
				{
					horz = 0;
				}

				if ((vert < 0 && transform.position.y < screenLayout.transform.position.y - screenLayout.playerMoveableArea.y / 2) ||
				    (vert > 0 && transform.position.y > screenLayout.transform.position.y + screenLayout.playerMoveableArea.y / 2))
				{
					vert = 0;
				}

				movement.Direction = new Vector2(horz, vert);
            }

            if (shooter != null)
            {
                shooter.shoot = Input.GetButton(shootButton);
            }

            if (powerUpManager != null)
            {
                if (Input.GetButtonDown(powerUpButton))
                {
                    powerUpManager.ActivatePowerUp(transform);
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

    /// <summary>
    /// Locks out user input.
    /// </summary>
    public bool LockInput { get; set; }
}
