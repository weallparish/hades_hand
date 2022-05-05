using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CardController : CardRenderer
{
    /// <summary>
    /// Animator of the game object
    /// </summary>
    private Animator animator;

    /// <summary>
    /// Value of the card
    /// </summary>
    [SerializeField]
    private int cardNum = 0;

    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// Cost of the card (default of 0)
    /// </summary>
    private int cardCost = 0;

    /// <summary>
    /// Position card should drift towards
    /// </summary>
    private Vector3 cardTarget;

    /// <summary>
    /// Game controller running entire game
    /// </summary>
    private GameController gameController;
    
    /// <summary>
    /// Dictates if a card is part of the hand or for temporary use
    /// </summary>
    [SerializeField]
    private bool temporary = false;

    /// <summary>
    /// Called before the first frame
    /// </summary>
    void Start()
    {
        //Run setup function from parent class (load sprites)
        Setup();

        //Find game controller
        gameController = FindObjectOfType<GameController>();

        //Set default card cost to 0
        cardCost = 0;

        //Find animator
        animator = GetComponentInChildren<Animator>();

        //Find renderer
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (temporary)
        {
            spriteRenderer.enabled = false;
        }
    }

    /// <summary>
    /// Called once per frame
    /// </summary>
    void Update()
    {
        //If the hand contains the current card
        if (gameController.Hand.Contains(cardNum))
        {
            //Set index in hand
            int handIndex = gameController.Hand.IndexOf(cardNum) + 1;

            //Move to nearest open slot in the hand
            cardTarget = new Vector3(handIndex - 4, 0, (float)(-4 - (handIndex - 4)) / 100);
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, cardTarget, 0.1f);
        }

        //If the card is temporary
        else if (temporary)
        {
            //Move towards the slot the card is assigned to
            transform.position = Vector3.MoveTowards(transform.position, cardTarget, 0.1f);
        }

        //If the hand does not contain the card and it isn't temporary
        else
        {
            spriteRenderer.enabled = true;

            //Move towards the final position
            transform.position = Vector3.MoveTowards(transform.position, cardTarget, 0.1f);

            //Once the card has reached it's destination
            if (Vector3.Distance(transform.position,cardTarget) < 0.15f)
            {
                //Destory card
                gameController.SelectedCard = null;
                Destroy(this.gameObject);
            }
        }

        //Constantly change sprite to card value
        ChangeSprite(cardNum);
    }

    /// <summary>
    /// Sets a card as temporary or non-temporary
    /// </summary>
    /// <param name="val">"true" or "false"</param>
    public void setTemporary(bool val)
    {
        temporary = val;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>cardCost</returns>
    public int getCost()
    {
        return cardCost;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>cardNum</returns>
    public int getCardNum()
    {
        return cardNum;
    }

    /// <summary>
    /// Sets value of card to num
    /// </summary>
    /// <param name="num">Value to set card to</param>
    public void setCardNum(int num)
    {
        cardNum = num;
    }

    /// <summary>
    /// Changes sprite to specified value
    /// </summary>
    /// <param name="num">Value to change sprite to</param>
    private void ChangeSprite(int num)
    {
        spriteRenderer.sprite = spriteArray[num];

        //If the card is an ace of diamonds, set its cost to 5
        if (num%14 == 0)
        {
            cardCost = 5;
        }

    }

    /// <summary>
    /// Sets a new move target and detaches card from the hand
    /// </summary>
    /// <param name="pos">Position to float towards</param>
    public void MoveTo(Vector3 pos)
    {
        //If the card is not temporary
        if (!temporary)
        {
            //Remove the card from the hand
            gameController.Hand.Remove(cardNum);
            cardTarget = new Vector3(pos.x, pos.y - 0.25f, pos.z);
        }
        else
        {
            cardTarget = pos;
        }


    }

    /// <summary>
    /// Activated when card is clicked
    /// </summary>
    private void OnMouseDown()
    {
        //If clicked when not selected, select the card
        if (gameController.playerTurn && gameController.SelectedCard != this)
        {
            gameController.SelectedCard = this;
        }

        //If clicked when selected, unselect the card
        else if (gameController.SelectedCard == this)
        {
            gameController.SelectedCard = null;
        }
    }

    /// <summary>
    /// Activated when card is hovered over
    /// </summary>
    private void OnMouseEnter()
    {
        if (!temporary)
        {
            animator.SetBool("TouchingMouse", true);
        }
    }

    /// <summary>
    /// Activated when card isn't hovered over
    /// </summary>
    private void OnMouseExit()
    {
        if (gameController.SelectedCard != this && !temporary)
        {
            animator.SetBool("TouchingMouse", false);
        }
    }
}
