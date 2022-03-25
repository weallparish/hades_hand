using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPile : MonoBehaviour
{
    public List<int> deckCards;
    public int cardDrawn;

    [SerializeField]
    private GameObject cardPrefab;

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

    }

    public void DrawCard()
    {
        cardDrawn = deckCards[Random.Range(0, deckCards.Count)];
        deckCards.Remove(cardDrawn);
        gameController.Hand.Add(cardDrawn);
        Instantiate(cardPrefab, new Vector3(gameController.Hand.Count - 4, (float)-4.5, (float)(-4 - (gameController.Hand.Count - 4)) / 100), Quaternion.identity, hand.transform);

        print(cardDrawn);

        if (deckCards.Count < 1)
        {
            boxCollider.enabled = false;

            foreach (SpriteRenderer r in renderers)
            {
                r.enabled = false;
            }
        }

        float xPos = (float)(-0.56 + ((13 - deckCards.Count + 2) / gameController.Level * 0.01));
        deckWidth.transform.localPosition = new Vector3(xPos, deckWidth.transform.localPosition.y, deckWidth.transform.localPosition.z);
    }

    private void OnMouseDown()
    {
        if (gameController.Draws > 0)
        {
            DrawCard();
            gameController.Draws -= 1;
        }
    }
}
