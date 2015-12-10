using UnityEngine;

/// <summary>
/// A class used to keep track of simple layout data.
/// </summary>
public class ScreenLayout : MonoBehaviour
{
	public Vector2 size;
	public Vector2 playerMoveableArea;
	public Vector2 mothershipMoveableArea;

	/// <summary>
	/// Draws the movable areas to the inspector screen for simple repositioning.
	/// </summary>
	protected void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireCube(transform.position, size);

		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(transform.position, playerMoveableArea);

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(transform.position, mothershipMoveableArea);
	}
}
