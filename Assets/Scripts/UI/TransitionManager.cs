using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeImg;
    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private float pauseDuration = 0.1f;
    [SerializeField] private AnimationCurve fadeCurve;

    public static TransitionManager Instance { get; private set; }
    public event Action OnTransitionStarted;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartCoroutine(FadeOut(null));
    }

    private void OnLevelWasLoaded(int level)
    {
        StartCoroutine(FadeOut(null));
    }

    public void DoFadeIn(Action OnFadeInCompleted)
    {
        StartCoroutine(FadeIn(OnFadeInCompleted));
    }

    public void DoFadeOut(Action OnFadeOutCompleted)
    {
        StartCoroutine(FadeOut(OnFadeOutCompleted));
    }

    public void DoFadeInOut(Action OnFadeInCompleted, Action OnFadeOutCompleted)
    {
        StartCoroutine(FadeInOut(OnFadeInCompleted, OnFadeOutCompleted));
    }

    public void LoadScene(string sceneName)
    {
        OnTransitionStarted?.Invoke();

        Action fadeInAction = () => SceneManager.LoadScene(sceneName);
        StartCoroutine(FadeIn(fadeInAction));
    }

    public void LoadScene(int sceneId)
    {
        OnTransitionStarted?.Invoke();

        Action fadeInAction = () => SceneManager.LoadScene(sceneId);
        StartCoroutine(FadeIn(fadeInAction));
    }

    private IEnumerator FadeIn(Action OnTransitionFinished)
    {
        fadeImg.blocksRaycasts = true;

        float start = 0;
        float end = 1;
        float timeElapsed = 0.0f;

        fadeImg.alpha = start;

        while (timeElapsed <= fadeDuration)
        {
            timeElapsed += Time.unscaledDeltaTime;
            float alpha = fadeCurve.Evaluate(timeElapsed / fadeDuration);
            fadeImg.alpha = Mathf.Lerp(start, end, alpha);

            yield return null;
        }

        OnTransitionFinished?.Invoke();
    }

    private IEnumerator FadeOut(Action OnTransitionFinished)
    {
        fadeImg.blocksRaycasts = true;

        float start = 1;
        float end = 0;
        float timeElapsed = 0.0f;

        fadeImg.alpha = start;

        while (timeElapsed <= fadeDuration)
        {
            timeElapsed += Time.unscaledDeltaTime;
            float alpha = fadeCurve.Evaluate(timeElapsed / fadeDuration);
            fadeImg.alpha = Mathf.Lerp(start, end, alpha);

            yield return null;
        }

        OnTransitionFinished?.Invoke();

        fadeImg.blocksRaycasts = false;
    }

    private IEnumerator FadeInOut(Action OnFadeInFinished, Action OnFadeOutFinished)
    {
        yield return FadeIn(OnFadeInFinished);
        yield return new WaitForSecondsRealtime(pauseDuration);
        yield return FadeOut(OnFadeOutFinished);
    }

    public float GetFadeDuration()
    {
        return fadeDuration;
    }
}
