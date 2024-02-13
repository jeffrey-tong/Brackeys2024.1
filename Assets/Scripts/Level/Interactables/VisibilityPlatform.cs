using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityPlatform : BaseInteractable
{
    private void Start()
    {

    }
    public override void Activate()
    {
        base.Activate();
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
