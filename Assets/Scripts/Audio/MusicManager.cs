using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private float fadeInDuration = 0.2f;
    [SerializeField] private CharacterData current;

    [SerializeField] private CharacterMusic[] characterMusics;
    private Dictionary<CharacterData, AudioSource> MusicLookup;

    [System.Serializable]
    public class CharacterMusic
    {
        public CharacterData characterDataSO;
        public AudioSource musicToPlay;
    }

    private void Start()
    {
        MusicLookup = new Dictionary<CharacterData, AudioSource>();
        foreach(CharacterMusic music in characterMusics)
        {
            MusicLookup.Add(music.characterDataSO, music.musicToPlay);
        }

        Door.OnAnyDoorEntered += Door_EnteredCallback;

        if (current != null && MusicLookup.TryGetValue(current, out AudioSource startSouce))
        {
            StartCoroutine(FadeIn(startSouce));
        }
    }

    private void Door_EnteredCallback(CharacterData obj)
    {
        if (current != null && MusicLookup.TryGetValue(current, out AudioSource fadeOutSource))
        {
            StartCoroutine(FadeOut(fadeOutSource));
        }

        current = obj;

        if (MusicLookup.TryGetValue(obj, out AudioSource fadeInSource))
        {
            StartCoroutine(FadeIn(fadeInSource));
        }  
    }

    private IEnumerator FadeIn(AudioSource source)
    {
        float start = 0;
        float end = 1;
        float timeElapsed = 0.0f;

        source.volume = start;

        while (timeElapsed <= fadeInDuration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / fadeInDuration;
            source.volume = Mathf.Lerp(start, end, t);

            yield return null;
        }
    }

    private IEnumerator FadeOut(AudioSource source)
    {
        float start = 1;
        float end = 0;
        float timeElapsed = 0;

        source.volume = start;

        while (timeElapsed <= fadeInDuration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / fadeInDuration;
            source.volume = Mathf.Lerp(start, end, t);

            yield return null;
        }
    }

    private void OnDestroy()
    {
        Door.OnAnyDoorEntered -= Door_EnteredCallback;
    }
}
