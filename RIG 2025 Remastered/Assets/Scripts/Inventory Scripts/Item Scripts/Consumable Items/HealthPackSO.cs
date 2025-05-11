using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthPack", menuName = "Item/Health Pack", order = 0)]
public class HealthPackSO : ItemSO
{
    [SerializeField] private int healAmt;

    public override void Use(GameObject playerObject)
    {
        PlayerInventory inventory = playerObject.GetComponent<PlayerInventory>();
        Health playerHealth = playerObject.GetComponent<Health>();
        if (playerHealth != null) {
            Debug.Log("Player has healed " + healAmt + " health");
            playerHealth.Heal(healAmt);
        }
        
        inventory?.RemoveItem(this);
        InventoryMenuController.GetInstance().ClosePlayerInventory();
    }
}
