using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int Level;
    public CardController SelectedCard;
    public CardController PlayedCard;
    public List<int> Hand;
    public List<int> EnemyField;
    public List<int> EnemyDeck;
    public EnemySlotController[] EnemySlots;
    public int maxPlayable;
    public bool playerTurn = false;
    public int turnNum = 0;

    public int PlayerHealth = 5;
    public int EnemyHealth = 5;

    private int PlaysMax = 1;
    private int DrawsMax = 1;

    public int Plays = 1;
    public int Draws = 1;
    public int SacrificePoints = 1;

    [SerializeField]
    private Sprite diamondImg;
    [SerializeField]
    private Sprite clubImg;
    [SerializeField]
    private Sprite spadeImg;

    [SerializeField]
    private GameObject diamondCard;
    [SerializeField]
    private GameObject clubCard;
    [SerializeField]
    private GameObject spadeCard;

    private SpriteRenderer diamondSprite;
    private SpriteRenderer clubSprite;
    private SpriteRenderer spadeSprite;

    [SerializeField]
    private DrawPile drawPile;
    [SerializeField]
    private TMPro.TextMeshProUGUI spCounter;
    [SerializeField]
    private TMPro.TextMeshProUGUI healthCounter;
    [SerializeField]
    private TMPro.TextMeshProUGUI enemyCounter;
    [SerializeField]
    private UnityEngine.UI.Button passButton;
    [SerializeField]
    private UnityEngine.UI.Button attackButton;

    // Start is called before the first frame update
    void Start()
    {
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

        passButton.onClick.AddListener(PassTurn);

        StartCoroutine(BeginRound());
    }

    // Update is called once per frame
    void Update()
    {
        if (Level >= 2)
        {
            diamondSprite.sprite = diamondImg;
        }
        if (Level >= 3)
        {
            clubSprite.sprite = clubImg;
            maxPlayable = 28;
        }
        if (Level >= 4)
        {
            spadeSprite.sprite = spadeImg;
        }

        spCounter.text = "SPs: " + SacrificePoints;
        healthCounter.text = "Health: " + PlayerHealth;
        enemyCounter.text = "Enemy: " + EnemyHealth;
    }

    private IEnumerator BeginRound()
    {
        yield return new WaitForSeconds(1);

        Draws = 0;
        Plays = PlaysMax;
        SacrificePoints = 0;

        PlayerHealth = 5;
        EnemyHealth = 5;

        EnemyDeck = new List<int> { 5, 1, 3 };

        for (int i=0; i<3; i++)
        {
            drawPile.DrawCard();
            yield return new WaitForSeconds(0.1f);
        }

        playerTurn = true;
    }

    private void EnemyTurn()
    {
        EnemyField.Clear();

        EnemySlotController[] enemySlots = FindObjectsOfType<EnemySlotController>();

        foreach (EnemySlotController slot in enemySlots)
        {
            EnemyField.Add(slot.GetCardNum());
        }

        if (EnemyField.Contains(-1))
        {
            EnemySlots[EnemyField.IndexOf(-1)].SetCardNum(EnemyDeck[0]);
            EnemyDeck.RemoveAt(0);
        }

        PlayerTurn();
    }

    private void PlayerTurn()
    {
        Draws = DrawsMax;
        Plays = PlaysMax;
        turnNum++;
        playerTurn = true;
    }

    private void PassTurn()
    {
        playerTurn = false;
        EnemyTurn();
    }

    public int EnemyBlock(int attackNum)
    {
        return -1;
    }
}
