using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacterManager : CharacterManager
{
    // スクリプトを取得するオブジェクト
    private const string _playerCommandManagerObjName = "PlayerCommandManager";

    // スクリプト
    private PlayerCommandManager _playerCommandManager;

    // キャラクタービュー
    private const string _playerCharacterViewObjName = "PlayerCharacter";


    protected override void Awake()
    {
        base.Awake();

        // 選択された自機キャラクターを取得
        int playerCharacterId = PlayerPrefs.GetInt(SelectCharacterData.SavePlayerCharacterId);
        SelectCharacter = CharacterDataBase.CharacterList[playerCharacterId - 1];

        // キャラクター画像をセット
        CharacterImage = GameObject.Find(_playerCharacterViewObjName).GetComponent<Image>();
        CharacterImage.sprite = SelectCharacter.CharacterImage;

        // スクリプトを取得
        _playerCommandManager = GameObject.Find(_playerCommandManagerObjName).GetComponent<PlayerCommandManager>();
    }
}
