using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Mulitplayer : MonoBehaviour
{
    public Button[,] buttons = new Button[4, 4];
    public TextMeshProUGUI currentPlayersTurn;//Text at top of screen to display whos turn it is
    public string playerSymbol = "X";
    public string computerSymbol = "O";
    public string currentPlayer;
    private bool gameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        initialise();
        currentPlayer = playerSymbol;
        DisplayPlayersTurn();
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerInput(int row, int col)
    {
        if (gameOver)
            return;

        buttons[row, col].GetComponentInChildren<TextMeshProUGUI>().text = playerSymbol;
        buttons[row, col].interactable = false;
        string winner = CheckWin();
        if (winner != "")
        {
            EndGame(winner);
            return;
        }
        computerTurn();
        winner = CheckWin();
        if (winner != "")
        {
            EndGame(winner);
        }
    }
    public void computerTurn()
    {
        int optimalScore = int.MinValue;//set to min so that any score encountered will be smaller than the best score which helps with correct intialization
        int[] optimalMove = new int[2];//array to store the row and col of best move for ai
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (buttons[i, j].GetComponentInChildren<TextMeshProUGUI>().text == "")
                {
                    buttons[i, j].GetComponentInChildren<TextMeshProUGUI>().text = computerSymbol;
                    int currentScore = miniMax(buttons, 0, false);//incrementing depth to keep track of the moves and setting isMaxing to false because it is now the players turn
                    buttons[i, j].GetComponentInChildren<TextMeshProUGUI>().text = "";

                    if(currentScore > optimalScore)
                    {
                        optimalScore = currentScore;
                        optimalMove[0] = i;
                        optimalMove[1] = j;
                    }
                }
            }
            
        }
        buttons[optimalMove[0], optimalMove[1]].GetComponentInChildren<TextMeshProUGUI>().text = computerSymbol;
        buttons[optimalMove[0], optimalMove[1]].interactable = false;


    }
    public int miniMax(Button[,] buttons, int depth, bool isMaxing)
    {
        string result = CheckWin();

        if (result != "")
        {
            if (result == computerSymbol)
            {
                return 1;
            }
            else if (result == playerSymbol)
            {
                return -1;
            }
            else if (result == ".")
            {
                return 0;
            }
        }
        
           


        if (isMaxing) //when it is the computers (maximising player) turn it is trying to minimize the players move by setting optimal score to int.MinValue
        {
            int optimalScore = int.MinValue;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (buttons[i, j].GetComponentInChildren<TextMeshProUGUI>().text == "")
                    {
                        buttons[i, j].GetComponentInChildren<TextMeshProUGUI>().text = computerSymbol;
                        int currentScore = miniMax(buttons, depth + 1, false);//incrementing depth to keep track of the moves and setting isMaxing to false because it is now the players turn
                        buttons[i, j].GetComponentInChildren<TextMeshProUGUI>().text = "";
                        optimalScore = Math.Max(currentScore, optimalScore);
                    }
                }
            }
            return optimalScore;
        }
        else
        {
            int optimalScore = int.MaxValue;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (buttons[i, j].GetComponentInChildren<TextMeshProUGUI>().text == "")
                    {
                        buttons[i, j].GetComponentInChildren<TextMeshProUGUI>().text = playerSymbol;
                        int currentScore = miniMax(buttons, depth + 1, true);//incrementing depth to keep track of the moves and setting isMaxing to false because it is now the players turn
                        buttons[i, j].GetComponentInChildren<TextMeshProUGUI>().text = "";
                        optimalScore = Math.Min(currentScore, optimalScore);
                    }
                }
            }
            return optimalScore;
        }
    }
    public void DisplayPlayersTurn()
    {
        //displaying turns
        if (currentPlayer == playerSymbol)
        {
            currentPlayersTurn.text = "Player";
        }
        else
            currentPlayersTurn.text = "Computer";
    }
    public string CheckWin()
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
                    winner = buttons[row, 0].GetComponentInChildren<TextMeshProUGUI>().text;

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
            if (remainingSpaces() == 0)
            {
                winner = ".";
                return winner;
            }
            return " ";
        }

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
    public int remainingSpaces()
    {
        int remainingSpaces = 16;
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                if (buttons[row, col].GetComponentInChildren<TextMeshProUGUI>().text != "")
                {
                    remainingSpaces--;
                }
            }
        }
        return remainingSpaces;

    }
    private void initialise()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                string buttonName = "Button" + i.ToString() + j.ToString();
                buttons[i, j] = GameObject.Find(buttonName).GetComponent<Button>();
                int row = i;
                int col = j;
                buttons[i, j].onClick.AddListener(() => PlayerInput(row, col));
                buttons[i, j].GetComponentInChildren<TextMeshProUGUI>().text = "";
                buttons[i, j].interactable |= true;
                //Debug.Log("Button Name:"+buttons[row,col].name+$"| At Location {row}, {col}");

            }
        }
    }
    public void EndGame(string winner)
    {
        gameOver = true;
        if (winner == playerSymbol)
        {
            currentPlayersTurn.text = "Player Wins";
        }
        else if (winner == computerSymbol)
        {
            currentPlayersTurn.text = "Computer Wins";
        }
        else if(winner == ".")
        {
            currentPlayersTurn.text = "Draw";
        }
    }
}
