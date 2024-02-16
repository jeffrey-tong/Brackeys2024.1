using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseTrigger : MonoBehaviour, ITrigger
{
    [SerializeField] private List<BaseInteractable> interactablesList = new List<BaseInteractable>();

    public static event Action<BaseTrigger> OnTriggered; // Event for when player enters the door
    public static event Action<BaseTrigger> OnEntered; // Event for when player is near the door 
    public static event Action<BaseTrigger> OnExit; // 

    public virtual void Trigger(PlayerController player)
    {
        foreach(BaseInteractable interactable in interactablesList)
        {
            if (interactable.GetCanInteract())
            {
                interactable.Activate();
            }
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            playerController.SetDoor(this);
            OnEntered?.Invoke(this);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();

            if (playerController.ClearTrigger(this))
                OnExit?.Invoke(this);
        }
    }
}
