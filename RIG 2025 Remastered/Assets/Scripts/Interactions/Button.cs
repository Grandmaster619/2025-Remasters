using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Interactable
{
    [SerializeField]
    private GameObject door;
    private bool doorOpen;
    
    public KeycodeAudio keycodeAudio;


    // Start is called before the first frame update
    void Start()
    {
        keycodeAudio.OnKeyPress += KeyPressAudio;
    }

    


    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Interact(GameObject sender)
    {
        Debug.Log("Interact for Button called");
        //for clickable buttons
        keycodeAudio.OnKeyPress.Invoke(this, new EventArgs());
        StartCoroutine(SinglePress());
    }

    IEnumerator SinglePress(){
        door.GetComponent<Animator>().SetBool("IsPressed", true);
        yield return new WaitForSeconds(.1f);
        door.GetComponent<Animator>().SetBool("IsPressed", false);

        yield return null;
    }

    private void KeyPressAudio(object sender, EventArgs eventargs)
    {
        keycodeAudio.KeyPressed();
    }
}

