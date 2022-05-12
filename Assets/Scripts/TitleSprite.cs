using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleSprite : CardRenderer
{
    [SerializeField]
    private Image imageRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //Run setup function from parent class (load sprites)
        Setup();

        //Find renderer
        imageRenderer = GetComponentInChildren<Image>();

        ChangeSprite(Random.Range(0, 55));
    }

    private void ChangeSprite(int num)
    {
        print("Change sprite");
        //Set sprite to item "num" in array
        imageRenderer.sprite = spriteArray[num];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
