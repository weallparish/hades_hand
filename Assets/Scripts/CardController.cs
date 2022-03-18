using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CardController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Sprite[] spriteArray;

    public GameController gameController;
    public DrawPile drawPile;

    int cardNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        drawPile = FindObjectOfType<DrawPile>();

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        AsyncOperationHandle<Sprite[]> spriteHandler = Addressables.LoadAssetAsync<Sprite[]>("Assets/Sprites/cardsLarge_tilemap.png");
        spriteHandler.Completed += LoadSprites;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void LoadSprites(AsyncOperationHandle<Sprite[]> handleToCheck)
    {
        if (handleToCheck.Status == AsyncOperationStatus.Succeeded)
        {
            spriteArray = handleToCheck.Result;
        }

        cardNum = drawPile.cardDrawn;
        ChangeSprite(cardNum);
    }

    void ChangeSprite(int num)
    {
        spriteRenderer.sprite = spriteArray[num];
    }
}
