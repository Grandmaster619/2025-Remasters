using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public string targetSceneName;
    public Collider hitbox;

    private void OnTriggerEnter(Collider hitbox) {
        if (hitbox.TryGetComponent(out PlayerInventory playerInventory))
            InventoryLoader.GetInstance().SetInventoryEntries(playerInventory.GetInventoryEntryList());
        SceneManager.LoadScene(targetSceneName);
    }
}
