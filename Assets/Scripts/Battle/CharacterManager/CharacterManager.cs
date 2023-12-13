using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    // スクリプトを取得するオブジェクト
    private const string _selectCharacterData = "SelectCharacterData";

    // スクリプト
    protected SelectCharacterData SelectCharacterData;

    [Header("キャラクター")]
    [SerializeField] protected CharacterDataBase CharacterDataBase;     // キャラクターのデータベースObject
    public Character SelectCharacter { get; protected set; }              // 選択されたキャラクター


    protected virtual void Awake()
    {
        // スクリプトの取得
        SelectCharacterData = GameObject.Find(_selectCharacterData).GetComponent<SelectCharacterData>();
    }

    protected virtual void Start()
    {
        
    }
}
