using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Door : BaseTrigger
{
    [Header("Components")]
    [SerializeField] private CharacterData CharDataSO;
    [SerializeField] private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private AudioClip doorEnteredClip;
    public static event Action<CharacterData> OnAnyDoorEntered;

    private void Start()
    {
        UpdateDoorColor();
    }

    public override void Trigger(PlayerController controller)
    {
        Action FadeIn = () => SwapCharacter(controller);
        TransitionManager.Instance.DoFadeInOut(FadeIn, null);
        if (doorEnteredClip != null) { AudioManager.Instance.PlayAudioSFX(doorEnteredClip); }
    }

    private void SwapCharacter(PlayerController controller)
    {
        CharacterData dataSO = controller.GetCharacterData();

        Vector2 position = controller.transform.position;

        Destroy(controller.gameObject);
        Instantiate(CharDataSO.prefab, position, Quaternion.identity);

        OnAnyDoorEntered?.Invoke(CharDataSO);
        SetCharacterData(dataSO);
    }

    public void SetCharacterData(CharacterData newData)
    {
        this.CharDataSO = newData;
        UpdateDoorColor();
    }

    private void UpdateDoorColor()
    {
        m_SpriteRenderer.color = CharDataSO.color;
    }
}
