using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CardController : CardRenderer
{
    private Animator animator;

    [SerializeField]
    private int cardNum = 0;
    private int cardCost = 0;

    private GameController gameController;

    private DrawPile drawPile;

    // Start is called before the first frame update
    void Start()
    {
        Setup();

        gameController = FindObjectOfType<GameController>();
        drawPile = FindObjectOfType<DrawPile>();

        cardCost = 0;

        animator = gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (gameController.PlayedCard != null && gameController.PlayedCard.getCardNum() == cardNum)
        //{
        //    gameController.SelectedCard = null;
        //    gameController.PlayedCard = null;
        //    gameController.Hand.Remove(cardNum);
        //    Destroy(this.gameObject);
        //}

        if (gameController.Hand.Contains(cardNum))
        {
            int handIndex = gameController.Hand.IndexOf(cardNum) + 1;

            Vector3 cardPos = new Vector3(handIndex - 4, 0, (float)(-4 - (handIndex - 4)) / 100);
            transform.localPosition = cardPos;
        }

        ChangeSprite(cardNum);
    }

    public int getCost()
    {
        return cardCost;
    }

    public int getCardNum()
    {
        return cardNum;
    }

    public void setCardNum(int num)
    {
        cardNum = num;
    }

    private void ChangeSprite(int num)
    {
        spriteRenderer.sprite = spriteArray[num];

        if (num == 14)
        {
            cardCost = 5;
        }

    }

    public void MoveTo(Vector3 pos)
    {
        gameController.Hand.Remove(cardNum);

        while (transform.position != pos)
        {
            print("moving");
            transform.localPosition = Vector3.MoveTowards(transform.position, pos, 0.01f);
        }

        gameController.SelectedCard = null;
        gameController.PlayedCard = null;
        Destroy(this.gameObject);
    }

    private void OnMouseDown()
    {
        if (gameController.playerTurn && gameController.SelectedCard != this)
        {
            gameController.SelectedCard = this;
        }
        else if (gameController.SelectedCard == this)
        {
            gameController.SelectedCard = null;
        }
    }

    private void OnMouseEnter()
    {
        animator.SetBool("TouchingMouse", true);
    }

    private void OnMouseExit()
    {
        if (gameController.SelectedCard != this)
        {
            animator.SetBool("TouchingMouse", false);
        }
    }
}
