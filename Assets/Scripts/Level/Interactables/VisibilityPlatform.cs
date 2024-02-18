using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityPlatform : BaseInteractable
{
    [SerializeField] private bool isVisible = true;

    private void Start()
    {
        gameObject.SetActive(isVisible);
    }

    public override void Activate()
    {
        base.Activate();
        isVisible = !isVisible;
        gameObject.SetActive(isVisible);
    }
}
