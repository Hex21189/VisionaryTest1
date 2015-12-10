using UnityEngine;

/// <summary>
/// Adds points to the to the score of the player that hits this object.
/// </summary>
public class AddPointsOnHit : MonoBehaviour
{
    public int points;

    /// <summary>
    /// Adds points to the score of the player that hits this object. 
    /// Note: This script currently only works with bullet objects.
    /// </summary>
    /// <param name="collider">Bullet object.</param>
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Bullet")
        {
            Bullet bullet = collider.GetComponent<Bullet>();
            PlayerScore score = bullet.owner.GetComponent<PlayerScore>();

            if (score != null)
            {
                score.AddPoints(points);
            }
        }
    }
}