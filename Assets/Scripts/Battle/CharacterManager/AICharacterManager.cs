using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterManager : CharacterManager
{
    private int _aiLevel;

    protected override void Start()
    {
        // 選択された敵キャラクターを取得
        int aiCharacterId = PlayerPrefs.GetInt(SelectCharacterData.SaveAICharacterId);
        SelectCharacter = CharacterDataBase.CharacterList[aiCharacterId - 1];

        // 選択された敵の強さを取得
        _aiLevel = PlayerPrefs.GetInt(SelectCharacterData.SaveAILevel);
    }

    public void SetAICommand()
    {

        // 敵のコマンドを決定

        // 
    }
}
