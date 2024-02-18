using System;
using UnityEngine;

[Serializable]
public class Dialogue
{
    [SerializeField][TextArea(2, 4)] public string dialogueText;
    [SerializeField] public float hangTime = 2.0f;
}