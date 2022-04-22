using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    /// <summary>
    /// Current level of the user's deck
    /// </summary>
    public int Level;

    /// <summary>
    /// Card object currently selected
    /// </summary>
    public CardController SelectedCard;

    /// <summary>
    /// Last card played
    /// </summary>
    public CardController PlayedCard;

    /// <summary>
    /// Current cards in user's hand
    /// </summary>
    public List<int> Hand;

    /// <summary>
    /// Current cards in enemy's field
    /// </summary>
    public List<int> EnemyField;

    /// <summary>
    /// Current cards in enemy's deck
    /// </summary>
    public List<int> EnemyDeck;

    /// <summary>
    /// Enemy slot controllers in enemy field
    /// </summary>
    public EnemySlotController[] EnemySlots;

    /// <summary>
    /// The highest value of card the player can currently play
    /// </summary>
    public int maxPlayable;

    /// <summary>
    /// Whether or not it is the player's turn
    /// </summary>
    public bool playerTurn = false;

    /// <summary>
    /// The turn number of the round
    /// </summary>
    public int turnNum = 0;

    /// <summary>
    /// Player health
    /// </summary>
    public int PlayerHealth = 5;

    /// <summary>
    /// Enemy Health
    /// </summary>
    public int EnemyHealth = 5;

    /// <summary>
    /// The max amount of cards that can be played per turn
    /// </summary>
    private int PlaysMax = 1;

    /// <summary>
    /// The max amount of cards that can be drawn per turn
    /// </summary>
    private int DrawsMax = 1;

    /// <summary>
    /// The amount of plays made in a turn
    /// </summary>
    public int Plays = 1;

    /// <summary>
    /// The amount of draws made in a turn
    /// </summary>
    public int Draws = 1;

    /// <summary>
    /// Current amount of sacrifice points
    /// </summary>
    public int SacrificePoints = 1;

    /// <summary>
    /// Sprite of an ace of diamonds
    /// </summary>
    [SerializeField]
    private Sprite diamondImg;

    /// <summary>
    /// Sprite of an ace of clubs
    /// </summary>
    [SerializeField]
    private Sprite clubImg;

    /// <summary>
    /// Sprite of an ace of spades
    /// </summary>
    [SerializeField]
    private Sprite spadeImg;

    /// <summary>
    /// Game object controlling diamond card
    /// </summary>
    [SerializeField]
    private GameObject diamondCard;

    /// <summary>
    /// Game object controlling club card
    /// </summary>
    [SerializeField]
    private GameObject clubCard;

    /// <summary>
    /// Game object controlling spade card
    /// </summary>
    [SerializeField]
    private GameObject spadeCard;

    /// <summary>
    /// Sprite renderer of diamond card
    /// </summary>
    private SpriteRenderer diamondSprite;

    /// <summary>
    /// Sprite renderer of club card
    /// </summary>
    private SpriteRenderer clubSprite;

    /// <summary>
    /// Sprite renderer of spade card
    /// </summary>
    private SpriteRenderer spadeSprite;

    /// <summary>
    /// Main draw pile
    /// </summary>
    [SerializeField]
    private DrawPile drawPile;

    /// <summary>
    /// Text displaying sps
    /// </summary>
    [SerializeField]
    private TMPro.TextMeshProUGUI spCounter;

    /// <summary>
    /// Text displaying health
    /// </summary>
    [SerializeField]
    private TMPro.TextMeshProUGUI healthCounter;

    /// <summary>
    /// Text displaying enemy health
    /// </summary>
    [SerializeField]
    private TMPro.TextMeshProUGUI enemyCounter;

    /// <summary>
    /// Pass button
    /// </summary>
    [SerializeField]
    private UnityEngine.UI.Button passButton;

    /// <summary>
    /// Called before first frame
    /// </summary>
    void Start()
    {
        //Set default values
        Level = 1;
        playerTurn = false;
        turnNum = 1;

        SelectedCard = null;
        PlayedCard = null;

        maxPlayable = 14;

        diamondSprite = diamondCard.GetComponent<SpriteRenderer>();
        clubSprite = clubCard.GetComponent<SpriteRenderer>();
        spadeSprite = spadeCard.GetComponent<SpriteRenderer>();

        EnemySlots = FindObjectsOfType<EnemySlotController>();

        //Add listener to pass button
        passButton.onClick.AddListener(PassTurn);

        //Begin round
        StartCoroutine(BeginRound());
    }

    /// <summary>
    /// Called once per frame
    /// </summary>
    void Update()
    {
        //If level >= 2, display diamond sprite
        if (Level >= 2)
        {
            diamondSprite.sprite = diamondImg;
        }
        
        //If level >= 3, display club sprite
        if (Level >= 3)
        {
            clubSprite.sprite = clubImg;
            maxPlayable = 28;
        }

        //If level >= 4, display spade sprite
        if (Level >= 4)
        {
            spadeSprite.sprite = spadeImg;
        }

        spCounter.text = "SPs: " + SacrificePoints;
        healthCounter.text = "Health: " + PlayerHealth;
        enemyCounter.text = "Enemy: " + EnemyHealth;
    }

    /// <summary>
    /// Called at the beginning of the round, sets default values
    /// </summary>
    /// <returns></returns>
    private IEnumerator BeginRound()
    {
        yield return new WaitForSeconds(1);

        //Set default round values
        Draws = 0;
        Plays = PlaysMax;
        SacrificePoints = 0;

        PlayerHealth = 5;
        EnemyHealth = 5;

        EnemyDeck = new List<int> {0,1,2,3,4,5,6,7,8,9,10,11,12};

        //Draw cards into player's hand
        for (int i=0; i<3; i++)
        {
            drawPile.DrawCard();
            yield return new WaitForSeconds(0.1f);
        }

        playerTurn = true;
    }

    /// <summary>
    /// Controls the enemy's turn. Draws and plays cards.
    /// </summary>
    private void EnemyTurn()
    {
        //Recalibrate values of enemy field
        EnemyField.Clear();

        EnemySlots = FindObjectsOfType<EnemySlotController>();

        foreach (EnemySlotController slot in EnemySlots)
        {
            EnemyField.Add(slot.GetCardNum());
        }

        //If an enemy field slot is empty, play a new card
        if (EnemyField.Contains(-1))
        {
            int cardChosen = Random.Range(0,EnemyDeck.Count);

            EnemySlots[EnemyField.LastIndexOf(-1)].SetCardNum(EnemyDeck[cardChosen]);
            EnemyDeck.RemoveAt(cardChosen);
        }

        PlayerTurn();
    }

    /// <summary>
    /// Controls the players turn, resets draws/plays and allows player to play
    /// </summary>
    private void PlayerTurn()
    {
        //Recalibrate cards in enemy field
        EnemyField.Clear();

        EnemySlots = FindObjectsOfType<EnemySlotController>();

        foreach (EnemySlotController slot in EnemySlots)
        {
            EnemyField.Add(slot.GetCardNum());
        }

        //Reset player values
        Draws = DrawsMax;
        Plays = PlaysMax;
        turnNum++;
        playerTurn = true;
    }

    /// <summary>
    /// Switches control to the enemy
    /// </summary>
    private void PassTurn()
    {
        playerTurn = false;
        EnemyTurn();
    }

    /// <summary>
    /// Finds all values in a list greater than a specified value
    /// </summary>
    /// <param name="list">List to find values from</param>
    /// <param name="val">Value to compare against</param>
    /// <returns>All values greater than val</returns>
    private List<int> FindGreater(List<int> list, int val)
    {
        List<int> returnList = new List<int>();

        foreach (int i in list)
        {
            if (i > val)
            {
                returnList.Add(i);
            }
        }

        return returnList;
    }

    /// <summary>
    /// Finds smallest item in a list
    /// </summary>
    /// <param name="list">List to search</param>
    /// <returns>Smallest value in list</returns>
    private int FindSmallest(List<int> list)
    {
        int smallestVal = 999;

        foreach (int i in list)
        {
            if (i < smallestVal && i > -1)
            {
                smallestVal = i;
            }
        }

        return smallestVal;
    }

    /// <summary>
    /// Controls the enemy block
    /// </summary>
    /// <param name="attackNum">Value that enemy is being attacked with</param>
    /// <returns>Card value the enemy blocked with</returns>
    public int EnemyBlock(int attackNum)
    {
        List<int> cardsGreater = FindGreater(EnemyField, attackNum);

        if (cardsGreater.Count == 0)
        {
            if (EnemyField.Count > 0)
            {
                //Block with the weakest card available if the enemy doesn't have high enough cards to block
                foreach (EnemySlotController slot in EnemySlots)
                {
                    if (slot.GetCardNum() == FindSmallest(EnemyField))
                    {
                        slot.SetCardNum(-1);
                        return FindSmallest(EnemyField);
                    }
                }
                return -1;
            }
            else
            {
                //If the enemy has no cards, don't block
                return -1;
            }
        }
        else
        {
            //If the enemy has a higher card to block with, block with the weakest of the high cards
            print(FindSmallest(cardsGreater));
            return FindSmallest(cardsGreater);
        }
    }
}
