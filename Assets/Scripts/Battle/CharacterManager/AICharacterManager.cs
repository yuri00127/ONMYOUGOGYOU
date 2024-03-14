using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AICharacterManager : CharacterManager
{
    // �X�N���v�g
    [SerializeField] private AICommandManager _aiCommandManager;

    // �L�����N�^�[�r���[
    [SerializeField] private GameObject _aiCharacterView;


    public int AILevel { get; private set; }
    

    protected override void Awake()
    {
        // �I�����ꂽ�G�L�����N�^�[���擾
        int aiCharacterId = PlayerPrefs.GetInt(SelectCharacterData.SaveAICharacterId, 1);
        SelectCharacter = CharacterDataBase.CharacterList[aiCharacterId - 1];

        // �I�����ꂽ�G�̋������擾
        AILevel = PlayerPrefs.GetInt(SelectCharacterData.SaveAILevel, 1);

        // �L�����N�^�[�摜���Z�b�g
        CharacterImage = _aiCharacterView.GetComponent<Image>();
        CharacterImage.sprite = SelectCharacter.CharacterImage;
    }

}
