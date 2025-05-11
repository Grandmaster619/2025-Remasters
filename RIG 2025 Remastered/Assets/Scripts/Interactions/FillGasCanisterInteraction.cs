using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillGasCanisterInteraction : Interactable
{
    [SerializeField] private GameObject MainSoundtrackAudioPrefab;
    protected override void Interact(GameObject sender)
    {
        PlayerInventory inventory = sender.GetComponent<PlayerInventory>();
        if (inventory == null)
            return;

        ItemSO shorthand = SODatabase.GetInstance().GetItemByName("Gas Can");
        if (inventory.Contains(shorthand) && GameManagerScene2.GetInstance().IsDrillControlPanelActive()) {
            inventory.RemoveItem(shorthand);
            inventory.AddItem(SODatabase.GetInstance().GetItemByName("Filled Gas Can"), 1);
            promptMessage = "Gas can filled!";
            Instantiate(MainSoundtrackAudioPrefab);
        }
    }

    private void Start()
    {
        StartCoroutine(IsDrillPanelActiveCheck());
        promptMessage = "Control panel must be fixed to fill gas!";
    }

    IEnumerator IsDrillPanelActiveCheck()
    {
        while (!GameManagerScene2.GetInstance().IsDrillControlPanelActive())
        {
            yield return new WaitForSeconds(3f);
        }

        promptMessage = "Press 'E' To Fill Gas Can";
    }
}
