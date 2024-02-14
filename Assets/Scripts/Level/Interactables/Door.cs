using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Door : MonoBehaviour, IInteractable
{
    [Header("Components")]
    [SerializeField] private CharacterData CharDataSO;
    [SerializeField] private SpriteRenderer m_SpriteRenderer;

    public static event Action<Door> OnAnyDoorEntered; // Event for when player enters the door
    public static event Action<Door> OnAnyDoorHovered; // Event for when player is near the door 
    public static event Action<Door> OnAnyDoorExit; // 

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

    public void Activate()
    {
        
    }

    public void EnterDoor(PlayerController controller)
    {
        CharacterData dataSO = controller.GetCharacterData();

        Vector2 position = controller.transform.position;

        Destroy(controller.gameObject);
        Instantiate(CharDataSO.prefab, position, Quaternion.identity);

        SetCharacterData(dataSO);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            playerController.SetDoor(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            playerController.ClearDoor(this);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.W))
            {

                PlayerController controller = collision.GetComponent<PlayerController>();

            }
        }
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
