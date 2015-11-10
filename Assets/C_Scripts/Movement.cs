using UnityEngine;
using System.Collections;

/// <summary>
/// Basic movment logic. AI systems should plug into this by setting the 
/// appropriate directions and speed. If values are left alone the object will
/// drift at a the last set speed.
/// </summary>
public class Movement : MonoBehaviour
{
    public bool faceMovementDirection;
    public float speed;

    private Vector2 direction;
    private Transform myTransform; // Cached transform for quick look ups in the update loop.

    /// <summary>
    /// Initialization logic.
    /// </summary>
    protected void Start()
    {
        myTransform = transform;
    }

	/// <summary>
	/// Move object as needed.
	/// </summary>
	void Update()
    {
        if (faceMovementDirection && direction.magnitude > 0)
        {
            myTransform.right = direction.normalized;
        }

        myTransform.position += speed * Time.deltaTime * (Vector3)direction;
	}

    /// <summary>
    /// Direction to move in. Magnitude must between 0 and 1 which allows for subtle movements on a joystick.
    /// </summary>
    public Vector2 Direction
    {
        get { return direction; }
        set
        {
            direction = value;

            if (direction.magnitude > 1)
            {
                direction.Normalize();
            }
        }
    }
}
