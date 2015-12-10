using UnityEngine;

/// <summary>
/// Script for handling the basic planet animations (rotation and hp texture).
/// </summary>
public class Planet : MonoBehaviour
{
    public float rotationSpeed; // degrees per second
    public float cloudRotationSpeed;
    public Transform clouds;

	/// <summary>
    /// Update rotationsand textures.
    /// </summary>
	protected void Update()
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.z += Time.deltaTime * rotationSpeed;
        transform.rotation = Quaternion.Euler(rotation);

        rotation = clouds.rotation.eulerAngles;
        rotation.z += Time.deltaTime * cloudRotationSpeed;
        clouds.rotation = Quaternion.Euler(rotation);
    }
}
