using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JournalEntry
{
    public int id;
    public string entryName;
    public string description;
    public bool collected;
}

[System.Serializable]
public class JournalData
{
    public List<JournalEntry> journals;
}

public class Inventory : MonoBehaviour
{
    public static int TOTAL_AMOUNT_OF_JOURNALS = 5;

    private InputHandler inputHandler;
    public int numberOfJournals;
    public int[] journalArray = new int[TOTAL_AMOUNT_OF_JOURNALS];
    public JournalData journalData;
    public bool hasFlashlight;

    private void Start()
    {
        numberOfJournals = 0;
        // Load your JSON data into the journalData object
        string json = "{\"journals\":[{\"id\":0,\"entryName\":\"Sample Entry\",\"description\":\"This is a sample journal entry. Replace this with your actual content.\",\"collected\":false},{\"id\":1,\"entryName\":\"Sample Entry 2\",\"description\":\"This is a sample journal entry. Replace this with your actual content. 2\",\"collected\":false},{\"id\":2,\"entryName\":\"Sample Entry 3\",\"description\":\"This is a sample journal entry. Replace this with your actual content. 3\",\"collected\":false},{\"id\":3,\"entryName\":\"Sample Entry 4\",\"description\":\"This is a sample journal entry. Replace this with your actual content. 4\",\"collected\":false},{\"id\":4,\"entryName\":\"Sample Entry 5\",\"description\":\"This is a sample journal entry. Replace this with your actual content. 5\",\"collected\":false}]}";
        Debug.Log("Original JSON: " + json);
        hasFlashlight = false;
        journalData = JsonUtility.FromJson<JournalData>(json);

        if (journalData == null)
        {
            Debug.LogError("Failed to deserialize JSON data.");
            return;
        }

        // Ensure the journals list is initialized
        if (journalData.journals == null)
        {
            Debug.LogWarning("Journals list is null in the loaded JSON data.");
            journalData.journals = new List<JournalEntry>();
        }
        else if (journalData.journals.Count < TOTAL_AMOUNT_OF_JOURNALS)
        {
            Debug.LogWarning("Journals list does not have enough elements.");
        }

        //journalData.journals = new List<JournalEntry>();

        Debug.Log("JournalData Count: " + journalData.journals.Count);
        Debug.Log("JournalData First Entry Collected: " + journalData.journals[0].collected);
        
    }

    public bool getFlashlightState()
    { return hasFlashlight; }

    public void setFlashlightState(bool flashlight)
    { hasFlashlight=flashlight; }

    public void JournalCollected(int ID)
    {
       

        if (ID >= 0 && ID < TOTAL_AMOUNT_OF_JOURNALS)
        {
            numberOfJournals++;
            journalArray[ID] = 1;
            Debug.Log("Journal collected");

            // Set collected as true in the JSON data
            journalData.journals[ID].collected = true;
        }
        else
        {
            Debug.LogError("Invalid journal ID: " + ID);
        }
        Debug.Log("unique journal id: "+journalData.journals[ID].id);
        Debug.Log("number of journals: "+numberOfJournals);
    }
}