using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTrigger : MonoBehaviour, ITrigger
{
    [SerializeField] private List<BaseInteractable> interactablesList = new List<BaseInteractable>();

    public virtual void Trigger()
    {
        foreach(BaseInteractable interactable in interactablesList)
        {
            if (interactable.GetCanInteract())
            {
                interactable.Activate();
            }
        }
    }
}
