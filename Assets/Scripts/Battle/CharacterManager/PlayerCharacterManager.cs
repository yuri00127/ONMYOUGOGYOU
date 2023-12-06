using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterManager : CharacterManager
{
    [Header("�X�N���v�g")]
    [SerializeField] CommandManager _commandManager;

    private void Awake()
    {
        // �I�����ꂽ���@�L�����N�^�[���擾
        int playerCharacterId = PlayerPrefs.GetInt(SelectCharacterData.SavePlayerCharacterId);
        SelectCharacter = CharacterDataBase.CharacterList[playerCharacterId - 1];

        _commandManager.SetCommand(SelectCharacter);
    }
}
