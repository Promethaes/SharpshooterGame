using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigation : MonoBehaviour
{
    public void ToGameScene()
    {
        SceneManager.LoadScene("Game Scene");
    }
    public void ToMenuScene()
    {
        SceneManager.LoadScene("Menu Scene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
