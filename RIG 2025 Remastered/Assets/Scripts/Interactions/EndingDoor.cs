using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingDoor : Interactable
{
    [SerializeField]
    private GameObject door;
    private bool doorOpen;
    public ItemSO GasCan;

    protected override void Interact(GameObject sender)
    {
        if (GameObject.Find("FirstPersonPlayer").GetComponent<PlayerInventory>().Contains(GasCan))
        {
            SceneManager.LoadScene(3);
        }
    }
}

