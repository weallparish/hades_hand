using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int Level;

    public List<int> CardsDrawn;

    // Start is called before the first frame update
    void Start()
    {
        Level = 1;
        CardsDrawn = new List<int>();
        CardsDrawn.Add(13);
        CardsDrawn.Add(27);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
