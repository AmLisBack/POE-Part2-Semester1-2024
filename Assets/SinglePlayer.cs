using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;
using System.Runtime.InteropServices.WindowsRuntime;

public class SinglePlayer : MonoBehaviour
{
    public Button[,] buttons = new Button[4, 4];
    public TextMeshProUGUI currentPlayersTurn;
    public Button powerUp1;
    public Button powerUp2;
    private Char playerSymbol = 'X';
    private Char computerSymbol = 'O';
    private Char currentPlayer;
    private bool gameOver = false;
    private bool playerMoved = false;
    public bool p1Turn = false; //X
    public bool p2Turn = false; //O
    private const int MaxRecursionDepth = 10;
    public int clickCount;
    private bool isReplaceClicked = false;
    private bool isDoubleClicked = false;
   

    void Start()
    {
        Initialise();
        currentPlayer = playerSymbol;
        DisplayPlayersTurn();
    }
    public void Update()
    {
        Debug.Log(currentPlayer.ToString());
    }

    public void PlayerInput(int row, int col)
    {
        if (gameOver || buttons[row, col].GetComponentInChildren<TextMeshProUGUI>().text != "")
            return;

        buttons[row, col].GetComponentInChildren<TextMeshProUGUI>().text = playerSymbol + "";
        buttons[row, col].interactable = false;
        playerMoved = true;

        string winner = CheckWin();
        if (winner != "")
        {
            EndGame(winner);
            return;
        }

        currentPlayer = computerSymbol;
        DisplayPlayersTurn();

        ComputerTurn();
    }

    public void ComputerTurn()
    {
        playerMoved = true;
        currentPlayer = computerSymbol;
        Debug.Log("Running");
        if (!playerMoved || gameOver)
            return;

        Debug.Log("Test");
        int optimalScore = int.MinValue;
        int[] optimalMove = new int[2];
        Debug.Log("Test3");
        char[,] board = GetBoardState(); // Change to char[,] type
        Debug.Log("Test4");
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (board[i, j] == '\0') // Check for empty cell
                {

                    board[i, j] = computerSymbol;
                    int currentScore = MiniMax(board, 0, false);
                    board[i, j] = '\0'; // Reset the cell to empty
                    if (currentScore > optimalScore)
                    {
                        optimalScore = currentScore;
                        optimalMove[0] = i;
                        optimalMove[1] = j;
                    }

                    // Your logic for finding the optimal move
                }
            }
        }
        buttons[optimalMove[0], optimalMove[1]].GetComponentInChildren<TextMeshProUGUI>().text = computerSymbol + "";
        buttons[optimalMove[0], optimalMove[1]].interactable = false;
        Debug.Log("Optimal move selected: " + optimalMove[0] + ", " + optimalMove[1]);
        string winner = CheckWin();
        if (winner != "")
        {
            EndGame(winner);
        }
        else
        {
            currentPlayer = playerSymbol;
            DisplayPlayersTurn();
        }

        playerMoved = false;
    }

    public int MiniMax(char[,] board, int depth, bool isMaxing)
    {
        board = GetBoardState();
        char outcome = Winner(board);
        if (outcome != '\0')
        {
            if (outcome == 'O')
            {
                return 1 - depth; // subtract depth from the score to determine which victory took the least amount of moves
            }
            else if (outcome == 'X')
            {
                return -1 + depth;
            }
            else
            {
                return 0;
            }
        }

        if (isMaxing) // when it is the computer's (maximising player) turn, it is trying to minimize the player's move by setting optimal score to int.MinValue
        {
            int optimalScore = int.MinValue;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (board[i, j] == ' ')
                    {
                        board[i, j] = computerSymbol;
                        int currentScore = MiniMax(board, depth + 1, false); // incrementing depth to keep track of the moves and setting isMaxing to false because it is now the player's turn
                        board[i, j] = ' ';
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
                    if (board[i, j] == ' ')
                    {
                        board[i, j] = playerSymbol;
                        int currentScore = MiniMax(board, depth + 1, true); // incrementing depth to keep track of the moves and set to true because it is now the computer's turn
                        board[i, j] = ' ';
                        optimalScore = Math.Min(currentScore, optimalScore);
                    }
                }
            }
            return optimalScore;
        }
    }

    public char Winner(char[,] board)
    {
        // Check rows
        for (int i = 0; i < 4; i++)
        {
            if (board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2] && board[i, 2] == board[i, 3] && board[i, 0] != '\0')
            {
                return board[i, 0];
            }
        }

        // Check columns
        for (int j = 0; j < 4; j++)
        {
            if (board[0, j] == board[1, j] && board[1, j] == board[2, j] && board[2, j] == board[3, j] && board[0, j] != '\0')
            {
                return board[0, j];
            }
        }

        // Check diagonals
        if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2] && board[2, 2] == board[3, 3] && board[0, 0] != '\0')
        {
            return board[0, 0];
        }
        if (board[0, 3] == board[1, 2] && board[1, 2] == board[2, 1] && board[2, 1] == board[3, 0] && board[0, 3] != '\0')
        {
            return board[0, 3];
        }

        // Check for tie
        bool isTie = true;
        foreach (char cell in board)
        {
            if (cell == '\0')
            {
                isTie = false;
                break;
            }
        }
        if (isTie)
        {
            return 'T';
        }

        return '\0'; // No winner yet
    }

    public void DisplayPlayersTurn()
    {
        if (currentPlayer == playerSymbol)
        {
            currentPlayersTurn.text = "Player";
        }
        else
        {
            currentPlayersTurn.text = "Computer";
        }
    }

    public string CheckWin()
    {
        string winner = "";

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

        if (buttons[0, 0].GetComponentInChildren<TextMeshProUGUI>().text == buttons[1, 1].GetComponentInChildren<TextMeshProUGUI>().text &&
            buttons[1, 1].GetComponentInChildren<TextMeshProUGUI>().text == buttons[2, 2].GetComponentInChildren<TextMeshProUGUI>().text &&
            buttons[2, 2].GetComponentInChildren<TextMeshProUGUI>().text == buttons[3, 3].GetComponentInChildren<TextMeshProUGUI>().text &&
            buttons[0, 0].GetComponentInChildren<TextMeshProUGUI>().text != "")
        {
            winner = buttons[0, 0].GetComponentInChildren<TextMeshProUGUI>().text;
            return winner;
        }

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

        return "";
    }
    
    public char CheckWin(char[,] board)
    {
        for (int row = 0; row < 4; row++)
        {
            if (board[row, 0] != '\0' &&
                board[row, 0] == board[row, 1] &&
                board[row, 1] == board[row, 2] &&
                board[row, 2] == board[row, 3])
            {
                return board[row, 0];
            }
        }

        for (int col = 0; col < 4; col++)
        {
            if (board[0, col] != '\0' &&
                board[0, col] == board[1, col] &&
                board[1, col] == board[2, col] &&
                board[2, col] == board[3, col])
            {
                return board[0, col];
            }
        }

        if (board[0, 0] != '\0' &&
            board[0, 0] == board[1, 1] &&
            board[1, 1] == board[2, 2] &&
            board[2, 2] == board[3, 3])
        {
            return board[0, 0];
        }

        if (board[0, 3] != '\0' &&
            board[0, 3] == board[1, 2] &&
            board[1, 2] == board[2, 1] &&
            board[2, 1] == board[3, 0])
        {
            return board[0, 3];
        }

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (board[i, j] == '\0')
                {
                    return '\0';
                }
            }
        }

        return '.';
    }

    public Char[,] GetBoardState()
    {
        char[,] board = new char[4, 4];

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                string buttonText = buttons[i, j].GetComponentInChildren<TextMeshProUGUI>().text;

                // If the buttonText contains only one character, convert it to char
                if (buttonText.Length == 1)
                {
                    board[i, j] = buttonText[0];
                }
                else
                {
                    // Handle the case when the buttonText is empty or has more than one character
                    board[i, j] = '\0'; // or any default value you prefer for empty strings
                }
            }
        }

        return board;
    }

    private void Initialise()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                string buttonName = "Button" + i.ToString() + j.ToString();
                buttons[i, j] = GameObject.Find(buttonName).GetComponent<Button>();
                int row = i;
                int col = j;
                buttons[i, j].onClick.AddListener(() => ButtonClicked(row, col));
                buttons[i, j].GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }

    public void ButtonClicked(int row, int col)
    {
        if (buttons[row, col].GetComponentInChildren<TextMeshProUGUI>().text == "")
        {
            if (currentPlayer == playerSymbol)
            {
                playerMoved = true;//true - When Min max works
                currentPlayer = computerSymbol;
            }
            
            buttons[row, col].GetComponentInChildren<TextMeshProUGUI>().text = playerSymbol + "";
            buttons[row, col].interactable = false;
            currentPlayer = currentPlayer == playerSymbol ? computerSymbol : playerSymbol;
            Debug.Log(currentPlayer);
            DisplayPlayersTurn();
            ComputerTurn();
            
        }
    }

    private void EndGame(string winner)
    {
        gameOver = true;
        if (winner == ".")
        {
            currentPlayersTurn.text = "Draw!";
        }
        else
        {
            currentPlayersTurn.text = winner + " wins!";
        }
    }

    private int remainingSpaces()
    {
        int count = 0;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (buttons[i, j].GetComponentInChildren<TextMeshProUGUI>().text == "")
                {
                    count++;
                }
            }
        }
        return count;
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
}

