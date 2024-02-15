using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Door : BaseTrigger
{
    [Header("Components")]
    [SerializeField] private CharacterData CharDataSO;
    [SerializeField] private SpriteRenderer m_SpriteRenderer;



    public static Dictionary<CharacterColor, Color> DoorColorLookup = new Dictionary<CharacterColor, Color>()
    {
        {CharacterColor.GRAY, Color.gray },
        {CharacterColor.BLUE, Color.blue },
        {CharacterColor.RED, Color.red }
    };

    private void Start()
    {
        UpdateDoorColor();
    }

    public override void Trigger(PlayerController controller)
    {
        CharacterData dataSO = controller.GetCharacterData();

        Vector2 position = controller.transform.position;

        Destroy(controller.gameObject);
        Instantiate(CharDataSO.prefab, position, Quaternion.identity);

        SetCharacterData(dataSO);
    }

    public void SetCharacterData(CharacterData newData)
    {
        this.CharDataSO = newData;
        UpdateDoorColor();
    }

    private void UpdateDoorColor()
    {
        if (CharDataSO != null && DoorColorLookup.ContainsKey(CharDataSO.color))
        {
            m_SpriteRenderer.color = DoorColorLookup[CharDataSO.color];
        }
    }
}
