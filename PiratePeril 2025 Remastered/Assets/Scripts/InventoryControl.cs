using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryControl : MonoBehaviour
{
    public GameObject inventoryDisplay;
    public GameObject cutlassObject;
    public GameObject jewelObject;
    public GameObject musketObject;
    public GameObject grogObject;

    private PersistentControl persistentObject;
    private bool displaying;

    private void Start()
    {
        persistentObject = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentControl>();
    }

    // Update is called once per frame
    void Update()
    {
        List<Collectibles> collectibles = persistentObject.Collected;
        cutlassObject.SetActive(collectibles.Contains(Collectibles.CANDESCENT_CUTLASS));
        jewelObject.SetActive(collectibles.Contains(Collectibles.JUMPING_JEWEL));
        musketObject.SetActive(collectibles.Contains(Collectibles.MIKE_THE_MUSKET));
        grogObject.SetActive(collectibles.Contains(Collectibles.GROG));

        if(Input.GetKeyDown(KeyCode.I) && !displaying)
        {
            inventoryDisplay.SetActive(true);
            Time.timeScale = 0;
            displaying = true;
        }
        else if(Input.GetKeyDown(KeyCode.I) && displaying)
        {
            inventoryDisplay.SetActive(false);
            Time.timeScale = 1;
            displaying = false;
        }
    }
}
