using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AICharacterManager : CharacterManager
{
    public int AILevel { get; private set; }
    

    


    protected override void Start()
    {
        // �I�����ꂽ�G�L�����N�^�[���擾
        int aiCharacterId = 1;
        /*�{�ԗp
        int aiCharacterId = PlayerPrefs.GetInt(SelectCharacterData.SaveAICharacterId, 1);
        */
        SelectCharacter = CharacterDataBase.CharacterList[aiCharacterId - 1];


        // �I�����ꂽ�G�̋������擾
        AILevel = 1;
        /*�{�ԗp
        _aiLevel = PlayerPrefs.GetInt(SelectCharacterData.SaveAILevel, 1);
        */

        
    }

    
}
