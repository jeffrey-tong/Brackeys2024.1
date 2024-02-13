using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestInteractableManager : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> interactables = new List<MonoBehaviour>();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            foreach(IInteractable interactable in interactables)
            {
                interactable.Activate();
            }
        }
    }
}
