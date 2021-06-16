using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool OneTimeInteract = false;

    private bool Interacted = false;

    public void Interact(object o)
    {
        if (!OneTimeInteract || !Interacted)
        {
            Interacted = true;
            OnInteract(o);
        }
    }
    public abstract void OnInteract(object o);

}
