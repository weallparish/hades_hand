using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPile : MonoBehaviour
{
    [SerializeField]
    GameObject cardPrefab;

    private BoxCollider2D boxCollider;

    public GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        gameController = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.CardsDrawn.Count > gameController.Level * 13 + 1)
        {
            boxCollider.enabled = false;
        }
        else
        {
            boxCollider.enabled = true;
        }
    }

    private void OnMouseDown()
    {
        GameObject.Instantiate(cardPrefab);
    }
}
