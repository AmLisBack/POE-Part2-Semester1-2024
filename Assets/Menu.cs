using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void SinglePlayerScene()
    {
        SceneManager.LoadScene("Single Player");
    }
    public void MultiplayerScene()
    {
        SceneManager.LoadScene("Multiplayer");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

}
