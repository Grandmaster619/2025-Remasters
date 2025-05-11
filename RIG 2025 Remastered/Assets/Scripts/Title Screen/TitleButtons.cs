using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtons : MonoBehaviour
{
    GameObject MainSoundtrackAudioPrefab;
    private void Start()
    {
        DontDestroyOnLoad dontDestroyOnLoad = FindObjectOfType<DontDestroyOnLoad>();
        if (dontDestroyOnLoad != null)
        {
            MainSoundtrackAudioPrefab = FindObjectOfType<DontDestroyOnLoad>().gameObject;
        }
    }

    public void Play()
    {
        
        if (MainSoundtrackAudioPrefab != null ) { Destroy( MainSoundtrackAudioPrefab ); }
        SceneManager.LoadScene(1);
    }

    public void Settings()
    {
    }

    public void Credits()
    {
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void TestScenes(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
