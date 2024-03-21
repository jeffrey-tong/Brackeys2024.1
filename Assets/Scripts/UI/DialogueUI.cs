using TMPro;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DialogueUI : MonoBehaviour
{
    private SpriteRenderer background;
    private TextMeshPro dialogueText;
    private PlayerLocomotion m_Locomotion;

    [SerializeField] private Transform leftPosition;
    [SerializeField] private Transform rightPosition;

    [Header("Properties")]
    [SerializeField] private float transitionTime = 0.2f;
    [SerializeField] private float normalTextSpeed = 0.03f;
    [SerializeField] private float fastTextSpeed = 0.01f;
    [SerializeField] private float fadeDuration = 0.2f;

    private Queue<DialogueTrigger> dialogueQueue = new Queue<DialogueTrigger>();

    private Action DialogueComplete;

    private bool isDialoguePlaying = false;
    private bool requestSkip = false;

    private void Awake()
    {
        background = GetComponentInChildren<SpriteRenderer>();
        dialogueText = GetComponentInChildren<TextMeshPro>();
        m_Locomotion = GetComponentInParent<PlayerLocomotion>();
        
        dialogueText.enabled = false;
        StartCoroutine(Fade(1f, 0f, 0f));
    }

    private void Start()
    { 
        DialogueTrigger.OnAnyDialogueTriggered += Dialogue_TriggeredCallback;
    }

    private void Update()
    {
        if (m_Locomotion)
        {
            if(m_Locomotion.GetPlayerFacing() < 0f)
            {
                background.flipX = false;
                transform.position = leftPosition.position;
            }
            else
            {
                background.flipX = true;
                transform.position = rightPosition.position;
            }
        }
    }

    private void Dialogue_TriggeredCallback(DialogueTrigger dialogue, Action OnDialogueComplete)
    {
        dialogueQueue.Enqueue(dialogue);
        if(!isDialoguePlaying)
        {
            StartCoroutine(HandleNextDialogue());
        }
    }

    private IEnumerator HandleNextDialogue()
    {
        while (dialogueQueue.Count > 0)
        {
            DialogueTrigger currentDialogueTrigger = dialogueQueue.Dequeue();

            dialogueText.SetText("");
            DialogueComplete = null;

            StartCoroutine(HandleAllDialogue(currentDialogueTrigger));

            yield return new WaitUntil(() => !isDialoguePlaying);

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator HandleAllDialogue(DialogueTrigger dialogue)
    {
        isDialoguePlaying = true;

        yield return new WaitForSecondsRealtime(dialogue.initialDelay);

        float timeElapsed = 0.0f;
        while (timeElapsed <= transitionTime)
        {
            timeElapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        isDialoguePlaying = true;
        Show();

        foreach (Dialogue d in dialogue.GetAllDialogues())
        {
            yield return HandleDialogue(d);
        }

        timeElapsed = 0.0f;
        while (timeElapsed <= transitionTime)
        {
            timeElapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        isDialoguePlaying = false;
        Hide();
        DialogueComplete?.Invoke();

        yield return new WaitWhile(() => isDialoguePlaying);
    }

    private IEnumerator HandleDialogue(Dialogue dialogue)
    {
        dialogueText.SetText("");

        for (int i = 0; i < dialogue.dialogueText.Length;)
        {
            if (requestSkip)
            {
                requestSkip = false;
                break;
            }

            char letter = dialogue.dialogueText[i];
            if (letter == '<')
            {
                int begin = dialogue.dialogueText.IndexOf("<");
                int end = dialogue.dialogueText.LastIndexOf(">");
                string tagText = dialogue.dialogueText.Substring(begin, end - begin + 1);
                dialogueText.SetText(dialogueText.text += tagText);
                i += (end - begin) + 1;
                continue;
            }

            dialogueText.SetText(dialogueText.text += letter);
            yield return new WaitForSecondsRealtime(normalTextSpeed);

            i++;
        }

        yield return new WaitForSecondsRealtime(dialogue.hangTime);
    }

    private void Show()
    {
        dialogueText.enabled = true;
        StartCoroutine(Fade(0f, 1f, fadeDuration));
    }

    private void Hide()
    {
        dialogueText.enabled = false;
        StartCoroutine(Fade(1f, 0f, fadeDuration));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float currentTime = 0f;
        Color startColor = background.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, endAlpha);

        while(currentTime < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, currentTime / duration);
            background.color = new Color(startColor.r, startColor.g, startColor.b, alpha);


            currentTime += Time.deltaTime;
            yield return null;
        }

        background.color = targetColor;
    }

    private void OnDestroy()
    {
        DialogueTrigger.OnAnyDialogueTriggered -= Dialogue_TriggeredCallback;
    }
}