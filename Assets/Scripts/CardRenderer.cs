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
    /// Called before the first frame
    /// </summary>
    void Start()
    {

    }

    /// <summary>
    /// Sets default values for sprite renderer and loads sprites
    /// </summary>
    public void Setup()
    {
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

        AsyncOperationHandle<Sprite[]> spriteHandler = Addressables.LoadAssetAsync<Sprite[]>("Assets/Sprites/cardsLarge_tilemap.png");

        spriteHandler.Completed += LoadSprites;
    }

    /// <summary>
    /// Called once per frame
    /// </summary>
    void Update()
    {
        
    }

    /// <summary>
    /// Loads the sprites into the sprite array
    /// </summary>
    /// <param name="handleToCheck"></param>
    private void LoadSprites(AsyncOperationHandle<Sprite[]> handleToCheck)
    {
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
        spriteRenderer.sprite = spriteArray[num];
    }
}
