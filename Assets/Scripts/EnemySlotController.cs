using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EnemySlotController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private GameController gameController;

    private Sprite[] spriteArray;
    private int cardNum;

    [SerializeField]
    private Sprite emptyCard;

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

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
        if (num >= 0)
        {
            spriteRenderer.sprite = spriteArray[num];
        }
        else
        {
            spriteRenderer.sprite = emptyCard;
        }

    }
}
