using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathButtons : MonoBehaviour
{
    private int currentScene;
    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    public void Restart()
    {
        SceneManager.LoadScene(currentScene);
    }
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void TestScenes(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
