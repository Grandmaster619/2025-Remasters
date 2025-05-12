// Author(s): Lincoln Schroeder
//
// Purpose: This file adds listeners to buttons based on their button type. This is only
// necessary for buttons that interact with the persistent object, because you won't be able
// to assign the button events inside of the Unity editor.
//
// Date Last Modified: 10/31/2019

using UnityEngine;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    public string ButtonType;
    public AudioClip UiClick;
    private PersistentController PersistentInfo;

    // Called when the button is first initiated. Adds the correct listeners depending on the button type.
    private void Awake()
    {
        PersistentInfo = GameObject.FindWithTag("Persistent").GetComponent<PersistentController>();
        Button thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(PersistentInfo.Click);
        if (ButtonType == "Resume")
            thisButton.onClick.AddListener(PersistentInfo.UnPauseGame);
        else if (ButtonType == "Menu")
            thisButton.onClick.AddListener(PersistentInfo.ToMainMenu);
        else if (ButtonType == "Options")
            thisButton.onClick.AddListener(PersistentInfo.SelectOptions);
        else if (ButtonType == "Quit")
            thisButton.onClick.AddListener(PersistentInfo.QuitGame);
        else if (ButtonType == "Play")
            thisButton.onClick.AddListener(PersistentInfo.PlayGame);
        else if (ButtonType == "Unpause")
            thisButton.onClick.AddListener(PersistentInfo.UnPauseGame);
        else if (ButtonType == "Tutorial")
            thisButton.onClick.AddListener(PersistentInfo.SelectTutorial);
        else
            Debug.LogError("Invalid ButtonType");
    }
}
