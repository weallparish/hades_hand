using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CardController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Sprite[] spriteArray;
    private Animator animator;
    private int cardNum = 0;

    [SerializeField]
    private Sprite emptyCard;

    public GameController gameController;
    public DrawPile drawPile;

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        drawPile = FindObjectOfType<DrawPile>();
        animator = GetComponent<Animator>();

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        AsyncOperationHandle<Sprite[]> spriteHandler = Addressables.LoadAssetAsync<Sprite[]>("Assets/Sprites/cardsLarge_tilemap.png");

        spriteHandler.Completed += LoadSprites;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.PreviousSelectedCard == cardNum && gameController.SelectedCard == 99)
        {
            gameController.Hand.Remove(cardNum);
            Destroy(this.gameObject);
        }

        if (gameController.Hand.Contains(cardNum))
        {
            int handIndex = gameController.Hand.IndexOf(cardNum);
            print("MOVE POS: " + handIndex);

            transform.position = new Vector3(handIndex - 4, (float)-4.5, (float)(-4 - (handIndex - 4)) / 100);

        }
    }

    private void LoadSprites(AsyncOperationHandle<Sprite[]> handleToCheck)
    {
        if (handleToCheck.Status == AsyncOperationStatus.Succeeded)
        {
            spriteArray = handleToCheck.Result;
        }

        cardNum = drawPile.cardDrawn;
        ChangeSprite(cardNum);
    }

    private void ChangeSprite(int num)
    {
        if (drawPile.cardDrawn <= 55)
        {
            spriteRenderer.sprite = spriteArray[num];
        }
        else
        {
            spriteRenderer.sprite = emptyCard;
        }
    }

    private void OnMouseDown()
    {
        gameController.PreviousSelectedCard = gameController.SelectedCard;
        gameController.SelectedCard = cardNum;

        if (gameController.SelectedCard == 99)
        {
            ChangeSprite(gameController.PreviousSelectedCard);
        }
    }
}
