using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private float fadeInDuration = 0.2f;
    private CharacterData current;

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
    }

    private void Door_EnteredCallback(CharacterData obj)
    {
        if (current != null && MusicLookup.TryGetValue(current, out AudioSource fadeOut))
        {
            StartCoroutine(FadeInOut(fadeOut, false));
        }

        current = obj;

        if (MusicLookup.TryGetValue(obj, out AudioSource fadeInSource))
        {
            StartCoroutine(FadeInOut(fadeInSource, true));
        }  
    }

    private IEnumerator FadeInOut(AudioSource source, bool fadeIn)
    {
        float start = fadeIn ? 0 : 1;
        float end = fadeIn ? 1 : 0;

        float timeElapsed = 0.0f;

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