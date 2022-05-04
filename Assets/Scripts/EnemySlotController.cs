using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EnemySlotController : CardRenderer
{
    /// <summary>
    /// Game controller running entire game
    /// </summary>
    private GameController gameController;

    private Animator animator;

    /// <summary>
    /// Card value of slot
    /// </summary>
    private int cardNum;

    /// <summary>
    /// Sprite to display when slot has no cards
    /// </summary>
    [SerializeField]
    private Sprite emptyCard;

    /// <summary>
    /// Called before the first frame update
    /// </summary>
    void Start()
    {
        //Run setup function from parent class (loads sprites)
        Setup();

        //Find game controller
        gameController = FindObjectOfType<GameController>();

        //Set card value to -1 (empty sprite)
        cardNum = -1;

        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Called once per frame
    /// </summary>
    void Update()
    {
        //Change sprite to current cardNum
        ChangeSprite(cardNum);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>cardNum</returns>
    public int GetCardNum()
    {
        return cardNum;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>animator</returns>
    public Animator GetAnimator()
    {
        return animator;
    }

    /// <summary>
    /// Sets card number to specified value
    /// </summary>
    /// <param name="num">Value to set card to</param>
    public void SetCardNum(int num)
    {
        cardNum = num;
    }

    /// <summary>
    /// Changes sprite to specified value
    /// </summary>
    /// <param name="num">Value to set sprite to</param>
    private void ChangeSprite(int num)
    {
        //If value isn't -1, set sprite to value in sprite array
        if (num >= 0)
        {
            spriteRenderer.sprite = spriteArray[num];
        }
        else
        {
            //If value is -1, set sprite to empty card
            spriteRenderer.sprite = emptyCard;
        }

    }

}
