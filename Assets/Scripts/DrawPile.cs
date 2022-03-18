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
    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        gameController = FindObjectOfType<GameController>();

        deckCards = new List<int>();

        for (int i = 0; i < 14; i++)
        {
            deckCards.Add(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (deckCards.Count < 1)
        {
            boxCollider.enabled = false;
        }
    }

    private void OnMouseDown()
    {
        cardDrawn = (Random.Range(0, deckCards.Count - 1));
        deckCards.Remove(cardDrawn);
        GameObject.Instantiate(cardPrefab);
    }
}
