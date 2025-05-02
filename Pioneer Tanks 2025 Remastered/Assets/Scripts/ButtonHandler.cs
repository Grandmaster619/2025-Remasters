using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ButtonType
{
    PLAY,
    ADJUSTLEVEL,
    QUIT,
    OPTIONS,
    CREDITS,
    MAIN,
    PAUSE,
    RESTART,
    HOWTOPLAY, 
    MUSIC
}

public class ButtonHandler : MonoBehaviour
{
    private PersistentController persistent;

    [SerializeField]
    private ButtonType buttonType;
    [SerializeField]
    private int level = -1;

    private void Awake()
    {
        persistent = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentController>();

        switch(buttonType)
        {
            case ButtonType.PLAY:
                transform.GetComponent<Button>().onClick.AddListener(Play);
                break;
            case ButtonType.ADJUSTLEVEL:
                transform.GetComponent<Button>().onClick.AddListener(delegate { AdjustLevel(level); });
                break;
            case ButtonType.QUIT:
                transform.GetComponent<Button>().onClick.AddListener(Quit);
                break;
            case ButtonType.OPTIONS:
                transform.GetComponent<Button>().onClick.AddListener(Options);
                break;
            case ButtonType.CREDITS:
                transform.GetComponent<Button>().onClick.AddListener(Credits);
                break;
            case ButtonType.MAIN:
                transform.GetComponent<Button>().onClick.AddListener(ToMainMenu);
                break;
            case ButtonType.PAUSE:
                transform.GetComponent<Button>().onClick.AddListener(Pause);
                break;
            case ButtonType.HOWTOPLAY:
                transform.GetComponent<Button>().onClick.AddListener(HowToPlay);
                break;
            case ButtonType.RESTART:
                transform.GetComponent<Button>().onClick.AddListener(Restart);
                break;
            case ButtonType.MUSIC:
                transform.GetComponent<Button>().onClick.AddListener(Music);
                break;
            default:
                break;
        }
    }

    private void Play()
    {
        // Call persistent object to load correct scene based off of current level selection.
        persistent.StartGame();
    }

    private void Quit()
    {
        // Call persistent object to perform any data save necessary, then go through quit process.
        persistent.QuitGame();
    }

    private void Options()
    {
        // Call persistent object to display options UI.
        persistent.ShowOptions();
    }

    private void Credits()
    {
        // Call persistent object to display credits UI.
        persistent.ShowCredits();
    }

    private void AdjustLevel(int level)
    {
        // Call persistent object to change current level selection.
    }

    private void ToMainMenu()
    {
        // Call persistent object to return to the Main Menu.
        persistent.ToMainMenu();
    }

    private void Pause()
    {
        if (persistent.PAUSED)
            persistent.UnPause();
        else
            persistent.Pause();
    }

    private void Restart()
    {
        persistent.StartGame();
    }

    private void HowToPlay()
    {
        persistent.ShowTutorial();
    }

    private void Music()
    {
        FindObjectOfType<AudioManager>().ToggleMusic();
        persistent.musicToggle = !persistent.musicToggle;
    }

}
