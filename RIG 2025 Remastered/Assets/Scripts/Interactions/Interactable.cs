using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string promptMessage;
    //add or remove an InteractionEvent component to this object
    public bool useEvents;

    public void BaseInteract(GameObject sender)
    {
        //first do an event
        if(useEvents)
            GetComponent<InteractionEvent>().OnInteract.Invoke();
        //then run the interact function
        Interact(sender);
    }

    protected virtual void Interact(GameObject sender)
    {
        //no code here, this is a template function that will be overwritten by a subclass
    }
}
