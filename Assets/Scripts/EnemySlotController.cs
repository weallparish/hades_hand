using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EnemySlotController : CardRenderer
{
    private GameController gameController;
    private int cardNum;

    [SerializeField]
    private Sprite emptyCard;

    // Start is called before the first frame update
    void Start()
    {
        Setup();

        gameController = FindObjectOfType<GameController>();

        cardNum = -1;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeSprite(cardNum);
    }

    public int GetCardNum()
    {
        return cardNum;
    }

    public void SetCardNum(int num)
    {
        cardNum = num;
    }

    private void ChangeSprite(int num)
    {
        if (num >= 0)
        {
            spriteRenderer.sprite = spriteArray[num];
        }
        else
        {
            spriteRenderer.sprite = emptyCard;
        }

    }

}
