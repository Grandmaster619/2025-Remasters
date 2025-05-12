using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonType
{
    PLAY,
    CONTINUE,
    MAIN,
    QUIT,
    OPTIONS
}

public class ButtonControl : MonoBehaviour
{
    public ButtonType type;

    private PersistentControl persistentObject;

    private void Awake()
    {
        persistentObject = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentControl>();
    }

    public void Click()
    {
        switch(type)
        {
            case ButtonType.PLAY:
                persistentObject.PlayGame();
                break;
            case ButtonType.QUIT:
                persistentObject.QuitGame();
                break;
            case ButtonType.MAIN:
                persistentObject.ToMainMenu();
                break;
            case ButtonType.CONTINUE:
                persistentObject.UnPauseGame();
                break;
            case ButtonType.OPTIONS:
                break;
            default:
                break;
        }
    }
}
