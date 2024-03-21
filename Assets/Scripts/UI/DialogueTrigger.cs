using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private Dialogue[] dialogues;

    public static event Action<DialogueTrigger, Action> OnAnyDialogueTriggered;
    public float initialDelay = 0.5f;
    private bool hasBeenTriggered = false;

    public void StartDialogue(Action OnDialogueCompleted)
    {
        if (hasBeenTriggered) return;

        hasBeenTriggered = true;
        OnAnyDialogueTriggered?.Invoke(this, OnDialogueCompleted);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartDialogue(DialogueComplete);
    }

    private void DialogueComplete()
    {

    }

    public IEnumerable<Dialogue> GetAllDialogues()
    {
        return dialogues;
    }
}