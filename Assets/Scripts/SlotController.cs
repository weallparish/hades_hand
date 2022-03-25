using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SlotController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private GameController gameController;

    [SerializeField]
    private Sprite emptyCard;
    [SerializeField]
    private int slotLimit = 1;
    [SerializeField]
    private bool isEditable = false;
    [SerializeField]
    private List<int> cards;

    private Sprite[] spriteArray;
    private bool isSelected = false;

    // Start is called before the first frame update
    void Start()
    {
        isSelected = false;

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        gameController = FindObjectOfType<GameController>();

        AsyncOperationHandle<Sprite[]> spriteHandler = Addressables.LoadAssetAsync<Sprite[]>("Assets/Sprites/cardsLarge_tilemap.png");

        spriteHandler.Completed += LoadSprites;
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

    // Update is called once per frame
    void Update()
    {
        if (gameController.PlayedCard == cards[cards.Count-1] && isSelected)
        {
            cards.RemoveAt(0);
            gameController.SelectedCard = 99;
            gameController.PlayedCard = 99;

            if (cards.Count > 0)
            {
                ChangeSprite(cards[0]);
            }
            else
            {
                spriteRenderer.sprite = emptyCard;
            }

            isSelected = false;
        }
    }

    private void OnMouseDown()
    {
        if (gameController.Plays > 0)
        {
            if (cards.Count < slotLimit)
            {
                ChangeSprite(gameController.SelectedCard);
                cards.Add(gameController.SelectedCard);
                gameController.PlayedCard = gameController.SelectedCard;

                if (!isEditable)
                {
                    gameController.SacrificePoints += 1;
                }
            }
            else if (isEditable)
            {
                gameController.SelectedCard = cards[cards.Count - 1];
                isSelected = true;
            }

            gameController.Plays -= 1;
        }

    }
}
