using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentController : MonoBehaviour
{
    [SerializeField] private List<string> gameLevels;
    [SerializeField] private Texture2D cursor;
    public bool PAUSED = false;
    public bool musicToggle = true;

    private Scene curScene;
    private int curLevel = -1;
    public int CurLevel { get => curLevel; set => curLevel = value; }
    private int curLives = 5;
    public int CurLives { get => curLives; set => curLives = value; }

    private Dictionary<TankType, int> tanksKilled = new Dictionary<TankType, int>()
    {
        { TankType.BROWN, 0 },
        { TankType.GREY, 0 },
        { TankType.TEAL, 0 },
        { TankType.YELLOW, 0 },
        { TankType.RED, 0 },
        { TankType.GREEN, 0 },
        { TankType.BLACK, 0 }
    };

    public Dictionary<TankType, int> TanksKilled { get => tanksKilled; }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        curScene = SceneManager.GetActiveScene();
        if(curScene.name == "Init")
        {
            SceneManager.LoadScene("MainMenu");
        }
        Vector2 cursorHotspot = new Vector2(cursor.width / 2, cursor.height / 2);
        Cursor.SetCursor(cursor, cursorHotspot, CursorMode.ForceSoftware);
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
        if (scene.name == "MainMenu")
            ShowMainMenu();
    }

    /// <summary>
    /// Called when exiting the current scene.
    /// </summary>
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (PAUSED)
                UnPause();
            else
                Pause();
        }
    }

    public void ToMainMenu()
    {
        FindObjectOfType<AudioManager>().Stop("theme");
        curLevel = -1;
        UnPause();
        if(SceneManager.GetActiveScene().name != "MainMenu")
        {
            SceneManager.LoadScene("MainMenu");
        }
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        LoadMenuElement(0);
    }

    public void ShowCredits()
    {
        //LoadMenuElement(1);
    }

    public void ShowOptions()
    {
        //LoadMenuElement(2);
    }

    public void ShowTutorial()
    {
        LoadMenuElement(1);
    }

    public void UnPause()
    {
        Time.timeScale = 1f;
        PAUSED = false;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        PAUSED = true;
    }

    private void LoadMenuElement(int index)
    {
        GameObject menuObject = GameObject.FindWithTag("MenuItems");
        int numChildren = menuObject.transform.childCount;
        if (index < numChildren)
        {
            for (int i = 0; i < numChildren; i++)
            {
                if (i == index)
                    menuObject.transform.GetChild(i).gameObject.SetActive(true);
                else
                    menuObject.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        else
            Debug.LogError("Attempted to load invalid menu index");
    }

    public void ResetTankKills()
    {
        tanksKilled = new Dictionary<TankType, int>()
        {
            { TankType.BROWN, 0 },
            { TankType.GREY, 0 },
            { TankType.TEAL, 0 },
            { TankType.YELLOW, 0 },
            { TankType.RED, 0 },
            { TankType.GREEN, 0 },
            { TankType.BLACK, 0 }
        };
    }

    public void LoadPlayScene(int level=-1)
    {
        UnPause();
        if (level == 5 || level == 10 || level == 15)
            curLives += 1;
        if (level == -1)
            SceneManager.LoadScene("TestLevel");
        if (level == 20)
            ToVictoryScreen();
        else
            SceneManager.LoadScene(gameLevels[level]);
        curScene = SceneManager.GetActiveScene();
    }

    public void GameOver()
    {
        // do game over stuff, return to main menu
    }

    public void AddKill(TankType tankType)
    {
        tanksKilled[tankType] += 1;
    }

    public void StartGame()
    {
        if(musicToggle) 
            FindObjectOfType<AudioManager>().Play("theme");
        UnPause();
        ResetTankKills();
        curLevel = 0;
        curLives = 5;
        LoadPlayScene(0);
    }

    public void ToEndScreen()
    {
        SceneManager.LoadScene("EndScreen");
    }

    public void ToVictoryScreen()
    {
        FindObjectOfType<AudioManager>().Stop("theme");
        SceneManager.LoadScene("VictoryScreen");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
