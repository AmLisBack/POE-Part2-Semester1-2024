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
    public int clickCount;
    public Button powerUp1;
    public Button powerUp2;
    private bool isReplaceClicked = false;
    private bool isDoubleClicked = false;
   
    

    // Start is called before the first frame update
    void Start()
    {
        //buttons made inactive so that they are only visible when each player has made their first move
        powerUp1.gameObject.SetActive(false);
        powerUp2.gameObject.SetActive(false);

        //the 2 power up buttons' text are randomly changed to either Remove or Skip Turn
        int rdPowerUp1 = Random.Range(0, 2);
        if (rdPowerUp1 == 0)
        {
            powerUp1.GetComponentInChildren<TextMeshProUGUI>().text = "Player X: Replace";

        }
        else if (rdPowerUp1 == 1)
        {
            powerUp1.GetComponentInChildren<TextMeshProUGUI>().text = "Player X: Double move";

        }
        int rdPowerUp2 = Random.Range(0, 2);
        if (rdPowerUp2 == 0)
        {
            powerUp2.GetComponentInChildren<TextMeshProUGUI>().text = "Player O: Replace";

        }
        else if (rdPowerUp2 == 1)
        {
            powerUp2.GetComponentInChildren<TextMeshProUGUI>().text = "Player O: Double move";

        }

        //check if power ups are clicked
        if (powerUp1.GetComponentInChildren<TextMeshProUGUI>().text == "Player X: Replace")
        {
            powerUp1.onClick.AddListener(Replace);
            

        }
        else if (powerUp1.GetComponentInChildren<TextMeshProUGUI>().text == "Player X: Double move")
        {
            powerUp1.onClick.AddListener(Skip);

        }

        if (powerUp2.GetComponentInChildren<TextMeshProUGUI>().text == "Player O: Replace")
        {
            powerUp2.onClick.AddListener(Replace);

        }
        else if(powerUp2.GetComponentInChildren<TextMeshProUGUI>().text == "Player O: Double move")
        {
            powerUp2.onClick.AddListener(Skip);
        }

        //randomly choosing startinng player
        int randomStart = Random.Range(0, 2);
        if(randomStart == 0 )
        {
            p1Turn = true;
        }
        else if (randomStart == 1 ) 
        {
            p2Turn = true;
        }
        //initialising board
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
        //displaying turns and winners
        string winner;
        winner = ReadBoard();
        DisplayPlayersTurn();
        if (winner == "X")
        {
            currentPlayersTurn.text = "Player X won";
        }
        else if(winner == "O"){
            currentPlayersTurn.text = "Player O won";
        }


        
    }

    public void ButtonClicked(int row, int col)
    {
        clickCount++;//keeping track f how many times a button is clicked

        //when count = 2, buttons are made active
        if (clickCount == 2)
        {
           powerUp1.gameObject.SetActive(true);
           powerUp2.gameObject.SetActive(true);
        }
        
       

        //Debug.Log("Button Clicked at row:" + row + ", " + col);
        buttons[row, col].interactable = false;
        if (p1Turn)
        {
           //checks if power up is clicked and executes respective code
            if (isReplaceClicked)
            {
                buttons[row, col].GetComponentInChildren<TextMeshProUGUI>().text = "X";
                isReplaceClicked = false;
                p1Turn = false;
                p2Turn = true;
                interactable();
            }
            else
            {
                buttons[row, col].GetComponentInChildren<TextMeshProUGUI>().text = "X";
                p1Turn = false;
                p2Turn = true;
            }

            if (isDoubleClicked)
            {
                powerUp1.interactable = false;
                buttons[row, col].GetComponentInChildren<TextMeshProUGUI>().text = "X";
                isDoubleClicked = false;    
                p1Turn = true;
                p2Turn = false;
            }

            return;
        }
        if(p2Turn)
        {
            
            if (isReplaceClicked)
            {
                powerUp2.interactable = false;
                buttons[row, col].GetComponentInChildren<TextMeshProUGUI>().text = "O";
                isReplaceClicked = false;
                p2Turn = false;
                p1Turn = true;
                interactable();
            }
            else
            {
                buttons[row, col].GetComponentInChildren<TextMeshProUGUI>().text = "O";
                p2Turn = false;
                p1Turn = true;

            }

            if (isDoubleClicked)
            {
                powerUp2.interactable = false;
                buttons[row, col].GetComponentInChildren<TextMeshProUGUI>().text = "O";
                isDoubleClicked = false;
                p2Turn = true;
                p1Turn = false;
            }

        }

        /*
       if (p1Turn && isRemoveClicked)
        {
            p1Turn = false;
            Debug.Log("p1Turn: " + p1Turn);
            p2Turn = true;
            buttons[row, col].GetComponentInChildren<TextMeshProUGUI>().text = "X";
            isRemoveClicked = false;
            powerUp1.interactable = false;
            
        }
        */
        
    }
    public void DisplayPlayersTurn()
    {
        //displaying turns
        if(p1Turn)
        {
            currentPlayersTurn.text = "Player X Turn";
        }
        if(p2Turn)
        {
            currentPlayersTurn.text = "Player O Turn";
        }
    }
    private string ReadBoard()
    {
        {
            //checks for winner or draw and returns a string
            string winner;
            //check horizontal lines
            for (int row = 0; row < 4; row++)
            {
                if (buttons[row, 0].GetComponentInChildren<TextMeshProUGUI>().text == buttons[row, 1].GetComponentInChildren<TextMeshProUGUI>().text &&
                    buttons[row, 1].GetComponentInChildren<TextMeshProUGUI>().text == buttons[row, 2].GetComponentInChildren<TextMeshProUGUI>().text &&
                    buttons[row, 2].GetComponentInChildren<TextMeshProUGUI>().text == buttons[row, 3].GetComponentInChildren<TextMeshProUGUI>().text &&
                    buttons[row, 0].GetComponentInChildren<TextMeshProUGUI>().text != "")
                {
                    winner = buttons[row,0].GetComponentInChildren<TextMeshProUGUI>().text;

                    return winner;
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
                    winner = buttons[0, col].GetComponentInChildren<TextMeshProUGUI>().text;
                    return winner;
                }
            }

            //top-left to bottom-right
            if (buttons[0, 0].GetComponentInChildren<TextMeshProUGUI>().text == buttons[1, 1].GetComponentInChildren<TextMeshProUGUI>().text &&
                buttons[1, 1].GetComponentInChildren<TextMeshProUGUI>().text == buttons[2, 2].GetComponentInChildren<TextMeshProUGUI>().text &&
                buttons[2, 2].GetComponentInChildren<TextMeshProUGUI>().text == buttons[3, 3].GetComponentInChildren<TextMeshProUGUI>().text &&
                buttons[0, 0].GetComponentInChildren<TextMeshProUGUI>().text != "")
            {
                winner = buttons[0, 0].GetComponentInChildren<TextMeshProUGUI>().text;
                return winner;
            }

            //top-right to bottom-left
            if (buttons[0, 3].GetComponentInChildren<TextMeshProUGUI>().text == buttons[1, 2].GetComponentInChildren<TextMeshProUGUI>().text &&
                buttons[1, 2].GetComponentInChildren<TextMeshProUGUI>().text == buttons[2, 1].GetComponentInChildren<TextMeshProUGUI>().text &&
                buttons[2, 1].GetComponentInChildren<TextMeshProUGUI>().text == buttons[3, 0].GetComponentInChildren<TextMeshProUGUI>().text &&
                buttons[0, 3].GetComponentInChildren<TextMeshProUGUI>().text != "")
            {
                winner = buttons[0, 3].GetComponentInChildren<TextMeshProUGUI>().text;
                return winner;
            }

            return " ";
        }

    }

    private void Replace()
    {
        ////when button is clicked, it makes all buttons interactable
        if (p1Turn)
        {
            powerUp1.interactable = false;
        }
        else if (p2Turn)
        {
            powerUp2.interactable = false;
        }
        isReplaceClicked = true;
        Debug.Log("is replace clicked: " + isReplaceClicked);
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {

                buttons[i, j].interactable = true;
                //Debug.Log("Button Name:"+buttons[row,col].name+$"| At Location {row}, {col}");

            }
        }

        


    }
    private void Skip()
    {
        //when button is clicked, sets bool to true
        if (p1Turn)
        {
            powerUp1.interactable = false;
        }
        else if (p2Turn)
        {
            powerUp2.interactable = false;
        }
        isDoubleClicked = true;
    }

    private void interactable()
    {
        //when method is called, it loops through array and makes all buttons that contain symbols X or O inactive 
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (buttons[i, j].GetComponentInChildren<TextMeshProUGUI>().text == "X" || buttons[i, j].GetComponentInChildren<TextMeshProUGUI>().text == "O")
                {
                    buttons[i, j].interactable = false;
                }
                //Debug.Log("Button Name:"+buttons[row,col].name+$"| At Location {row}, {col}");

            }
        }
    }
}
