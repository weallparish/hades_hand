using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CardRenderer : MonoBehaviour
{
    /// <summary>
    /// The renderer of the sprite
    /// </summary>
    public SpriteRenderer spriteRenderer;
    /// <summary>
    /// Stores all card sprites 
    /// </summary>
    public Sprite[] spriteArray;

    /// <summary>
    /// Sets default values for sprite renderer and loads sprites
    /// </summary>
    public void Setup()
    {
        //Get sprite renderer of attached game object
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        
        //Find all addressables in "cardsLarge_tilemap"
        AsyncOperationHandle<Sprite[]> spriteHandler = Addressables.LoadAssetAsync<Sprite[]>("Assets/Sprites/cardsLarge_tilemap.png");

        //Once completed, load sprites
        spriteHandler.Completed += LoadSprites;
    }

    /// <summary>
    /// Loads the sprites into the sprite array
    /// </summary>
    /// <param name="handleToCheck"></param>
    private void LoadSprites(AsyncOperationHandle<Sprite[]> handleToCheck)
    {
        //Add loaded sprites to spriteArray
        if (handleToCheck.Status == AsyncOperationStatus.Succeeded)
        {
            spriteArray = handleToCheck.Result;
        }
    }

    /// <summary>
    /// Changes the sprite
    /// </summary>
    /// <param name="num">Value of sprite to change to</param>
    private void ChangeSprite(int num)
    {
        //Set sprite to item "num" in array
        spriteRenderer.sprite = spriteArray[num];
    }
}
