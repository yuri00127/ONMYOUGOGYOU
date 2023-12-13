using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AICharacterManager : CharacterManager
{
    public int AILevel { get; private set; }
    

    


    protected override void Start()
    {
        // 選択された敵キャラクターを取得
        int aiCharacterId = 1;
        /*本番用
        int aiCharacterId = PlayerPrefs.GetInt(SelectCharacterData.SaveAICharacterId, 1);
        */
        SelectCharacter = CharacterDataBase.CharacterList[aiCharacterId - 1];


        // 選択された敵の強さを取得
        AILevel = 1;
        /*本番用
        _aiLevel = PlayerPrefs.GetInt(SelectCharacterData.SaveAILevel, 1);
        */

        
    }

    
}
