using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CardRenderer : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] spriteArray;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

        AsyncOperationHandle<Sprite[]> spriteHandler = Addressables.LoadAssetAsync<Sprite[]>("Assets/Sprites/cardsLarge_tilemap.png");

        spriteHandler.Completed += LoadSprites;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadSprites(AsyncOperationHandle<Sprite[]> handleToCheck)
    {
        if (handleToCheck.Status == AsyncOperationStatus.Succeeded)
        {
            spriteArray = handleToCheck.Result;
        }
    }

    private void ChangeSprite(int num)
    {
        spriteRenderer.sprite = spriteArray[num];
    }
}
