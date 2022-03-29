using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int Level;
    public int SelectedCard;
    public int SelectedCost;
    public int PlayedCard;
    public List<int> Hand;
    public int maxPlayable;

    private int PlaysMax = 1;
    private int DrawsMax = 1;

    public int Plays = 1;
    public int Draws = 1;
    public int SacrificePoints = 0;

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

    // Start is called before the first frame update
    void Start()
    {
        Level = 1;

        SelectedCard = 99;
        PlayedCard = 99;

        maxPlayable = 14;

        diamondSprite = diamondCard.GetComponent<SpriteRenderer>();
        clubSprite = clubCard.GetComponent<SpriteRenderer>();
        spadeSprite = spadeCard.GetComponent<SpriteRenderer>();

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
    }
}
