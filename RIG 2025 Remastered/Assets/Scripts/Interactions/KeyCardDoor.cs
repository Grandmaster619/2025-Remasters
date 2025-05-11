using UnityEngine;

public class KeyCardDoor : Interactable
{
    [SerializeField]
    private GameObject door;
    private bool doorOpen;
    public ItemSO KeyCard;

    private AudioSource src;

    public void Start()
    {
        src = GetComponent<AudioSource>();
    }

    protected override void Interact(GameObject sender)
    {
        if (GameObject.Find("FirstPersonPlayer").GetComponent<PlayerInventory>().Contains(KeyCard))
        {
            doorOpen = !doorOpen;
            door.GetComponent<Animator>().SetBool("IsOpen", doorOpen);
            src.Play();
        }
    }
}

