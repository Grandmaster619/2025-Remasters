using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeLeverUI : Interactable
{
    [SerializeField] private string CompletedPromptMessage = "Oil refinery has been fixed!";
    private int placeNum = 1;
    public PlayerInventory inventory;
    private AudioSource src;
    public AudioSource src2;

    [SerializeField] private MeshRenderer TopLight;
    [SerializeField] private MeshRenderer BottomLight;
    [SerializeField] private Material GreenLight;


    private void Start()
    {
        src = GetComponent<AudioSource>();
    }

    protected override void Interact(GameObject sender)
    {
        if (placeNum > 4)
            return;

        ItemSO shorthand = SODatabase.GetInstance().GetItemByName("Handle");
        if(inventory.Contains(shorthand))
        {
            inventory.RemoveItem(shorthand);
            GetComponent<Animator>().SetInteger("LeverNum", placeNum++);
            src.Play();
            if (placeNum == 4)
            {
                src2.Play();
                TopLight.material = GreenLight;
                BottomLight.material = GreenLight;
                promptMessage = CompletedPromptMessage;
                GameManagerScene2.GetInstance().ActivateDrillControlPanel();
            }   
        }
    }
}
