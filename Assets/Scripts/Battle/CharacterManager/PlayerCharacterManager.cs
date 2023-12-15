using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterManager : CharacterManager
{
    // スクリプトを取得するオブジェクト
    private const string _playerCommandManagerObjName = "PlayerCommandManager";

    // スクリプト
    private PlayerCommandManager _playerCommandManager;


    protected override void Awake()
    {
        base.Awake();

        // 選択された自機キャラクターを取得
        int playerCharacterId = PlayerPrefs.GetInt(SelectCharacterData.SavePlayerCharacterId);
        SelectCharacter = CharacterDataBase.CharacterList[playerCharacterId - 1];

        // スクリプトを取得
        _playerCommandManager = GameObject.Find(_playerCommandManagerObjName).GetComponent<PlayerCommandManager>();
    }
}
