using UnityEngine;
using System.Collections;

public interface IPowerUp
{
    Sprite GetIconSprite();
    void Activate(Transform owner);
}
