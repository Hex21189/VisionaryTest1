using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Creates a string of repeated tiled textures to give the illusion of a growing image chain. 
/// Final sprite is scaled so the chain should fit exactly.
/// </summary>
public class RepeateTexture : SimplePool
{
    public Transform target;   

    private Transform myTransform;
    private List<Transform> repeatedSprites;
    private Bounds spriteBounds;

	/// <summary>
	/// Initializes boundary data to reduce calculations done in the update loop.
    /// Draw back of this is that the sprite bounds cannot change dynamically (which shouldn't be done anyway).
	/// </summary>
	protected void Start()
    {
        myTransform = transform;
        repeatedSprites = new List<Transform>();
        SpriteRenderer renderer = poolablePrefab.GetComponent<SpriteRenderer>();

        if (renderer)
        {
            spriteBounds = renderer.sprite.bounds;
        }
	}
	
	/// <summary>
	/// Redraw/reposition sprites to reach from transform.position to end.
	/// </summary>
	protected void Update()
    {
        Draw();
	}

    /// <summary>
    /// Repositions all sprites to span from transform.position to end. If there are not enough sprites 
    /// new ones are spawned. If there are too many, then extras are deleted. Last sprite is always scaled
    /// to reach exactly to the end.
    /// </summary>
    private void Draw()
    {
        // Get the current sprite with an unscaled size
        Vector2 spriteSize = new Vector2(spriteBounds.size.x / myTransform.localScale.x, spriteBounds.size.y / myTransform.localScale.y);
        Vector2 start = new Vector2(myTransform.position.x, myTransform.position.y);
        Vector2 end = new Vector2(target.position.x, target.position.y);
        Vector2 nextPosition = start;
        Vector2 rightVector = (end - start).normalized;

        int index = 0;
        float dist = Vector2.Distance(end, nextPosition);
		float sizeX = Mathf.Abs (spriteSize.x);

        // Handle unscaled repeated textures
		while (Vector3.Distance(end, nextPosition) > sizeX)
        {
            // Position sprite
            Transform sprite = GetSprite(index++);
            sprite.position = nextPosition;
            sprite.localScale = myTransform.localScale;
            sprite.right = rightVector;

            // Get next sprite position
            dist = Vector2.Distance(end, nextPosition);
            float yDist = end.y - nextPosition.y;
            float xDist = end.x - nextPosition.x;
			nextPosition += new Vector2(xDist * sizeX / dist, yDist * sizeX / dist);
        }

        // Position and scale final sprite
        Transform finalSprite = GetSprite(index++);
        finalSprite.position = nextPosition;
        finalSprite.right = rightVector;
        finalSprite.localScale = new Vector3(Vector3.Distance(nextPosition, end) / spriteSize.x, 1, 1); 

        // Release extra sprites
        while (index < repeatedSprites.Count)
        {
            ReleaseObject(repeatedSprites[index].gameObject);
            repeatedSprites.RemoveAt(index);
        }
    }

    /// <summary>
    /// Get an existing sprite at the given index or create a new one if the index is out of bounds. 
    /// If a new sprite is created the index is not guaranteed point to the sprite.
    /// </summary>
    /// <param name="index">Index of existing sprite. Invalid index will create a new sprite</param>
    /// <returns>Transform of sprite fetched.</returns>
    private Transform GetSprite(int index)
    {
        Transform sprite;

        if (index >= 0 && index < repeatedSprites.Count)
        {
            sprite = repeatedSprites[index];
        }
        else
        {
            sprite = GetAvailableObject().transform;
            sprite.parent = transform;
            repeatedSprites.Add(sprite);
        }

        return sprite;
    }
}
