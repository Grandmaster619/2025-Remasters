using UnityEngine;

public class Toggle : Interactable
{
    [SerializeField]
    private GameObject door;
    private bool doorOpen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Interact(GameObject sender)
    {
        //for clickable buttons
        //StartCoroutine(SinglePress());

        //for toggle buttons
        doorOpen = !doorOpen;
        door.GetComponent<Animator>().SetBool("IsOpen", doorOpen);
    }

    /*IEnumerator SinglePress(){
        door.GetComponent<Animator>().SetBool("IsPressed", true);
        yield return new WaitForSeconds(.5f);
        door.GetComponent<Animator>().SetBool("IsPressed", false);

        return null;
    }*/
}

