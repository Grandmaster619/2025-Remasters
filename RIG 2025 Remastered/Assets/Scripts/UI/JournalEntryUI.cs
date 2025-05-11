using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JournalEntryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI journalText;

    public void Start()
    {
        gameObject.SetActive(false);
    }

    public void OpenJournalEntryUI(JournalEntrySO journalEntry)
    {
        Time.timeScale = 0;
        journalText.text = journalEntry.GetJournalEntryText();
        Debug.Log("Journal Entry activated");
        gameObject.SetActive(true);
    }

    public void CloseJournalEntryUI()
    {
        Time.timeScale = 1;
        PauseMenu.GetInstance().SetCanOpenPauseMenu(true);
        gameObject.SetActive(false);
    }
}
