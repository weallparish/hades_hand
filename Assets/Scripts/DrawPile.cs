using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPile : MonoBehaviour
{
    /// <summary>
    /// Cards currently in deck
    /// </summary>
    private List<int> deckCards;

    /// <summary>
    /// Most recent card drawn
    /// </summary>
    private int cardDrawn;

    /// <summary>
    /// Suite level of deck
    /// </summary>
    private int deckLevel;

    /// <summary>
    /// Prefab of card to spawn
    /// </summary>
    [SerializeField]
    private GameObject cardPrefab;

    /// <summary>
    /// Discard pile to reshuffle from
    /// </summary>
    [SerializeField]
    private SlotController discardPile;

    /// <summary>
    /// Collider of game object
    /// </summary>
    private BoxCollider2D boxCollider;

    /// <summary>
    /// Renderers of child game objects
    /// </summary>
    private SpriteRenderer[] renderers;

    /// <summary>
    /// Game controller running the entire game
    /// </summary>
    private GameController gameController;

    /// <summary>
    /// Width of deck (how thick it is)
    /// </summary>
    private GameObject deckWidth;

    /// <summary>
    /// Hand to spawn cards inside of
    /// </summary>
    private GameObject hand;

    /// <summary>
    /// Called before the first frame update
    /// </summary>
    void Start()
    {
        //Set default values
        boxCollider = GetComponent<BoxCollider2D>();
        renderers = GetComponentsInChildren<SpriteRenderer>();
        gameController = FindObjectOfType<GameController>();
        deckWidth = GameObject.Find("DeckSide");
        hand = GameObject.Find("Hand");

        deckLevel = 1;
        cardDrawn = 0;

        //Reset cards in deck
        deckCards = new List<int>();

        for (int i = 1; i < 13; i++)
        {
            deckCards.Add(i);
        }
        deckCards.Add(14);
    }

    /// <summary>
    /// Called once per frame
    /// </summary>
    void Update()
    {
        //
        float xPos = (float)(-0.56 + ((13 - deckCards.Count + 2) / gameController.Level * 0.01));
        deckWidth.transform.localPosition = new Vector3(xPos, deckWidth.transform.localPosition.y, deckWidth.transform.localPosition.z);

        if (boxCollider.enabled == false && deckCards.Count > 0)
        {
            boxCollider.enabled = true;

            foreach (SpriteRenderer r in renderers)
            {
                r.enabled = true;
            }
        }

        if (gameController.Level == 2 && deckLevel < 2)
        {
            for (int i = 15; i < 27; i++)
            {
                deckCards.Add(i);
            }
            deckCards.Add(28);

            foreach(int c in discardPile.cards)
            {
                deckCards.Add(c);
            }
            discardPile.cards.Clear();

            deckLevel = 2;
        }

        if (gameController.Level == 3 && deckLevel < 3)
        {
            for (int i = 29; i < 41; i++)
            {
                deckCards.Add(i);
            }
            deckCards.Add(42);

            deckLevel = 3;
        }

        if (gameController.Level == 4 && deckLevel < 4)
        {
            for (int i = 43; i < 55; i++)
            {
                deckCards.Add(i);
            }

            deckLevel = 4;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>cardDrawn</returns>
    public int getCardDrawn()
    {
        return cardDrawn;
    }

    /// <summary>
    /// Draws a card from the deck
    /// </summary>
    public void DrawCard()
    {
        cardDrawn = deckCards[Random.Range(0, deckCards.Count)];
        deckCards.Remove(cardDrawn);
        gameController.Hand.Add(cardDrawn);
        GameObject card = Instantiate(cardPrefab, new Vector3(gameController.Hand.Count - 4, (float)-4.5, (float)(-4 - (gameController.Hand.Count - 4)) / 100), Quaternion.identity, hand.transform);
        card.GetComponent<CardController>().setCardNum(cardDrawn);

        if (deckCards.Count < 1)
        {
            boxCollider.enabled = false;

            foreach (SpriteRenderer r in renderers)
            {
                r.enabled = false;
            }
        }
    }

    /// <summary>
    /// Activated when sprite is clicked on
    /// </summary>
    private void OnMouseDown()
    {
        if (gameController.Draws > 0 && gameController.Hand.Count < 7 && gameController.playerTurn)
        {
            DrawCard();
            gameController.Draws -= 1;
        }
    }
}
