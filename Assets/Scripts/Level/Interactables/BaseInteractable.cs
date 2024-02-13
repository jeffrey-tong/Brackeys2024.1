using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInteractable : MonoBehaviour, IInteractable
{
    public bool canInteract = true;
    public virtual void Activate()
    {

    }

    public bool GetCanInteract()
    {
        return canInteract;
    }
}
