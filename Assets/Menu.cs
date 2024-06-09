using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void DifficultyScene()
    {
        SceneManager.LoadScene("Difficulty");
    }
    public void MultiplayerScene()
    {
        SceneManager.LoadScene("Multiplayer");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Easy()
    {
        SceneManager.LoadScene("Single Player");
    }
    public void Medium()
    {
        SceneManager.LoadScene("Medium");
    }
    public void Hard()
    {
        SceneManager.LoadScene("Hard");
    }
}
