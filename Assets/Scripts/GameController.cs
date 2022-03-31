using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int Level;
    public CardController SelectedCard;
    public CardController PlayedCard;
    public List<int> Hand;
    public int maxPlayable;
    public bool playerTurn = false;
    public int turnNum = 0;

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

        passButton.onClick.AddListener(PassTurn);
        attackButton.onClick.AddListener(Attack);

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
    }

    private IEnumerator BeginRound()
    {
        yield return new WaitForSeconds(1);

        Draws = 0;
        Plays = PlaysMax;
        SacrificePoints = 0;

        for (int i=0; i<3; i++)
        {
            drawPile.DrawCard();
            yield return new WaitForSeconds(0.1f);
        }

        playerTurn = true;
    }

    private void EnemyTurn()
    {
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

    private void Attack()
    {
        SlotController[] slots = FindObjectsOfType<SlotController>();

        foreach(SlotController s in slots)
        {
            s.Attack();
        }

        playerTurn = false;
        EnemyTurn();
    }
}
