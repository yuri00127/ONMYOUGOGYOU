using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AICharacterManager : CharacterManager
{
    // スクリプトを取得するオブジェクト
    private const string _aiCommandManagerObjName = "AICommandManager";

    // スクリプト
    private AICommandManager _aiCommandManager;

    public int AILevel { get; private set; }
    

    protected override void Awake()
    {
        // スクリプトを取得
        _aiCommandManager = GameObject.Find(_aiCommandManagerObjName).GetComponent<AICommandManager>();

        base.Awake();

        // 選択された敵キャラクターを取得
        int aiCharacterId = 1;
        SelectCharacter = CharacterDataBase.CharacterList[aiCharacterId - 1];
        /*本番用
        int aiCharacterId = PlayerPrefs.GetInt(SelectCharacterData.SaveAICharacterId, 1);
        */

        // 選択された敵の強さを取得
        AILevel = 1;
        /*本番用
        _aiLevel = PlayerPrefs.GetInt(SelectCharacterData.SaveAILevel, 1);
        */

    }

}
