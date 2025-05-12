// Author(s): Lincoln Schroeder
//
// Purpose: This file contains all actions that require a Persistent GameObject in order to function properly,
// such as loading the next scene (with a loading screen), storing player/game data that is used in other scenes,
// and pausing the game. **There should only be ONE Persistent GameObject holding this script. It needs to be
// tagged as "Persistent", and multiple of these objects could cause massive confusion.**
//
// Date Last Modified: 10/31/2019

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PersistentController : MonoBehaviour
{
    [SerializeField] private GameObject PlayerObject, GameOverScreenPrefab;
    private bool PAUSED;
    private int playerScore, enemyCount, lives, additionaLife;
    
    // Can be implemented later.
    /*
    
    [SerializeField]
    private GameObject GameWonScreenPrefab;
    */

    private GameObject loadScreen;
    private Slider loadScreenProgress;
    private bool routineRunning;
    public AudioSource playButtonSnd, exitGameSnd, mainMenuSnd, openMenuSnd, closeMenuSnd, tutorialButtonSnd;

    private int Highscore;
    [SerializeField] private Text HighscoreText;

    // Only runs on the very first startup of the game.
    private void Start()
    {
        
        routineRunning = false;
        playerScore = 0;
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        // At the very beginning, the initial scene only contains the PersistentController Object, so we need to load
        // the main menu.
        if (SceneManager.GetActiveScene().buildIndex == 0)
            SceneManager.LoadScene("MainMenu");

        // Every scene needs a gameobject containing the loadscreen. It will be used by this persistent script in order to 
        // appear and show progress whenever a scene is being loaded. This is because this does not work with prefabs.
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
        PAUSED = false;
    }

    // Update function. Called every frame. Currently the only implementation in this method
    // is to check if the player pressed the Escape key to pause/unpause the game.
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (PAUSED)
                {
                    UnPauseGame();
                }
                else
                {
                    PauseGame();
                }
            }

            //if (lives > 0 && GameObject.Find("Player") == null)
            //    Instantiate(PlayerObject).name = "Player";
            //else if(lives == 0 && GameObject.Find("GameOverCanvas") == null)
            //{
            //    Cursor.visible = true;

            //    foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            //    {
            //        if (enemy.layer == 9)
            //            enemy.GetComponent<EnemyController>().enabled = false;
            //        else
            //            enemy.GetComponent<BossHandler>().enabled = false;
            //    }

            //    Instantiate(GameOverScreenPrefab, GameObject.Find("UI").transform).name = "GameOverCanvas";
            //}
        }
    }

    // The next 3 functions are specific to Persistent objects, to help the object know when they are
    // transitioning to new scenes, and reset some of the variables each time.

    // Called when a scene is being loaded.
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Called each time a scene is loaded. Resets necessary variables that rely on the current scene.
    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        DontDestroyOnLoad(this.gameObject);

        if (SceneManager.GetActiveScene().name != "InitScene")
        {
            // Every scene needs a gameobject containing the loadscreen. It will be used by this persistent script in order to 
            // appear and show progress whenever a scene is being loaded. This is because this does not work with prefabs.
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
        }
    }

    // Called when a scene is being "de-loaded".
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Begins the process to load the desired scene.
    // @param sceneName: string, the name of the scene to be loaded.
    public void LoadScene(string sceneName)
    {
        PAUSED = false;
        Time.timeScale = 1f;
        if (SceneManager.GetActiveScene().name == sceneName)
            SceneManager.LoadScene(sceneName);
        else
            StartCoroutine(LoadSceneAsync(sceneName));

    }

    // Handles the fade in and loading bar for the loading screen.
    // @param sceneName: string, the name of the scene to be loaded.
    IEnumerator LoadSceneAsync(string sceneName)
    {
        routineRunning = true;
        if (loadScreen != null)
            loadScreen.SetActive(true);
        Color temp = loadScreen.transform.GetChild(0).GetChild(0).GetComponent<Image>().color;
        temp.a = 0F;
        loadScreen.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = temp;
        loadScreenProgress.gameObject.SetActive(false);

        while (loadScreen.transform.GetChild(0).GetChild(0).GetComponent<Image>().color.a < 1.0F)
        {
            Color curTransparency = loadScreen.transform.GetChild(0).GetChild(0).GetComponent<Image>().color;
            curTransparency.a += (Time.deltaTime / 1F) / 1.0F;
            if (curTransparency.a > 1.0F)
                curTransparency.a = 1.0F;
            loadScreen.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = curTransparency;
            yield return null;
        }

        loadScreenProgress.gameObject.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9F);
            if (loadScreenProgress != null)
                loadScreenProgress.value = progress;
            yield return null;
        }
        routineRunning = false;
    }

    // When the player uses a "Quit" button, this method will be called. It is possible to add
    // necessary processes that must be executed before the application quits, such as saving 
    // data into files, making sure it is safe to quit from this point, etc. This method IS NOT
    // called when the player uses other methods to close the application, such as 'ALT+F4', or
    // the Task Manager.
    public void QuitGame()
    {
        exitGameSnd.Play();
        Application.Quit();
    }

    // Specfic function to load the main game scene.
    public void PlayGame()
    {
        playButtonSnd.Play();
        UnPauseGame();
        LoadScene("Game");
        playerScore = 0;
        lives = 5;
        enemyCount = 0;
        additionaLife = 10000;
    }

    // For the main menu, displays the options screen and disables other screens.
    public void SelectOptions()
    {
        DisplayMenuElement(2);
    }

    // For the main menu, displays the tutorial screen and disables other screens.
    public void SelectTutorial()
    {
        DisplayMenuElement(3);
        tutorialButtonSnd.Play();
    }

    // Navigates the player to the main menu, by either loading the main menu scene or switching to the 
    // main menu screen.
    public void ToMainMenu()
    {
        mainMenuSnd.Play();
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            UnPauseGame();
            Cursor.visible = true;
            LoadScene("MainMenu");
        }
        else
            DisplayMenuElement(1);
    }

    // In the list of menu types, displays the desired menu.
    // @param index: int, the index of the menu to be added.
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
                else if (i != 0) // The background shouldn't be deleted when going through the menu.
                    menuObject.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        else
            Debug.LogError("Attempted to load invalid menu index");
    }

    // Increments the player score by the desired amount.
    // @param amount: int, the amount the score should be incremented (or decremented, if negative).
    public void IncrementPlayerScore(int amount)
    {
        playerScore += amount;
    }

    // "Pauses" the game. Time.timeScale changes the speed at which processes are executed, so setting it
    // to 0 will stop any actions that rely on time. Other scripts that run at runtime should check 
    // if(PersistentDataObject.IsPaused()) in their update function, so that when the game is paused, 
    // desired scripts will stop what they're doing. Also enables the cursor to let the player navigate the
    // pause menu.
    public void PauseGame()
    {
        openMenuSnd.Play();
        PAUSED = true;
        Cursor.visible = true;
        Time.timeScale = 0f;

        if(GameObject.Find("Background Music") != null)
            GameObject.Find("Background Music").GetComponent<AudioSource>().Pause();
    }

    // "Unpauses" the game. Reset Time.timescale back to normal and makes the cursor invisible again.
    public void UnPauseGame()
    {
        closeMenuSnd.Play();
        PAUSED = false;
        Cursor.visible = false;
        Time.timeScale = 1f;

        if (GameObject.Find("Background Music") != null)
            GameObject.Find("Background Music").GetComponent<AudioSource>().Play();
    }

    // Lets other scripts know if the game is paused or not.
    // @return PAUSED: bool, true if the game is paused, false otherwise.
    public bool IsPaused()
    {
        return PAUSED;
    }

    // Getter for playerScore.
    // @return playerScore: int.
    public int GetPlayerScore()
    {
        return playerScore;
    }

    public int GetAdditionalLife()
    {
        return additionaLife;
    }

    // Plays the button click sound. Currently there is no sound.
    public void Click()
    {
        //GetComponent<AudioSource>().Play();
    }

    public int GetEnemyCount()
    {
        return enemyCount;
    }

    public void SetEnemyCount(int enemyCount)
    {
        this.enemyCount = enemyCount;
    }

    public void DecEnemyCount(int points)
    {
        enemyCount--;
        this.IncrementPlayerScore(points);

        if(playerScore >= additionaLife)
        {
            lives++;
            additionaLife *= 2;
        }
    }

    public int GetLives()
    {
        return lives;
    }

    public void DecLives()
    {
        Destroy(GameObject.Find("Player"));
        
        lives--;

        if (lives > 0)
        {
            Instantiate(PlayerObject).name = "Player";
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                enemy.GetComponent<ParticleLauncher>().ClearAllBullets();
        }
        else
        {
            Cursor.visible = true;
            Instantiate(GameOverScreenPrefab);
            if(playerScore > Highscore)
            {
                Highscore = playerScore;
                HighscoreText.text = "Highscore: " + playerScore;
            }
        }
    }

    // These next methods are not applicable yet, but can eventually be implemented. They simply display the
    // screens that let the player know if they won or lost. These screens can have the player's final score, maybe
    // a leaderboard, whatever. Right now, they are not being used.

    /*
    public void GameLost()
    {
        if (!routineRunning)
            StartCoroutine(DisplayGameOver());
    }

    public void GameWon()
    {
        if (!routineRunning)
            StartCoroutine(DisplayWon());
    }

    IEnumerator DisplayGameOver()
    {
        routineRunning = true;
        PAUSED = false;
        Time.timeScale = 1f;
        float timeElapsed = 0.0f;
        GameObject screen = GameObject.Instantiate(GameOverScreenPrefab);
        Color temp = screen.transform.GetChild(0).GetChild(0).GetComponent<Image>().color;
        temp.a = 0F;
        screen.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = temp;
        screen.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);

        while (screen.transform.GetChild(0).GetChild(0).GetComponent<Image>().color.a < 1.0F)
        {
            Color curTransparency = screen.transform.GetChild(0).GetChild(0).GetComponent<Image>().color;
            curTransparency.a += (Time.deltaTime / 1F) / 4.5F;
            if (curTransparency.a > 1.0F)
                curTransparency.a = 1.0F;
            screen.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = curTransparency;

            yield return null;
        }

        screen.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);

        while (timeElapsed < 3f)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        routineRunning = false;
        ToMainMenu();
    }

    IEnumerator DisplayWon()
    {
        routineRunning = true;
        PAUSED = false;
        Time.timeScale = 1f;
        float timeElapsed = 0.0f;
        GameObject screen = GameObject.Instantiate(GameWonScreenPrefab);
        Color temp = screen.transform.GetChild(0).GetChild(0).GetComponent<Image>().color;
        temp.a = 0F;
        screen.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = temp;
        screen.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);

        while (screen.transform.GetChild(0).GetChild(0).GetComponent<Image>().color.a < 1.0F)
        {
            Color curTransparency = screen.transform.GetChild(0).GetChild(0).GetComponent<Image>().color;
            curTransparency.a += (Time.deltaTime / 1F) / 3.0F;
            if (curTransparency.a > 1.0F)
                curTransparency.a = 1.0F;
            screen.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = curTransparency;
            yield return null;
        }

        screen.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);

        while (timeElapsed < 3f)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        routineRunning = false;
        ToMainMenu();
    }
    */
}