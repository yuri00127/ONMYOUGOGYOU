using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// キャラクターのデータ構造
[CreateAssetMenu]
[SerializeField]
public class Character : ScriptableObject
{
    public int Id;                                          // ID
    public string Name;                                     // 名前
    public int AttributeId;                                 // 属性のID
    public Sprite CharacterImage;                           // 画像
    public Sprite[] CommandSprites = new Sprite[5];         // コマンドの画像
    public Sprite[] SelectCommandSprites = new Sprite[5];   // 選択状態のコマンドの画像
}
