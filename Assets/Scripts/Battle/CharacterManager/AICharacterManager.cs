using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AICharacterManager : CharacterManager
{
    // �X�N���v�g���擾����I�u�W�F�N�g
    private const string _aiCommandManagerObjName = "AICommandManager";

    // �X�N���v�g
    private AICommandManager _aiCommandManager;

    public int AILevel { get; private set; }
    

    protected override void Awake()
    {
        // �X�N���v�g���擾
        _aiCommandManager = GameObject.Find(_aiCommandManagerObjName).GetComponent<AICommandManager>();

        base.Awake();

        // �I�����ꂽ�G�L�����N�^�[���擾
        int aiCharacterId = 1;
        SelectCharacter = CharacterDataBase.CharacterList[aiCharacterId - 1];
        /*�{�ԗp
        int aiCharacterId = PlayerPrefs.GetInt(SelectCharacterData.SaveAICharacterId, 1);
        */

        // �I�����ꂽ�G�̋������擾
        AILevel = 1;
        /*�{�ԗp
        _aiLevel = PlayerPrefs.GetInt(SelectCharacterData.SaveAILevel, 1);
        */

    }

}
