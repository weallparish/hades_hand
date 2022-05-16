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

    private Animator animator;

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
    /// Game Object storing the discard pile
    /// </summary>
    [SerializeField]
    private SlotController discardPile;

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

    /// <summary>
    /// Attack button connected to card slot
    /// </summary>
    [SerializeField]
    private UnityEngine.UI.Button connectedButton = null;

   /// <summary>
   /// Called before the first frame
   /// </summary>
    void Start()
    {
        //Run setup function from parent class (loads sprites)
        Setup();

        //Set default values to false
        isSelected = false;
        summonSick = false;

        //Find game controller
        gameController = FindObjectOfType<GameController>();

        //Find animator
        animator = GetComponent<Animator>();

    }

    /// <summary>
    /// Changes sprite to specified number from array
    /// </summary>
    /// <param name="num">Value to change sprite to</param>
    private void ChangeSprite(int num)
    {
        //If value isn't -1, set sprite to value in array
        if (num >= 0)
        {
            spriteRenderer.sprite = spriteArray[num];
        }
        else
        {
            //If value is -1, set sprite to empty card
            spriteRenderer.sprite = emptyCard;
        }

    }

    /// <summary>
    /// Called once per frame
    /// </summary>
    void Update()
    {
        //If there are cards in slot, change sprite to value of top card
        if (cards.Count > 0)
        {
            if (gameController.PlayedCard == null)
            {
                ChangeSprite(cards[cards.Count - 1]);
            }
        }
        else
        {
            //If there aren't any cards in the slot, change sprite to empty
            ChangeSprite(-1);
            summonSick = false;
        }

        //If a card was played AND that card has the same value as the slot AND the slot is selected, remove that card and move it to the new slot
        if (gameController.PlayedCard != null && gameController.PlayedCard.getCardNum() == cards[cards.Count-1] && isSelected)
        {
            //Remove card from slot and unselect the card
            cards.RemoveAt(cards.Count-1);
            gameController.PlayedCard = null;

            isSelected = false;
        }

        //If the card has been summonSick for more than 1 turn, set summonSick to false
        if (gameController.turnNum > turnPlayed)
        {
            summonSick = false;
        }

        if (cards.Count > 0  && !summonSick && (gameController.playerTurn || gameController.canBlock))
        {
            if (connectedButton != null)
            {
                connectedButton.gameObject.SetActive(true);
            }
        }
        else
        {
            if (connectedButton != null)
            {
                connectedButton.gameObject.SetActive(false);
            }
        }

        if (summonSick)
        {
            if (!animator.GetBool("SummonSick"))
            {
                animator.SetBool("SummonSick", true);
            }
        }
        else
        {
            if (animator.GetBool("SummonSick"))
            {
                animator.SetBool("SummonSick", false);
            }
        }
    }

    /// <summary>
    /// Called when object is clicked on
    /// </summary>
    private void OnMouseDown()
    {
        if (gameController.playerTurn)
        {
            //Play card if it is the player's turn
            StartCoroutine(PlayCard());
        }
    }

    /// <summary>
    /// Checks what type of card is being played and if it is allowed to be played
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayCard()
    {
        //If the card slot isn't full
        if (cards.Count < slotLimit)
        {
            //If there is a card selected
            if (gameController.SelectedCard != null)
            {
                //If the user can afford/still play the card
                if ((gameController.Plays > 0 && gameController.SacrificePoints >= gameController.SelectedCard.getCost() && (gameController.SelectedCard.getCardNum() <= gameController.maxPlayable || gameController.SelectedCard.getCardNum() % 14 == 0)) || slotLimit > 1)
                {
                    //Set played card to currently selected card
                    gameController.PlayedCard = gameController.SelectedCard;

                    //Add selected card to list of cards in slot
                    cards.Add(gameController.SelectedCard.getCardNum());

                    //If the slot can only carry 1 card (typically a slot on the field)
                    if (slotLimit == 1)
                    {
                        //Remove sacrifice points for playing the card as well as a play
                        gameController.SacrificePoints -= gameController.SelectedCard.getCost();
                        gameController.Plays -= 1;

                        //Activate ability (such as ace or spell)
                        StartCoroutine(ActivateAbility());
                    }

                    //Move selected card to position of slot
                    gameController.SelectedCard.MoveTo(transform.position);

                    //If the card was temporary, remove that status (allows card to be killed when moved)
                    gameController.SelectedCard.setTemporary(false);

                }

                //If the slot isn't editable
                if (!isEditable)
                {
                    //Give player a sacrifice point
                    gameController.PlayedCard = gameController.SelectedCard;
                    gameController.SacrificePoints += 1;
                }
            }
        }
        //If the slot is editable BUT full
        else if (isEditable)
        {
            //If a card is selected
            if (gameController.SelectedCard != null)
            {
                //If card is an upgrade to current card
                if ((gameController.SelectedCard.getCardNum() == cards[cards.Count - 1] + 14))
                {
                    //Set current card to upgraded card
                    cards[cards.Count - 1] = gameController.SelectedCard.getCardNum();
                    gameController.PlayedCard = gameController.SelectedCard;

                    if (slotLimit == 1)
                    {
                        //Remove sacrifice points and plays
                        gameController.SacrificePoints -= gameController.SelectedCard.getCost();
                        gameController.Plays -= 1;

                        //Acivate card ability (such as ace or spell)
                        StartCoroutine(ActivateAbility());
                    }

                    //Find all cards in hand
                    CardController[] handCards = FindObjectsOfType<CardController>();

                    //Move selected card to position of slot
                    gameController.SelectedCard.MoveTo(transform.position);

                    //If the card was temporary, remove that status (allows card to be killed when moved)
                    gameController.SelectedCard.setTemporary(false);
                }
            }
         
            else
            {
                //Create new card to base selection off of
                GameObject newCard = Instantiate(cardPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                CardController cardValue = newCard.GetComponent<CardController>();
                
                //Make this card temporary
                cardValue.setTemporary(true);

                //Move to position of slot
                cardValue.MoveTo(transform.position);

                yield return new WaitForSeconds(0.01f);

                //Set card value to value of slot
                cardValue.setCardNum(cards[cards.Count - 1]);

                //Set selected card to newly created card
                gameController.SelectedCard = cardValue;

                //Mark that slot is selected
                isSelected = true;
            }
        }
    }

    /// <summary>
    /// Activates special abilities of cards (aces)
    /// </summary>
    /// <returns></returns>
    private IEnumerator ActivateAbility()
    {
        //If card is ace of diamonds, upgrade player to diamond deck
        if (cards[cards.Count - 1] == 14)
        {
            yield return new WaitForSeconds(0.1f);
            gameController.Level = 2;
            cards.Clear();
            ChangeSprite(-1);
        }

        //If card is ace of clubs, upgrade player to clubs deck
        else if (cards[cards.Count - 1] == 28)
        {
            yield return new WaitForSeconds(0.1f);
            gameController.Level = 3;
            cards.Clear();
            ChangeSprite(-1);
        }

        //If card is ace of spades, upgrade player to spades deck
        else if (cards[cards.Count - 1] == 42)
        {
            yield return new WaitForSeconds(0.1f);
            gameController.Level = 4;
            cards.Clear();
            ChangeSprite(-1);
        }

        else
        {
            //Start summon sickness
            summonSick = true;
            turnPlayed = gameController.turnNum;
        }
    }

    public void AddCards(List<int> list)
    {
        foreach (int i in list)
        {
            cards.Add(i);
        }
    }

    public int GetCardNum()
    {
        if (cards.Count > 0 && !summonSick)
        {
            return cards[cards.Count - 1];
        }
        else
        {
            return -1;
        }
    }

    /// <summary>
    /// Deals damage to enemy and gives enemy a chance to block
    /// </summary>
    public void Attack()
    {
        if (gameController.playerTurn)
        {
            print("Attack");
            //If the slot is able to attack
            if (cards.Count > 0 && !summonSick)
            {
                //Allow enemy to try to block attack
                int blockCard = gameController.EnemyBlock(cards[cards.Count - 1]);


                //If the enemy didn't block, deal damage to the enemy
                if (blockCard == -1)
                {
                    gameController.EnemyHealth--;

                    //Cause summon sickness
                    summonSick = true;
                    turnPlayed = gameController.turnNum;
                }

                //If the card the enemy blocked with had a higher value, destroy current card
                else if (blockCard >= cards[cards.Count -1])
                {
                    discardPile.AddCards(cards);
                    cards.Clear();
                }

                else
                {
                    //Cause summon sickness
                    summonSick = true;
                    turnPlayed = gameController.turnNum;
                }
            }

            if (gameController.EnemyHealth <= 0)
            {
                gameController.WinScreen();
            }
        }
        else if (gameController.canBlock)
        {
            print("Block");
            if (cards.Count > 0 && !summonSick)
            {

                //If the card the enemy blocked with had a higher value, destroy current card
                if (cards[cards.Count-1] <= gameController.EnemyAttackValue)
                {
                    discardPile.AddCards(cards);
                    cards.Clear();
                }

                gameController.canBlock = false;
            }
        }

        gameController.RecalibrateField();
    }

    public bool getIsEditable()
    {
        return isEditable;
    }

}
