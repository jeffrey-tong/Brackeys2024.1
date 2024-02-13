using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestInteractableManager : MonoBehaviour
{
    [SerializeField] private List<BaseTrigger> triggers = new List<BaseTrigger>();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            foreach(BaseTrigger trigger in triggers)
            {
                trigger.Trigger();
            }
        }
    }
}
