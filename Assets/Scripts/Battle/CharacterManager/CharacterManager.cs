using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    // スクリプト
    [SerializeField] protected SelectCharacterData SelectCharacterData;

    [Header("キャラクター")]
    [SerializeField] protected CharacterDataBase CharacterDataBase;     // キャラクターのデータベースObject
    public Character SelectCharacter { get; protected set; }            // 選択されたキャラクター

    // キャラクターの画像
    protected Image CharacterImage;

    protected virtual void Awake() { }
}
