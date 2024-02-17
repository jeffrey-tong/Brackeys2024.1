using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkingDoor : Door
{
    private float shrinkFactor = 0.5f;
    private Vector3 defaultScale = Vector3.one;

    public override void Trigger(PlayerController controller)
    {
        if (controller.transform.localScale.magnitude >= defaultScale.magnitude)
        {
            Action FadeIn = () => ShrinkCharacter(controller);
            TransitionManager.Instance.DoFadeInOut(FadeIn, null);
        }
        else
        {
            Action FadeIn = () => ExpandCharacter(controller);
            TransitionManager.Instance.DoFadeInOut(FadeIn, null);
        }
    }

    private void ShrinkCharacter(PlayerController controller)
    {
        Vector3 newScale = new Vector3(defaultScale.x * shrinkFactor, defaultScale.y * shrinkFactor, defaultScale.z * shrinkFactor);
        controller.transform.localScale = newScale;
    }

    private void ExpandCharacter(PlayerController controller)
    {
        controller.transform.localScale = defaultScale;
    }
}
