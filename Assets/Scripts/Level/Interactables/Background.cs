using System;
using UnityEngine;

public class Background : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private SpriteRenderer backgroundObjects;
    [SerializeField] private BackgroundColor BackgroundColor;
    private void Awake()
    {
        if (background == null)
            background = GetComponent<SpriteRenderer>();   

        if (backgroundObjects == null)
            backgroundObjects = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        PlayerController.OnPlayerChanged += OnPlayerChanged_Callback;
        OnPlayerChanged_Callback();
    }

    private void OnPlayerChanged_Callback()
    {
        var data = BackgroundColor.GetColorData(PlayerController.Current.GetCharacterData());
        if (background != null)
        {
            background.color = data.backgroundColor;
        }

        if (backgroundObjects != null)
        {
            backgroundObjects.color = data.backgroundObjectColor;
        }
    }

    private void OnValidate()
    {
        var data = BackgroundColor.GetDefault() ;
        if (background != null)
        {
            background.color = data.backgroundColor;
        }

        if (backgroundObjects != null)
        {
            backgroundObjects.color = data.backgroundObjectColor;
        }
    }
}
