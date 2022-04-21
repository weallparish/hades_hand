using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SlotController : CardRenderer
{
    /// <summary>
    /// Game controller running entire game
    /// </summary>
    private GameController gameController;

    /// <summary>
    /// Sprite to render the lack of a card
    /// </summary>
    [SerializeField]
    private Sprite emptyCard;

    /// <summary>
    /// Limit on amount of cards allowed in a slot
    /// </summary>
    [SerializeField]
    private int slotLimit = 1;

    /// <summary>
    /// Dictates whether cards can be removed from the slot and moved around
    /// </summary>
    [SerializeField]
    private bool isEditable = false;

    /// <summary>
    /// Prefab of card controller to spawn
    /// </summary>
    [SerializeField]
    private GameObject cardPrefab;

    /// <summary>
    /// List of cards contained in slot
    /// </summary>
    public List<int> cards;

    /// <summary>
    /// Turn that slot was updated
    /// </summary>
    private int turnPlayed;

    /// <summary>
    /// Shows whether the slot is selected
    /// </summary>
    private bool isSelected = false;

    /// <summary>
    /// Dictates if card can attack
    /// </summary>
    private bool summonSick = false;

    // Start is called before the first frame update
    void Start()
    {
        Setup();

        isSelected = false;
        summonSick = false;

        gameController = FindObjectOfType<GameController>();

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
        if (cards.Count > 0)
        {
            if (gameController.PlayedCard == null)
            {
                ChangeSprite(cards[cards.Count - 1]);
            }
        }
        else
        {
            ChangeSprite(-1);
        }

        if (gameController.PlayedCard != null && gameController.PlayedCard.getCardNum() == cards[cards.Count-1] && isSelected)
        {
            cards.RemoveAt(0);
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

                    gameController.SelectedCard.MoveTo(transform.position);
                    gameController.SelectedCard.setTemporary(false);

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

                    CardController[] handCards = FindObjectsOfType<CardController>();

                    foreach (CardController card in handCards)
                    {
                        if (card.getCardNum() == gameController.SelectedCard.getCardNum())
                        {
                            card.MoveTo(transform.position);
                            break;
                        }
                    }
                }
            }
         
            else
            {
                GameObject newCard = Instantiate(cardPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                CardController cardValue = newCard.GetComponent<CardController>();
                cardValue.setTemporary(true);
                cardValue.MoveTo(transform.position);

                yield return new WaitForSeconds(0.01f);

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
        if (cards.Count > 0 && !summonSick)
        {
            int blockCard = gameController.EnemyBlock(cards[cards.Count - 1]);
            summonSick = true;
            turnPlayed = gameController.turnNum;

            if (blockCard == -1)
            {
                gameController.EnemyHealth--;
            }
            else if (blockCard >= cards[0])
            {
                cards.Clear();
            }
        }
    }

}
