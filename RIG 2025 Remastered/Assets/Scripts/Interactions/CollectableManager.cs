using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CollectableManager : MonoBehaviour
{
    private const string jsonFileName = "journals.json";

    public void MarkCollectableAsCollected(string entryName)
    {
        // Load existing entries from the JSON file
        JournalEntry[] entries = LoadEntries();

        // Find the entry with the specified name
        JournalEntry entryToUpdate = System.Array.Find(entries, entry => entry.entryName == entryName);

        if (entryToUpdate != null)
        {
            // Update the collected status
            entryToUpdate.collected = true;

            // Save the updated entries back to the JSON file
            SaveEntries(entries);
        }
        else
        {
            Debug.LogError("Collectable entry not found: " + entryName);
        }
    }

    private JournalEntry[] LoadEntries()
    {
        string filePath = Path.Combine(Application.persistentDataPath, jsonFileName);

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<JournalEntry[]>(json);
        }
        else
        {
            // If the file doesn't exist, create an empty array
            return new JournalEntry[0];
        }
    }

    private void SaveEntries(JournalEntry[] entries)
    {
        string filePath = Path.Combine(Application.persistentDataPath, jsonFileName);
        string json = JsonUtility.ToJson(entries, true);

        File.WriteAllText(filePath, json);
    }
}
