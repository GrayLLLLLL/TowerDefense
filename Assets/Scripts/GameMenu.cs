using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{

    public void OnStartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void OnExitGame()
    {
        Application.Quit();
    }

}
