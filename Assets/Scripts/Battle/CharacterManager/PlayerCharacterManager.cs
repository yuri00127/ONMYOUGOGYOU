using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterManager : CharacterManager
{
    [Header("スクリプト")]
    [SerializeField] CommandManager _commandManager;

    private void Awake()
    {
        // 選択された自機キャラクターを取得
        int playerCharacterId = PlayerPrefs.GetInt(SelectCharacterData.SavePlayerCharacterId);
        SelectCharacter = CharacterDataBase.CharacterList[playerCharacterId - 1];

        _commandManager.SetCommand(SelectCharacter);
    }
}
