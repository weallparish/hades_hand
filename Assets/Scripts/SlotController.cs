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
    private GameObject cardPrefab;

    public List<int> cards;

    private int turnPlayed;
    private Sprite[] spriteArray;
    private bool isSelected = false;
    private bool summonSick = false;

    // Start is called before the first frame update
    void Start()
    {
        isSelected = false;
        summonSick = false;

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
        if (num >= 0)
        {
            spriteRenderer.sprite = spriteArray[num];
        }
        else
        {
            spriteRenderer.sprite = emptyCard;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (cards.Count > 0 )
        {
            ChangeSprite(cards[cards.Count - 1]);
        }
        else
        {
            ChangeSprite(-1);
        }

        if (gameController.PlayedCard != null && gameController.PlayedCard.getCardNum() == cards[cards.Count-1] && isSelected)
        {
            cards.RemoveAt(0);
            gameController.SelectedCard = null;
            gameController.PlayedCard = null;

            if (cards.Count > 0)
            {
                ChangeSprite(cards[0]);
            }
            else
            {
                ChangeSprite(-1);
            }

            isSelected = false;
        }

        if (gameController.turnNum > turnPlayed)
        {
            summonSick = false;
        }
    }

    private void OnMouseDown()
    {
        if (gameController.playerTurn)
        {
            StartCoroutine(PlayCard());
        }

    }

    private IEnumerator PlayCard()
    {
        if (cards.Count < slotLimit)
        {
            if (gameController.SelectedCard != null)
            {
                if ((gameController.Plays > 0 && gameController.SacrificePoints >= gameController.SelectedCard.getCost() && (gameController.SelectedCard.getCardNum() <= gameController.maxPlayable || gameController.SelectedCard.getCardNum() % 14 == 0)) || slotLimit > 1)
                {
                    cards.Add(gameController.SelectedCard.getCardNum());
                    gameController.PlayedCard = gameController.SelectedCard;

                    if (slotLimit == 1)
                    {
                        gameController.SacrificePoints -= gameController.SelectedCard.getCost();
                        gameController.Plays -= 1;

                        summonSick = true;
                        turnPlayed = gameController.turnNum;
                        StartCoroutine(ActivateAbility());
                    }
                }

                if (!isEditable)
                {
                    gameController.SacrificePoints += 1;
                }
            }
        }
        else if (isEditable)
        {
            if (gameController.SelectedCard != null)
            {
                if ((gameController.SelectedCard.getCardNum() == cards[cards.Count - 1] + 14) && slotLimit == 1)
                {
                    cards[cards.Count - 1] = gameController.SelectedCard.getCardNum();
                    gameController.PlayedCard = gameController.SelectedCard;

                    if (slotLimit == 1)
                    {
                        gameController.SacrificePoints -= gameController.SelectedCard.getCost();
                        gameController.Plays -= 1;

                        summonSick = true;
                        turnPlayed = gameController.turnNum;
                        StartCoroutine(ActivateAbility());
                    }
                }
            }
         
            else
            {
                print("Move slot card test");

                GameObject newCard = Instantiate(cardPrefab, new Vector3(0, 0, 10), Quaternion.identity);
                CardController cardValue = newCard.GetComponent<CardController>();

                yield return new WaitForSeconds(0.1f);

                cardValue.setCardNum(cards[cards.Count - 1]);

                gameController.SelectedCard = cardValue;
                isSelected = true;
            }
        }
    }

    private IEnumerator ActivateAbility()
    {
        if (cards[cards.Count - 1] == 14)
        {
            yield return new WaitForSeconds(0.1f);
            gameController.Level = 2;
            cards.Clear();
            ChangeSprite(-1);
        }
        else if (cards[cards.Count - 1] == 28)
        {
            yield return new WaitForSeconds(0.1f);
            gameController.Level = 3;
            cards.Clear();
            ChangeSprite(-1);
        }
        else if (cards[cards.Count - 1] == 42)
        {
            yield return new WaitForSeconds(0.1f);
            gameController.Level = 4;
            cards.Clear();
            ChangeSprite(-1);
        }
    }

    public void Attack()
    {
        print("Attack");
    }

}
