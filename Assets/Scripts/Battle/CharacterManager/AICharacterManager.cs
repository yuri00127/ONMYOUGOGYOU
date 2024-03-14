using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AICharacterManager : CharacterManager
{
    // スクリプト
    [SerializeField] private AICommandManager _aiCommandManager;

    // キャラクタービュー
    [SerializeField] private GameObject _aiCharacterView;


    public int AILevel { get; private set; }
    

    protected override void Awake()
    {
        // 選択された敵キャラクターを取得
        int aiCharacterId = PlayerPrefs.GetInt(SelectCharacterData.SaveAICharacterId, 1);
        SelectCharacter = CharacterDataBase.CharacterList[aiCharacterId - 1];

        // 選択された敵の強さを取得
        AILevel = PlayerPrefs.GetInt(SelectCharacterData.SaveAILevel, 1);

        // キャラクター画像をセット
        CharacterImage = _aiCharacterView.GetComponent<Image>();
        CharacterImage.sprite = SelectCharacter.CharacterImage;
    }

}
