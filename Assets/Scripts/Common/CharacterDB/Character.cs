using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// キャラクターのデータ構造
[CreateAssetMenu]
[SerializeField]
public class Character : ScriptableObject
{
    public int Id;
    public string Name;
    public int AttributeId;
    public Sprite CharacterImage;
    public Sprite[] CommandSprites = new Sprite[5];
    public Sprite[] SelectCommandSprites = new Sprite[5];
}
