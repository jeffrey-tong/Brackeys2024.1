using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTrigger : MonoBehaviour, ITrigger
{
    [SerializeField] private List<BaseInteractable> interactablesList = new List<BaseInteractable>();
    private bool canTrigger = true;
    public virtual void Trigger()
    {
        canTrigger = true;
        foreach(BaseInteractable interactable in interactablesList)
        {
            if (interactable.GetCanInteract())
            {
                continue;
            }
            canTrigger = false;
        }
        if (canTrigger)
        {
            foreach (BaseInteractable interactable in interactablesList)
            {
                interactable.Activate();
            }
        }
    }
}
