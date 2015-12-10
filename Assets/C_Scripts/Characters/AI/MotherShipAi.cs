using UnityEngine;

/// <summary>
/// Simple mother ship AI that moves the ship up and down in the playable area.
/// </summary>
[RequireComponent(typeof(DeathTrigger))]
[RequireComponent(typeof(Movement))]
public class MotherShipAi : MonoBehaviour
{
    public ScreenLayout screenLayout;

    private DeathTrigger deathTrigger;
    private Movement movement;
    private Transform myTransform;
    private bool isMovingUp;

    /// <summary>
    /// Load all components required to run this script.
    /// </summary>
    protected void Awake()
    {
        myTransform = transform;
        movement = GetComponent<Movement>();
        deathTrigger = GetComponent<DeathTrigger>();
    }

    /// <summary>
    /// Adjust the movement direction if we go too far in one direction.
    /// Note: movement only occurs in the Y plane.
    /// </summary>
    protected void Update()
    {
		if (!deathTrigger.Dieing)
        {
			if (myTransform.position.y > screenLayout.transform.position.y + screenLayout.mothershipMoveableArea.y / 2)
            {
				isMovingUp = false;
			}
            else if (myTransform.position.y < screenLayout.transform.position.y - screenLayout.mothershipMoveableArea.y / 2)
            {
				isMovingUp = true;
			}

            Vector2 direction = movement.Direction;
			direction.y = isMovingUp ? 1 - Mathf.Abs(direction.x) : -1 + Mathf.Abs(direction.x);
			movement.Direction = direction;
		}
    }
}
