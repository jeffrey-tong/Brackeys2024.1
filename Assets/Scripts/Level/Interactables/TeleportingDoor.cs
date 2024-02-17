using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportingDoor : Door
{
    [SerializeField] private Transform endPoint;

    public override void Trigger(PlayerController controller)
    {
        Action FadeIn = () => TeleportCharacter(controller);
        TransitionManager.Instance.DoFadeInOut(FadeIn, null);
    }

    private void TeleportCharacter(PlayerController controller)
    {
       controller.transform.position = endPoint.position;
    }

}
