using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int Level;
    public int PreviousSelectedCard;
    public int SelectedCard;
    public List<int> Hand;

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

    // Start is called before the first frame update
    void Start()
    {
        Level = 1;

        PreviousSelectedCard = 99;
        SelectedCard = 99;

        diamondSprite = diamondCard.GetComponent<SpriteRenderer>();
        clubSprite = clubCard.GetComponent<SpriteRenderer>();
        spadeSprite = spadeCard.GetComponent<SpriteRenderer>();
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
        }
        if (Level >= 4)
        {
            spadeSprite.sprite = spadeImg;
        }
    }
}
