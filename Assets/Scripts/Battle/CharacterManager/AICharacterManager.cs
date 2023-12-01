using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterManager : CharacterManager
{
    private int _aiLevel;

    protected override void Start()
    {
        // �I�����ꂽ�G�L�����N�^�[���擾
        int aiCharacterId = PlayerPrefs.GetInt(SelectCharacterData.SaveAICharacterId);
        SelectCharacter = CharacterDataBase.CharacterList[aiCharacterId - 1];

        // �I�����ꂽ�G�̋������擾
        _aiLevel = PlayerPrefs.GetInt(SelectCharacterData.SaveAILevel);
    }

    public void SetAICommand()
    {

        // �G�̃R�}���h������

        // 
    }
}
