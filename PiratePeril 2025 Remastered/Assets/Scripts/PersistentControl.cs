﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public enum Collectibles
{
    CANDESCENT_CUTLASS,
    GROG,
    JUMPING_JEWEL,
    MIKE_THE_MUSKET
}


public class PersistentControl : MonoBehaviour
{
    private int sfxVolume;
    public int SfxVolume { get { return sfxVolume; } set { sfxVolume = value; } }

    private int musicVolume;
    public int MusicVolume { get { return musicVolume; } set { musicVolume = value; } }

    private List<Collectibles> collected;
    public List<Collectibles> Collected { get => collected; set => collected = value; }

    private bool PAUSED;
    private int playerScore;
    // private GameObject loadScreen;
    // private Slider loadScreenProgress;
    private bool routineRunning;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        collected = new List<Collectibles>();
        routineRunning = false;
        LoadGame();
    }

    /// <summary>
    /// Called on the first frame of Instantiation (however this is only called the first time it is 
    /// instantiated.
    /// </summary>
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        /// At the very beginning, the initial scene only contains the PersistentController Object, so we need to load
        /// the main menu.
        if (SceneManager.GetActiveScene().buildIndex == 0)
            SceneManager.LoadScene("MainMenu");

        /*
        /// Every scene needs a gameobject containing the loadscreen. It will be used by this persistent script in order to 
        /// appear and show progress whenever a scene is being loaded. This is because this does not work with prefabs.
        GameObject[] objects = Resources.FindObjectsOfTypeAll<GameObject>();
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].tag == "LoadScreen")
            {
                loadScreen = objects[i];
                i = objects.Length;
            }
        }
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].tag == "LoadScreenSlider")
            {
                loadScreenProgress = objects[i].GetComponent<Slider>();
                i = objects.Length;
            }

        }
        */
        PAUSED = false;
    }

    /// <summary>
    /// Update is called every frame.
    /// </summary>
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (PAUSED)
                {
                    Cursor.visible = false;
                    UnPauseGame();
                }
                else
                {
                    Cursor.visible = true;
                    PauseGame();
                }
            }
        }
    }

    /// The next 3 functions are specific to Persistent objects, to help the object know when they are
    /// transitioning to new scenes, and reset some of the variables each time.

    /// <summary>
    /// Called when a scene is being loaded.
    /// </summary>
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Called every time a new scene has been loaded. Resets necessary variables and finsd necessary objects.
    /// </summary>
    /// <param name="scene">Scene loaded</param>
    /// <param name="loadSceneMode">Mode in which the scene is loaded (this program uses asynchronous loading)</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        DontDestroyOnLoad(this.gameObject);

        if (SceneManager.GetActiveScene().name != "InitScene")
        {
            /*
            /// Every scene needs a gameobject containing the loadscreen. It will be used by this persistent script in order to 
            /// appear and show progress whenever a scene is being loaded. This is because this does not work with prefabs.
            GameObject[] objects = Resources.FindObjectsOfTypeAll<GameObject>();
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i].tag == "LoadScreen")
                {
                    loadScreen = objects[i];
                    i = objects.Length;
                }
            }
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i].tag == "LoadScreenSlider")
                {
                    loadScreenProgress = objects[i].GetComponent<Slider>();
                    i = objects.Length;
                }
            }*/
        }
    }

    /// <summary>
    /// Called when exiting the current scene.
    /// </summary>
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Begins the process to load a new scene.
    /// </summary>
    /// <param name="sceneName">The name of the scene to be loaded.</param>
    public void LoadScene(string sceneName)
    {

        PAUSED = false;
        Time.timeScale = 1f;
        StartCoroutine(LoadSceneAsync(sceneName));

    }

    /// <summary>
    /// Handles the starting process of loading a new scene, including fading to black, and enabling the 
    /// loading screen. Loads the scene asynchronously.
    /// </summary>
    /// <param name="sceneName">The name of the scene to be loaded.</param>
    /// <returns>Yield return null during fade in process and loading process to return back to code at the
    /// next frame.</returns>
    IEnumerator LoadSceneAsync(string sceneName)
    {
        routineRunning = true;
        /*if (loadScreen != null)
            loadScreen.SetActive(true);
        Color temp = loadScreen.transform.GetChild(0).GetComponent<Image>().color;
        temp.a = 0F;
        loadScreen.transform.GetChild(0).GetComponent<Image>().color = temp;
        loadScreenProgress.gameObject.SetActive(false);
        loadScreen.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(false);

        while (loadScreen.transform.GetChild(0).GetComponent<Image>().color.a < 1.0F)
        {
            Color curTransparency = loadScreen.transform.GetChild(0).GetComponent<Image>().color;
            curTransparency.a += (Time.deltaTime / 1F) / 1.0F;
            if (curTransparency.a > 1.0F)
                curTransparency.a = 1.0F;
            loadScreen.transform.GetChild(0).GetComponent<Image>().color = curTransparency;
            yield return null;
        }

        loadScreenProgress.gameObject.SetActive(true);
        loadScreen.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(true);
        */
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9F);
            /*if (loadScreenProgress != null)
                loadScreenProgress.value = progress;*/
            yield return null;
        }
        
        routineRunning = false;
    }

    /// <summary>
    /// When the player uses a "Quit" button, this method will be called. It is possible to add
    /// necessary processes that must be executed before the application quits, such as saving 
    /// data into files, making sure it is safe to quit from this point, etc. This method IS NOT
    /// called when the player uses other methods to close the application, such as 'ALT+F4', or
    /// the Task Manager.
    /// </summary>
    public void QuitGame()
    {
        SaveAll();
        Application.Quit();
    }

    /// <summary>
    /// Specific function to load the game scene.
    /// </summary>
    public void PlayGame()
    {
        playerScore = 0;
        UnPauseGame();
        LoadScene("GameScene");
    }

    /// <summary>
    /// Navigates the player to the main menu, either by loading the scene, or simply enabling
    /// the correct canvas.
    /// </summary>
    public void ToMainMenu()
    {
        playerScore = 0;
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            UnPauseGame();
            Cursor.visible = true;
            LoadScene("MainMenu");
        }
        else
            DisplayMenuElement(0);
    }

    /// <summary>
    /// Displays the desired menu in the menu children. Used for accessing options, tutorial, and main menu.
    /// </summary>
    /// <param name="index"></param>
    private void DisplayMenuElement(int index)
    {
        GameObject menuObject = GameObject.FindWithTag("MainMenu");
        int numChildren = menuObject.transform.childCount;
        if (index < numChildren)
        {
            for (int i = 0; i < numChildren; i++)
            {
                if (i == index)
                    menuObject.transform.GetChild(i).gameObject.SetActive(true);
                else if (i != 0) /// The background shouldn't be deleted when going through the menu.
                    menuObject.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        else
            Debug.LogError("Attempted to load invalid menu index");
    }

    /// <summary>
    /// Increments the player score by the given amount.
    /// </summary>
    /// <param name="amount">the amount that the player score will be increased.</param>
    public void IncrementPlayerScore(int amount)
    {
        playerScore += amount;
    }

    /// <summary>
    /// "Pauses" the game. Time.timeScale changes the speed at which processes are executed, so setting it
    /// to 0 will stop any actions that rely on time. Other scripts that run at runtime should check 
    /// if(PersistentDataObject.IsPaused()) in their update function, so that when the game is paused, 
    /// desired scripts will stop what they're doing. Also enables the cursor to let the player navigate the
    /// pause menu.
    /// </summary>
    public void PauseGame()
    {
        PAUSED = true;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    /// <summary>
    /// "Unpauses" the game. Reset Time.timescale back to normal and makes the cursor invisible again.
    /// </summary>
    public void UnPauseGame()
    {
        PAUSED = false;
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Used by other scripts. Lets them know whether or not the game is currently paused.
    /// </summary>
    /// <returns>True if the game is paused, false otherwise.</returns>
    public bool IsPaused()
    {
        return PAUSED;
    }

    /// <summary>
    /// Getter for the player's score.
    /// </summary>
    /// <returns>Value representing the player's score.</returns>
    public int GetPlayerScore()
    {
        return playerScore;
    }

    /// <summary>
    /// Saves all stats into the local game file.
    /// </summary>
    public void SaveAll()
    {
        // SaveData.Save(this);
    }

    /// <summary>
    /// Loads stats from the save file, if it exists. If not, loads new game data.
    /// </summary>
    public void LoadGame()
    {
        /*
        try
        {
            GameData loadedData = SaveData.Load();

            allTimeHighScore = loadedData.AllTimeHighScore;
            allTimeHighRound = loadedData.AllTimeHighRound;
            defaultStages = loadedData.DefaultStages;
            mouseSens = loadedData.MouseSens;
            sfxVolume = loadedData.SfxVolume;
            musicVolume = loadedData.MusicVolume;
        }
        catch
        {
            SaveData.CreateNewGameFile();
            GameData newGameData = SaveData.NewGame();
            allTimeHighScore = newGameData.AllTimeHighScore;
            allTimeHighRound = newGameData.AllTimeHighRound;
            defaultStages = newGameData.DefaultStages;
            mouseSens = newGameData.MouseSens;
            sfxVolume = newGameData.SfxVolume;
            musicVolume = newGameData.MusicVolume;
        }
        */
    }
}