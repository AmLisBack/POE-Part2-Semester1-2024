using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Button[,] buttons = new Button[4,4];
    public bool p1Turn = false; //X
    public bool p2Turn = false; //O
    public TextMeshProUGUI currentPlayersTurn;//Text at top of screen to display whos turn it is

    // Start is called before the first frame update
    void Start()
    {
        
        int randomStart = Random.Range(0, 2);
        if(randomStart == 0 )
        {
            p1Turn = true;
        }
        else if (randomStart == 1 ) 
        {
            p2Turn = true;
        }
        for(int i = 0; i < 4; i++)
        {
            for(int j =0; j < 4; j++)
            {
                string buttonName = "Button" + i.ToString()+ j.ToString();
                buttons[i,j] = GameObject.Find(buttonName).GetComponent<Button>();
                int row = i;
                int col = j;
                buttons[i, j].onClick.AddListener(() => ButtonClicked(row, col));
                //Debug.Log("Button Name:"+buttons[row,col].name+$"| At Location {row}, {col}");
                
            }
        }
        DisplayPlayersTurn();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayPlayersTurn();
    }

    public void ButtonClicked(int row, int col)
    {
        Debug.Log("Button Clicked at row:" + row + ", " + col);
        if (p1Turn)
        {
            buttons[row, col].GetComponentInChildren<TextMeshProUGUI>().text = "X";
            p1Turn = false;
            p2Turn = true;
        }
        if(p2Turn)
        {
            buttons[row, col].GetComponentInChildren<TextMeshProUGUI>().text = "O";
            p2Turn = false;
            p1Turn=true;
        }
        buttons[row,col].interactable = false;  
        
    }
    public void DisplayPlayersTurn()
    {
        if(p1Turn)
        {
            currentPlayersTurn.text = "Player X Turn";
        }
        if(p2Turn)
        {
            currentPlayersTurn.text = "Player O Turn";
        }
    }
}
