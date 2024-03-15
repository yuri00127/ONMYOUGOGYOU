using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacter : Button
{
    // スクリプト
    [SerializeField] private CharacterDataBase _characterDataBase;
    [SerializeField] private SelectStepManager _selectStepManager;

    private string _characterIdFormat = "Character{0}";
    private int _selectCharacterId;

    // キャラクタービュー
    [SerializeField] private Image _playerCharacterViewImage;
    [SerializeField] private Image _aiCharacterViewImage;
    [SerializeField] private Sprite _defaultCharacterSprite;

    private const int _aiLevelSelectStep = 2;
    private const int _playerCharacterSelectStep = 0;
    private const int _aiCharacterSelectStep = 1;


    public override void PointerEnter(GameObject gameObject)
    {
        if (_selectStepManager.NowSelectStep != _aiLevelSelectStep)
        {
            base.PointerEnter(gameObject);
        }

        // プレイヤーキャラクター選択時
        if (_selectStepManager.NowSelectStep == _playerCharacterSelectStep)
        {
            _playerCharacterViewImage.sprite = gameObject.GetComponent<Image>().sprite;
            return;
        }

        // AIキャラクター選択時
        if (_selectStepManager.NowSelectStep == _aiCharacterSelectStep)
        {
            _aiCharacterViewImage.sprite = gameObject.GetComponent<Image>().sprite;
            return;
        }
    }

    /// <summary>
    /// キャラクターを選択
    /// </summary>
    /// <param name="gameObject">キャラクターのオブジェクト</param>
    public void Submit(GameObject gameObject)
    {
        if (_selectStepManager.NowSelectStep != _aiLevelSelectStep)
        {
            // 選択されたキャラクターのIDを取得
            for (int i = 1; i <= _characterDataBase.CharacterList.Count; i++)
            {
                if (gameObject.name == string.Format(_characterIdFormat, i))
                {
                    _selectCharacterId = i;
                    break;
                }
            }
        }

        // プレイヤーキャラクターの選択
        if (_selectStepManager.NowSelectStep == _playerCharacterSelectStep)
        {
            _selectStepManager.NextAICharacterSelect(_selectCharacterId);
            _playerCharacterViewImage.sprite = gameObject.GetComponent<Image>().sprite;
            _aiCharacterViewImage.sprite = _defaultCharacterSprite;
            return;
        }

        // AIキャラクターの選択
        if (_selectStepManager.NowSelectStep == _aiCharacterSelectStep)
        {
            _selectStepManager.NextAILevelSelect(_selectCharacterId);
            _aiCharacterViewImage.sprite = gameObject.GetComponent<Image>().sprite;
            return;
        }
    }
}
