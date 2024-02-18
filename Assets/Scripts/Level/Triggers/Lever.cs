using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : BaseTrigger
{
    private SpriteRenderer sr;
    [SerializeField] private Sprite sprite1;
    [SerializeField] private Sprite sprite2;
    private bool isLeverOn = false;

    [SerializeField] private AudioClip onClip;
    [SerializeField] private AudioClip offClip;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    public override void Trigger(PlayerController player)
    {
        base.Trigger(player);
        isLeverOn = !isLeverOn;
        AudioManager.Instance.PlayAudioSFX(isLeverOn? onClip : offClip);
        sr.sprite = isLeverOn ? sprite2 : sprite1;

    }

    private bool canTrigger = false;
}
