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

    [SerializeField]
    private GameObject HandContainer;

    /// <summary>
    /// Current cards in player's field
    /// </summary>
    public List<int> PlayerField;

    /// <summary>
    /// Value of most recent enemy attack
    /// </summary>
    public int EnemyAttackValue;

    /// <summary>
    /// Current cards in enemy's field
    /// </summary>
    public List<int> EnemyField;

    /// <summary>
    /// Current cards in enemy's deck
    /// </summary>
    private List<int> EnemyDeck;

    /// <summary>
    /// Current cards in enemy's discard pile
    /// </summary>
    public List<int> EnemyDiscard;

    private int titleCardAmount;

    /// <summary>
    /// Enemy slot controllers in enemy field
    /// </summary>
    private EnemySlotController[] EnemySlots;

    /// <summary>
    /// Currently attacking card
    /// </summary>
    private EnemySlotController AttackingCard;

    /// <summary>
    /// Level of the enemy's deck
    /// </summary>
    private int EnemyDeckLevel;

    /// <summary>
    /// The highest value of card the player can currently play
    /// </summary>
    public int maxPlayable;

    /// <summary>
    /// Whether or not it is the player's turn
    /// </summary>
    public bool playerTurn = false;

    /// <summary>
    /// Whether or not the player can block
    /// </summary>
    public bool canBlock = false;

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

    [SerializeField]
    private Sprite cardBackImg;

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
    /// Attack Button Text for slot 1
    /// </summary>
    [SerializeField]
    private UnityEngine.UI.Button attackButton1;

    /// <summary>
    /// Attack Button Text for slot 2
    /// </summary>
    [SerializeField]
    private UnityEngine.UI.Button attackButton2;

    /// <summary>
    /// Attack Button Text vfor slot 3
    /// </summary>
    [SerializeField]
    private UnityEngine.UI.Button attackButton3;

    [SerializeField]
    private GameObject titleScreen;

    [SerializeField]
    private GameObject playScreen;

    [SerializeField]
    private GameObject gameOverScreen;

    [SerializeField]
    private GameObject winScreen;

    [SerializeField]
    private GameObject settingsScreen;

    [SerializeField]
    private GameObject titleScreenCardPrefab;

    //Settings Variables
    [SerializeField]
    private UnityEngine.UI.Slider healthSlider;

    [SerializeField]
    private TMPro.TextMeshProUGUI healthSettingDisplay;

    [SerializeField]
    private UnityEngine.UI.Slider handSlider;

    [SerializeField]
    private TMPro.TextMeshProUGUI handSettingDisplay;

    /// <summary>
    /// Called before first frame
    /// </summary>
    void Start()
    {
        //Set default values
        Level = 1;
        playerTurn = false;
        turnNum = 1;
        titleCardAmount = 0;

        SelectedCard = null;
        PlayedCard = null;

        maxPlayable = 14;

        diamondSprite = diamondCard.GetComponent<SpriteRenderer>();
        clubSprite = clubCard.GetComponent<SpriteRenderer>();
        spadeSprite = spadeCard.GetComponent<SpriteRenderer>();

        drawPile = FindObjectOfType<DrawPile>();

        EnemySlots = FindObjectsOfType<EnemySlotController>();

        EnemyDeckLevel = 1;

        //Add listener to pass button
        passButton.onClick.AddListener(PassTurn);

        //Drop cards
        StartCoroutine(DropCards());
    }

    public void StartGame()
    {
        titleScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        winScreen.SetActive(false);
        playScreen.SetActive(true);
        StartCoroutine(BeginRound());
    }

    public void GameOverScreen()
    {
        playScreen.SetActive(false);
        titleScreen.SetActive(false);
        winScreen.SetActive(false);
        gameOverScreen.SetActive(true);
    }

    public void TitleScreen()
    {
        gameOverScreen.SetActive(false);
        playScreen.SetActive(false);
        winScreen.SetActive(false);
        settingsScreen.SetActive(false);
        titleScreen.SetActive(true);

        StartCoroutine(DropCards());
    }

    public void WinScreen()
    {
        gameOverScreen.SetActive(false);
        playScreen.SetActive(false);
        titleScreen.SetActive(false);
        winScreen.SetActive(true);
    }

    public void SettingsScreen()
    {
        titleScreen.SetActive(false);
        settingsScreen.SetActive(true);
    }

    private IEnumerator DropCards()
    {
        while(titleScreen.activeInHierarchy && titleCardAmount <= 250)
        {
            float xpos = Random.Range(0, Screen.width);
            float ypos = Random.Range(0, Screen.height);
            GameObject titleCard = Instantiate(titleScreenCardPrefab, new Vector3(xpos, ypos, titleScreen.transform.position.z), Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
            titleCard.transform.parent = titleScreen.transform;

            titleCardAmount++;

            print("Card Count: " + titleCardAmount);

            yield return new WaitForSeconds(Random.Range(0.1f, 5.0f));
        }
    }

    /// <summary>
    /// Called once per frame
    /// </summary>
    void Update()
    {
        //If level >= 2, display diamond sprite
        if (Level >= 2)
        {
            if (EnemyDeckLevel < 2)
            {
                diamondSprite.sprite = diamondImg;
                EnemyDeckLevel = 2;
            }
        }
        else
        {
            diamondSprite.sprite = cardBackImg;
        }

        //If level >= 3, display club sprite
        if (Level >= 3)
        {
            if (EnemyDeckLevel < 3)
            {
                clubSprite.sprite = clubImg;
                maxPlayable = 28;
                EnemyDeckLevel = 3;
            }
        }
        else
        {
            clubSprite.sprite = cardBackImg;
        }

        //If level >= 4, display spade sprite
        if (Level >= 4)
        {
            if (EnemyDeckLevel < 4)
            {
                spadeSprite.sprite = spadeImg;
                EnemyDeckLevel = 4;
            }
        }
        else
        {
            spadeSprite.sprite = cardBackImg;
        }

        if (playerTurn)
        {
            attackButton1.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText("Attack");
            attackButton2.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText("Attack");
            attackButton3.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText("Attack");

            passButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText("End Turn");
            passButton.gameObject.SetActive(true);

            UnityEngine.UI.ColorBlock colors = attackButton1.colors;
            colors.normalColor = new Color32(210, 80, 80, 255);
            colors.highlightedColor = new Color32(112, 40, 40, 255);
            colors.selectedColor = Color.white;

            attackButton1.colors = colors;
            attackButton2.colors = colors;
            attackButton3.colors = colors;

            colors = passButton.colors;
            colors.normalColor = new Color32(176, 98, 191, 255);
            colors.highlightedColor = new Color32(126, 69, 138, 255);
            colors.selectedColor = new Color32(176, 98, 191, 255);

            passButton.colors = colors;
        }
        else
        {
            attackButton1.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText("Block");
            attackButton2.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText("Block");
            attackButton3.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText("Block");

            passButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText("Skip Block");

            if (!canBlock)
            {
                passButton.gameObject.SetActive(false);
            }
            else
            {
                passButton.gameObject.SetActive(true);
            }

            UnityEngine.UI.ColorBlock colors = attackButton1.colors;
            colors.normalColor = new Color32(100, 83, 212, 255);
            colors.highlightedColor = new Color32(62, 52, 128, 255);
            colors.selectedColor = Color.white;

            attackButton1.colors = colors;
            attackButton2.colors = colors;
            attackButton3.colors = colors;

            colors = passButton.colors;
            colors.normalColor = new Color32(207, 133, 68, 255);
            colors.highlightedColor = new Color32(168, 107, 54, 255);
            colors.selectedColor = new Color32(100, 83, 212, 255);

            passButton.colors = colors;
        }

        spCounter.text = "SPs: " + SacrificePoints;
        healthCounter.text = "Health: " + PlayerHealth;
        enemyCounter.text = "Enemy: " + EnemyHealth;
        healthSettingDisplay.text = "(" + healthSlider.value + ")";
        handSettingDisplay.text = "(" + handSlider.value + ")";
    }

    /// <summary>
    /// Called at the beginning of the round, sets default values
    /// </summary>
    /// <returns></returns>
    private IEnumerator BeginRound()
    {
        //Set default round values
        Draws = 0;
        Plays = PlaysMax;
        SacrificePoints = 0;
        Level = 1;

        Hand.Clear();

        drawPile.ResetCards();

        PlayerHealth = (int) healthSlider.value;
        EnemyHealth = (int) healthSlider.value;

        EnemyDeck = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14 };

        foreach (EnemySlotController slot in EnemySlots)
        {
            slot.SetCardNum(-1);
        }

        CardController[] previousCards = HandContainer.GetComponentsInChildren<CardController>();

        foreach (CardController card in previousCards)
        {
            Destroy(card.gameObject);
        }

        SlotController[] previousField = FindObjectsOfType<SlotController>();

        foreach (SlotController slot in previousField)
        {
            slot.cards.Clear();
        }

        //Draw cards into player's hand
        for (int i = 0; i < handSlider.value; i++)
        {
            drawPile.DrawCard();
            yield return new WaitForSeconds(0.1f);
        }

        playerTurn = true;
    }

    /// <summary>
    /// Controls the enemy's turn. Draws and plays cards.
    /// </summary>
    private IEnumerator EnemyTurn()
    {
        //Recalibrate values of enemy field
        RecalibrateField();

        int cardChosen = Random.Range(0, EnemyDeck.Count - 1);
        int smallestCard = FindSmallest(EnemyField);

        EnemySlotController updatedSlot = null;

        //If an enemy field slot is empty, play a new card
        if (EnemyField.Contains(-1) && EnemyDeck.Count > 0)
        {
            updatedSlot = EnemySlots[EnemyField.LastIndexOf(-1)];
            updatedSlot.SetCardNum(EnemyDeck[cardChosen]);
            EnemyDeck.RemoveAt(cardChosen);

            StartCoroutine(ActivateAbility(updatedSlot));
        }
        else if (cardChosen > smallestCard)
        {
            EnemyDiscard.Add(smallestCard);

            updatedSlot = EnemySlots[EnemyField.LastIndexOf(smallestCard)];
            updatedSlot.SetCardNum(EnemyDeck[cardChosen]);
            EnemyDeck.RemoveAt(cardChosen);

            StartCoroutine(ActivateAbility(updatedSlot));
        }

        List<int> cardsGreater = FindGreater(EnemyField, FindGreatest(PlayerField));

        if (cardsGreater.Count != 0)
        {
            foreach (int i in cardsGreater)
            {
                EnemyAttackValue = i;

                if (AttackingCard != null)
                {
                    AttackingCard.GetAnimator().SetBool("Damage", false);
                    AttackingCard = null;
                }

                foreach (EnemySlotController slot in EnemySlots)
                {
                    if (slot.GetCardNum() == EnemyAttackValue)
                    {
                        AttackingCard = slot;
                        break;
                    }
                }

                canBlock = true;

                AttackingCard.GetAnimator().SetBool("Waiting", true);
                

                if (FindGreatest(PlayerField) <= 0)
                {
                    canBlock = false;
                    PlayerHealth--;
                    AttackingCard.GetAnimator().SetBool("Damage", true);
                }

                yield return new WaitUntil(() => canBlock == false);

                AttackingCard.GetAnimator().SetBool("Waiting", false);

                yield return new WaitForSeconds(0.4f);

                if (PlayerHealth <= 0)
                {
                    GameOverScreen();
                }
            }
        }

        PlayerTurn();
    }

    /// <summary>
    /// Controls the players turn, resets draws/plays and allows player to play
    /// </summary>
    private void PlayerTurn()
    {
        //Recalibrate cards in enemy field
        RecalibrateField();

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
        if (playerTurn)
        {
            playerTurn = false;
            StartCoroutine(EnemyTurn());
        }
        else if (canBlock)
        {
            print("Attack animation");
            AttackingCard.GetAnimator().SetBool("Damage", true);
            canBlock = false;
            PlayerHealth--;
        }
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

    private int FindGreatest(List<int> list)
    {
        int largestVal = 0;

        foreach (int i in list)
        {
            if (i > largestVal && i > -1)
            {
                largestVal = i;
            }
        }

        return largestVal;
    }

    /// <summary>
    /// Reload which cards are in the enemy field
    /// </summary>
    public void RecalibrateField()
    {
        EnemyField.Clear();

        EnemySlotController[] EnemySlots = FindObjectsOfType<EnemySlotController>();

        foreach (EnemySlotController slot in EnemySlots)
        {
            EnemyField.Add(slot.GetCardNum());
        }

        PlayerField.Clear();

        SlotController[] PlayerSlots = FindObjectsOfType<SlotController>();

        foreach (SlotController slot in PlayerSlots)
        {
            if (slot.getIsEditable())
            {
                PlayerField.Add(slot.GetCardNum());
            }
        }
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
                        EnemyDiscard.Add(slot.GetCardNum());
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

    private IEnumerator ActivateAbility(EnemySlotController slot)
    {
        //If card is ace of diamonds, upgrade player to diamond deck
        if (slot.GetCardNum() == 14)
        {
            yield return new WaitForSeconds(0.5f);
            EnemyDeck.AddRange(EnemyDiscard);
            EnemyDiscard.Clear();
            EnemyDeck.AddRange(new List<int>() {15,16,17,18,19,20,21,22,23,24,25,26,28 });
            slot.SetCardNum(-1);
        }

        //If card is ace of clubs, upgrade player to clubs deck
        else if (slot.GetCardNum() == 28)
        {
            yield return new WaitForSeconds(0.5f);
            EnemyDeck.AddRange(EnemyDiscard);
            EnemyDiscard.Clear();
            EnemyDeck.AddRange(new List<int>() {29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 42});
            slot.SetCardNum(-1);
        }

        //If card is ace of spades, upgrade player to spades deck
        else if (slot.GetCardNum() == 42)
        {
            yield return new WaitForSeconds(0.5f);
            EnemyDeck.AddRange(EnemyDiscard);
            EnemyDiscard.Clear();
            EnemyDeck.AddRange(new List<int>() { 43, 44, 45, 46, 47, 48, 49 ,50 ,51 ,52 ,53 ,54 });
            slot.SetCardNum(-1);
        }

        RecalibrateField();
    }

}