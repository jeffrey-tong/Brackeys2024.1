using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPopup : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject popupWindow;

    [Header("Properties")]
    [SerializeField] private float yOffset = 2f;
    [SerializeField] private float scaleDuration = 0.3f;
    [SerializeField] private AnimationCurve scaleCurve;

    private void Start()
    {
        BaseTrigger.OnEntered += TriggerEntered_Callback;
        BaseTrigger.OnExit += TriggerExit_Callback;
    }

    private void TriggerEntered_Callback(BaseTrigger trigger)
    {
        Vector3 position = trigger.transform.position;
        position.y += yOffset;

        transform.position = position;

        StopAllCoroutines();
        StartCoroutine(HandleScale(true));
    }

    private IEnumerator HandleScale(bool enter)
    {
        Vector3 start = enter ? Vector3.zero : Vector3.one;
        Vector3 end = enter ? Vector3.one : Vector3.zero;

        float timeElapsed = 0.0f;
        popupWindow.transform.localScale = start;

        if (enter)
        {
            popupWindow.SetActive(true);
        }

        while(timeElapsed <= scaleDuration)
        {
            timeElapsed += Time.deltaTime;

            float t = timeElapsed / scaleDuration;
            t = scaleCurve.Evaluate(t);

            popupWindow.transform.localScale = Vector3.LerpUnclamped(start, end, t);
            yield return null;
        }

        if (!enter)
            popupWindow.SetActive(false);
    }

    private void TriggerExit_Callback(BaseTrigger trigger)
    {
        StopAllCoroutines();
        StartCoroutine(HandleScale(false));
    }

    private void OnDestroy()
    {
        BaseTrigger.OnEntered -= TriggerEntered_Callback;
        BaseTrigger.OnExit -= TriggerExit_Callback;
    }
}
