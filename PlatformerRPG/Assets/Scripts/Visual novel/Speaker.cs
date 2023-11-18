using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpeaker", menuName = "Data/Visual Novel/New Speaker")]
[System.Serializable]
public class Speaker : ScriptableObject
{
    public string speakerName;
    public Color textColor;

    public List<Sprite> sprites;
    public SpriteController prefab;
}
