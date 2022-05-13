using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleSprite : CardRenderer, IDragHandler
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


        StartCoroutine(SetSpriteNum());
    }

    private IEnumerator SetSpriteNum()
    {
        yield return new WaitForSeconds(0.01f);

        ChangeSprite(Random.Range(0, 55));
    }

    private void ChangeSprite(int num)
    {
        //Set sprite to item "num" in array
        imageRenderer.overrideSprite = spriteArray[num];

    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        gameObject.transform.position = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
