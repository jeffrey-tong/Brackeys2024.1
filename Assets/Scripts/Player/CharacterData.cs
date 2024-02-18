using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Character Data", menuName = "Custom/CharacterData")]
public class CharacterData : ScriptableObject
{
    public GameObject prefab;
    public Color color;
}

public enum CharacterColor
{
    GRAY,
    RED,
    YELLOW,
    BLUE,
}
