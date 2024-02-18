using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Background Color", menuName = "Custom/Background Color")]
public class BackgroundColor : ScriptableObject
{
    [System.Serializable]
    public class BackgroundColorData
    {
        public CharacterData characterData;
        public Color backgroundColor;
        public Color backgroundObjectColor;
    }

    [SerializeField] private CharacterData defaultColor;
    [SerializeField] private BackgroundColorData[] backgroundColors;
    public Dictionary<CharacterData, BackgroundColorData> backgroundLookup = new Dictionary<CharacterData, BackgroundColorData>();

    private void OnValidate()
    {
        backgroundLookup.Clear();
        foreach (BackgroundColorData data in backgroundColors)
        {
            backgroundLookup.Add(data.characterData, data);
        }
    }

    public BackgroundColorData GetColorData(CharacterData characterData)
    {
        if (backgroundLookup.ContainsKey(characterData))
        {
            return backgroundLookup[characterData];
        }

        return null;
    }

    public BackgroundColorData GetDefault()
    {
        return backgroundLookup[defaultColor];
    }
}