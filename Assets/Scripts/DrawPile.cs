using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPile : MonoBehaviour
{
    private List<int> deckCards;

    private int cardDrawn;
    private int deckLevel;

    [SerializeField]
    private GameObject cardPrefab;
    [SerializeField]
    private SlotController discardPile;

    private BoxCollider2D boxCollider;
    private SpriteRenderer[] renderers;
    private GameController gameController;
    private GameObject deckWidth;
    private GameObject hand;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        renderers = GetComponentsInChildren<SpriteRenderer>();
        gameController = FindObjectOfType<GameController>();
        deckWidth = GameObject.Find("DeckSide");
        hand = GameObject.Find("Hand");

        deckLevel = 1;
        cardDrawn = 99;

        deckCards = new List<int>();

        for (int i = 1; i < 13; i++)
        {
            deckCards.Add(i);
        }
        deckCards.Add(14);
    }

    // Update is called once per frame
    void Update()
    {
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

    public int getCardDrawn()
    {
        return cardDrawn;
    }

    public void DrawCard()
    {
        cardDrawn = deckCards[Random.Range(0, deckCards.Count)];
        deckCards.Remove(cardDrawn);
        gameController.Hand.Add(cardDrawn);
        Instantiate(cardPrefab, new Vector3(gameController.Hand.Count - 4, (float)-4.5, (float)(-4 - (gameController.Hand.Count - 4)) / 100), Quaternion.identity, hand.transform);

        if (deckCards.Count < 1)
        {
            boxCollider.enabled = false;

            foreach (SpriteRenderer r in renderers)
            {
                r.enabled = false;
            }
        }
    }

    private void OnMouseDown()
    {
        if (gameController.Draws > 0 && gameController.Hand.Count < 7 && gameController.playerTurn)
        {
            DrawCard();
            gameController.Draws -= 1;
        }
    }
}
