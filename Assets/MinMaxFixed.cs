using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinMaxFixed : MonoBehaviour
{
    public TextMeshProUGUI currentPlayersTurn;
    public Button[,] buttons = new Button[4, 4];
    private Player[,] board = new Player[4, 4];
    private Player currentPlayer = Player.X;

    public enum Player { None, X, O }

    void Start()
    {
        InitializeBoard();
        InitialiseButtons();
    }

    void InitializeBoard()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                board[i, j] = Player.None;
            }
        }
    }

    void InitialiseButtons()
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

    void ButtonClicked(int row, int col)
    {
        DisplayPlayersTurn();
        if (MakeMove(row, col, Player.X))
        {
            buttons[row, col].interactable = false;
            buttons[row, col].GetComponentInChildren<TextMeshProUGUI>().text = "X";

            // Calculate and display utility scores for the computer's next move
            //DisplayUtilityScoresForNextMove();

            currentPlayer = Player.O;
            AIMove();
        }
        string winner = CheckWin();
        if (winner != "")
        {
            EndGame(winner);
            return;
        }
    }

    bool MakeMove(int row, int col, Player player)
    {
        if (board[row, col] == Player.None)
        {
            board[row, col] = player;
            return true;
        }
        return false;
    }

    void DisplayUtilityScoresForNextMove()
    {
        List<(int, int)> emptySquares = GetEmptySquares();
        foreach (var (r, c) in emptySquares)
        {
            float utilityScore = CalculateUtilityForCell(r, c, Player.O);
            buttons[r, c].GetComponentInChildren<TextMeshProUGUI>().text = utilityScore.ToString("F2");
        }
    }

    float CalculateUtilityForCell(int row, int col, Player player)
    {
        float rowUtility = CalculateUtilityForRow(row, player);
        float colUtility = CalculateUtilityForCol(col, player);
        float diag1Utility = 0, diag2Utility = 0;

        if (row == col)
            diag1Utility = CalculateUtilityForDiag1(player);
        if (row + col == 3)
            diag2Utility = CalculateUtilityForDiag2(player);

        return (rowUtility + colUtility + diag1Utility + diag2Utility) / 4.0f;
    }

    float CalculateUtilityForRow(int row, Player player)
    {
        int count = 0;
        for (int i = 0; i < 4; i++)
        {
            if (board[row, i] == player)
                count++;
            else if (board[row, i] != Player.None)
                return 0;
        }
        return (float)count / 4;
    }

    float CalculateUtilityForCol(int col, Player player)
    {
        int count = 0;
        for (int i = 0; i < 4; i++)
        {
            if (board[i, col] == player)
                count++;
            else if (board[i, col] != Player.None)
                return 0;
        }
        return (float)count / 4;
    }

    float CalculateUtilityForDiag1(Player player)
    {
        int count = 0;
        for (int i = 0; i < 4; i++)
        {
            if (board[i, i] == player)
                count++;
            else if (board[i, i] != Player.None)
                return 0;
        }
        return (float)count / 4;
    }

    float CalculateUtilityForDiag2(Player player)
    {
        int count = 0;
        for (int i = 0; i < 4; i++)
        {
            if (board[i, 3 - i] == player)
                count++;
            else if (board[i, 3 - i] != Player.None)
                return 0;
        }
        return (float)count / 4;
    }

    void AIMove()
    {
        List<(int, int)> emptySquares = GetEmptySquares();
        float maxUtility = -1;
        (int, int) bestMove = (-1, -1);

        foreach (var (r, c) in emptySquares)
        {
            float utilityScore = CalculateUtilityForCell(r, c, Player.O);
            if (utilityScore > maxUtility)
            {
                maxUtility = utilityScore;
                bestMove = (r, c);
            }
        }

        if (bestMove.Item1 != -1 && bestMove.Item2 != -1)
        {
            MakeMove(bestMove.Item1, bestMove.Item2, Player.O);
            buttons[bestMove.Item1, bestMove.Item2].interactable = false;
            buttons[bestMove.Item1, bestMove.Item2].GetComponentInChildren<TextMeshProUGUI>().text = "O";
        }
    }

    List<(int, int)> GetEmptySquares()
    {
        List<(int, int)> emptySquares = new List<(int, int)>();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (board[i, j] == Player.None)
                {
                    emptySquares.Add((i, j));
                }
            }
        }
        return emptySquares;
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
    private void EndGame(string winner)
    {

        if (winner == ".")
        {
            currentPlayersTurn.text = "Draw!";
        }
        else
        {
            currentPlayersTurn.text = winner + " wins!";
        }
    }

    public void DisplayPlayersTurn()
    {
        
    currentPlayersTurn.text = "Player";
        
        
    }
}
