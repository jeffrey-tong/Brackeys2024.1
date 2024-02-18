using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image image;
    private TextMeshProUGUI textMesh;

    [SerializeField] private Color imageNormalColor;
    [SerializeField] private Color imageHighlightColor;

    [SerializeField] private Color normalColor = new Color(244f, 248f, 232f);
    [SerializeField] private Color hoverColor = new Color(123f, 28f, 20f);

    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;

    private void Awake()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnValidate()
    {
        image.color = imageNormalColor;
        GetComponentInChildren<TextMeshProUGUI>().color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = imageHighlightColor;
        textMesh.color = hoverColor;

        if (hoverSound != null)
            AudioManager.Instance.PlayAudioSFX(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = imageNormalColor;
        textMesh.color = normalColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        image.color = imageHighlightColor;
        textMesh.color = hoverColor;

        if (clickSound != null)
            AudioManager.Instance.PlayAudioSFX(clickSound);
    }
}