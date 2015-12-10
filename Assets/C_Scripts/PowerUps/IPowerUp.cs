using UnityEngine;

/// <summary>
/// Interface for all powerups.
/// </summary>
public interface IPowerUp
{
    Sprite GetIconSprite();
    void Activate(Transform owner);
}
