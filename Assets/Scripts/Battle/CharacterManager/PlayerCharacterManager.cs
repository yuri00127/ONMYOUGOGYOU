using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacterManager : CharacterManager
{
    // スクリプト
    [SerializeField] private PlayerCommandManager _playerCommandManager;

    // キャラクタービュー
    [SerializeField] private GameObject _playerCharacterViewObj;


    protected override void Awake()
    {
        // 選択された自機キャラクターを取得
        int playerCharacterId = PlayerPrefs.GetInt(SelectCharacterData.SavePlayerCharacterId);
        SelectCharacter = CharacterDataBase.CharacterList[playerCharacterId - 1];

        // キャラクター画像をセット
        CharacterImage = _playerCharacterViewObj.GetComponent<Image>();
        CharacterImage.sprite = SelectCharacter.CharacterImage;
    }
}
