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
        buttons[row, col].interactable = false;
        if (p1Turn)
        {
            buttons[row, col].GetComponentInChildren<TextMeshProUGUI>().text = "X";
            p1Turn = false;
            p2Turn = true;
            return;
        }
        if(p2Turn)
        {
            buttons[row, col].GetComponentInChildren<TextMeshProUGUI>().text = "O";
            p2Turn = false;
            p1Turn=true;
        }
         
        
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
    private bool ReadBoard()
    {
        {
            //check horizontal lines
            for (int row = 0; row < 4; row++)
            {
                if (buttons[row, 0].GetComponentInChildren<TextMeshProUGUI>().text == buttons[row, 1].GetComponentInChildren<TextMeshProUGUI>().text &&
                    buttons[row, 1].GetComponentInChildren<TextMeshProUGUI>().text == buttons[row, 2].GetComponentInChildren<TextMeshProUGUI>().text &&
                    buttons[row, 2].GetComponentInChildren<TextMeshProUGUI>().text == buttons[row, 3].GetComponentInChildren<TextMeshProUGUI>().text &&
                    buttons[row, 0].GetComponentInChildren<TextMeshProUGUI>().text != "")
                {
                    return true;
                }
            }

            //check vertical lines
            for (int col = 0; col < 4; col++)
            {
                if (buttons[0, col].GetComponentInChildren<TextMeshProUGUI>().text == buttons[1, col].GetComponentInChildren<TextMeshProUGUI>().text &&
                    buttons[1, col].GetComponentInChildren<TextMeshProUGUI>().text == buttons[2, col].GetComponentInChildren<TextMeshProUGUI>().text &&
                    buttons[2, col].GetComponentInChildren<TextMeshProUGUI>().text == buttons[3, col].GetComponentInChildren<TextMeshProUGUI>().text &&
                    buttons[0, col].GetComponentInChildren<TextMeshProUGUI>().text != "")
                {
                    return true;
                }
            }

            //top-left to bottom-right
            if (buttons[0, 0].GetComponentInChildren<TextMeshProUGUI>().text == buttons[1, 1].GetComponentInChildren<TextMeshProUGUI>().text &&
                buttons[1, 1].GetComponentInChildren<TextMeshProUGUI>().text == buttons[2, 2].GetComponentInChildren<TextMeshProUGUI>().text &&
                buttons[2, 2].GetComponentInChildren<TextMeshProUGUI>().text == buttons[3, 3].GetComponentInChildren<TextMeshProUGUI>().text &&
                buttons[0, 0].GetComponentInChildren<TextMeshProUGUI>().text != "")
            {
                return true;
            }

            //top-right to bottom-left
            if (buttons[0, 3].GetComponentInChildren<TextMeshProUGUI>().text == buttons[1, 2].GetComponentInChildren<TextMeshProUGUI>().text &&
                buttons[1, 2].GetComponentInChildren<TextMeshProUGUI>().text == buttons[2, 1].GetComponentInChildren<TextMeshProUGUI>().text &&
                buttons[2, 1].GetComponentInChildren<TextMeshProUGUI>().text == buttons[3, 0].GetComponentInChildren<TextMeshProUGUI>().text &&
                buttons[0, 3].GetComponentInChildren<TextMeshProUGUI>().text != "")
            {
                return true;
            }

            return false;
        }

    }
}
