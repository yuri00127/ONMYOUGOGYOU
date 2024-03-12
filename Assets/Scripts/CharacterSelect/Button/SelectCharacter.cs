using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacter : Button
{
    [SerializeField] private CharacterDataBase _characterDataBase;
    [SerializeField] private SelectStepManager _selectStepManager;

    private string _characterIdFormat = "Character{0}";
    private int _selectCharacterId;

    // キャラクタービュー
    private const string _playerCharacterViewObjName = "PlayerCharacterBox";
    private Image _playerCharacterViewImage;
    private const string _aiCharacterViewObjName = "AICharacterBox";
    private Image _aiCharacterViewImage;
    [SerializeField] private Sprite _defaultCharacterSprite;


    private void Awake()
    {
        _playerCharacterViewImage = GameObject.Find(_playerCharacterViewObjName).GetComponent<Image>();
        _aiCharacterViewImage = GameObject.Find(_aiCharacterViewObjName).GetComponent<Image>();
    }

    public override void PointerEnter(GameObject gameObject)
    {
        if (_selectStepManager.NowSelectStep != 2)
        {
            base.PointerEnter(gameObject);
        }

        // プレイヤーキャラクター選択時
        if (_selectStepManager.NowSelectStep == 0)
        {
            _playerCharacterViewImage.sprite = gameObject.GetComponent<Image>().sprite;
            return;
        }

        // AIキャラクター選択時
        if (_selectStepManager.NowSelectStep == 1)
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
        if (_selectStepManager.NowSelectStep != 2)
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
        if (_selectStepManager.NowSelectStep == 0)
        {
            _selectStepManager.NextAICharacterSelect(_selectCharacterId);
            _playerCharacterViewImage.sprite = gameObject.GetComponent<Image>().sprite;
            _aiCharacterViewImage.sprite = _defaultCharacterSprite;
            return;
        }

        // AIキャラクターの選択
        if (_selectStepManager.NowSelectStep == 1)
        {
            _selectStepManager.NextAILevelSelect(_selectCharacterId);
            _aiCharacterViewImage.sprite = gameObject.GetComponent<Image>().sprite;
            return;
        }
    }
}
