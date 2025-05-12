using UnityEngine;
using TMPro;
using System;

public class PassCheck : MonoBehaviour
{
    public char[] characterArray;
    public int inputInt;
    public TextMeshProUGUI textMeshPro;
    private int currentIndex = 0;
    public Animator animator;
    public bool OpenedOnce = false;

    public EventHandler<EventArgs> OnKeycodeCorrect;

    void Start()
    {
        characterArray = new char[5];

        UpdateText();
    }

    public void AddToCharacterArray(int input)
    {
        if (currentIndex < characterArray.Length)
        {
            char character = (char)(input + '0');
            characterArray[currentIndex] = character;
            currentIndex++;
            UpdateText();
        }
        else
        {
            Debug.Log("Character array is full!");
        }
    }

    public void UpdateText()
    {
        textMeshPro.text = new string(characterArray);
    }

    public void ResetText()
    {
        characterArray = new char[5];
        currentIndex = 0;

        UpdateText();
    }

    public void CheckText()
    {
        Debug.Log("Checking...");
        if (new string(characterArray) == "11370")
        {
            animator.SetBool("IsOpen", true);
            textMeshPro.text = "OK";
            OnKeycodeCorrect.Invoke(this, new EventArgs());
            OpenedOnce = true;
        }
        else
        {
            textMeshPro.text = "ERROR";
        }
    }
}