using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int Level;
    public int PreviousSelectedCard;
    public int SelectedCard;
    public List<int> Hand;

    // Start is called before the first frame update
    void Start()
    {
        Level = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
