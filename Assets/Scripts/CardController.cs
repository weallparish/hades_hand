using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CardController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Sprite[] spriteArray;

    [SerializeField]
    private int cardNum = 0;
    [SerializeField]
    private int cardCost = 0;

    private GameController gameController;
    private DrawPile drawPile;

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        drawPile = FindObjectOfType<DrawPile>();

        cardCost = 0;

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        AsyncOperationHandle<Sprite[]> spriteHandler = Addressables.LoadAssetAsync<Sprite[]>("Assets/Sprites/cardsLarge_tilemap.png");

        spriteHandler.Completed += LoadSprites;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.PlayedCard == cardNum)
        {
            gameController.SelectedCard = 99;
            gameController.PlayedCard = 99;
            gameController.Hand.Remove(cardNum);
            Destroy(this.gameObject);
        }

        if (gameController.Hand.Contains(cardNum))
        {
            int handIndex = gameController.Hand.IndexOf(cardNum) + 1;

            Vector3 cardPos = new Vector3(handIndex - 4, 0, (float)(-4 - (handIndex - 4)) / 100);
            transform.localPosition = cardPos;

        }
    }

    public int getCost()
    {
        return cardCost;
    }

    private void LoadSprites(AsyncOperationHandle<Sprite[]> handleToCheck)
    {
        if (handleToCheck.Status == AsyncOperationStatus.Succeeded)
        {
            spriteArray = handleToCheck.Result;
        }

        cardNum = drawPile.getCardDrawn();

        ChangeSprite(cardNum);
    }

    private void ChangeSprite(int num)
    {
        spriteRenderer.sprite = spriteArray[num];

        if (num == 14)
        {
            cardCost = 5;
        }

    }

    private void OnMouseDown()
    {
        gameController.SelectedCard = cardNum;
        gameController.SelectedCost = cardCost;
    }
}
