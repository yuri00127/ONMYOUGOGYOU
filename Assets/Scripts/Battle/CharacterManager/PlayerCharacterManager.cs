using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacterManager : CharacterManager
{
    // �X�N���v�g
    [SerializeField] private PlayerCommandManager _playerCommandManager;

    // �L�����N�^�[�r���[
    [SerializeField] private GameObject _playerCharacterViewObj;


    protected override void Awake()
    {
        // �I�����ꂽ���@�L�����N�^�[���擾
        int playerCharacterId = PlayerPrefs.GetInt(SelectCharacterData.SavePlayerCharacterId);
        SelectCharacter = CharacterDataBase.CharacterList[playerCharacterId - 1];

        // �L�����N�^�[�摜���Z�b�g
        CharacterImage = _playerCharacterViewObj.GetComponent<Image>();
        CharacterImage.sprite = SelectCharacter.CharacterImage;
    }
}
