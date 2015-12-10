using UnityEngine;
using System.Collections;

/// <summary>
/// Basic movment logic. AI systems should plug into this by setting the 
/// appropriate directions and speed. If values are left alone the object will
/// drift at a the last set speed.
/// </summary>
public class Movement : MonoBehaviour
{
    [Header("Stats")]
    public bool faceMovementDirection;
    public float speed;
    public bool supportMoveAnimations = false;
	
    public Vector2 direction;
    private Transform myTransform; // Cached transform for quick look ups in the update loop.
    private Animator animator;

    /// <summary>
    /// Initialization logic.
    /// </summary>
    protected void Awake()
    {
        myTransform = transform;

        if (supportMoveAnimations)
        {
            animator = GetComponent<Animator>();
            supportMoveAnimations = animator != null;

            if (!supportMoveAnimations)
            {
                Debug.LogWarning("Cannot use move animations without an animator on object: " + name);
            }
        }

        Direction = Vector2.zero;
    }

	/// <summary>
	/// Move object as needed.
	/// </summary>
	void Update()
    {
        if (!Lock)
        {
            if (faceMovementDirection && direction.magnitude > 0)
            {
                myTransform.right = direction.normalized;
            }

            myTransform.position += speed * Time.deltaTime * (Vector3)direction;
        }
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

            if (!Lock && supportMoveAnimations)
            {
                if (direction.y == 0)
                {
                    animator.SetInteger("Vertical", 0);
                }
                else
                {
                    animator.SetInteger("Vertical", (int)Mathf.Sign(direction.y));
                }
            }
        }
    }

    /// <summary>
    /// Getter and setter used to prevent this object from moving physically.
    /// </summary>
    public bool Lock { get; set; }
}
