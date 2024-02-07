using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacterManager : CharacterManager
{
    // �X�N���v�g���擾����I�u�W�F�N�g
    private const string _playerCommandManagerObjName = "PlayerCommandManager";

    // �X�N���v�g
    private PlayerCommandManager _playerCommandManager;

    // �L�����N�^�[�r���[
    private const string _playerCharacterViewObjName = "PlayerCharacter";


    protected override void Awake()
    {
        base.Awake();

        // �I�����ꂽ���@�L�����N�^�[���擾
        int playerCharacterId = PlayerPrefs.GetInt(SelectCharacterData.SavePlayerCharacterId);
        SelectCharacter = CharacterDataBase.CharacterList[playerCharacterId - 1];

        // �L�����N�^�[�摜���Z�b�g
        CharacterImage = GameObject.Find(_playerCharacterViewObjName).GetComponent<Image>();
        CharacterImage.sprite = SelectCharacter.CharacterImage;

        // �X�N���v�g���擾
        _playerCommandManager = GameObject.Find(_playerCommandManagerObjName).GetComponent<PlayerCommandManager>();
    }
}
