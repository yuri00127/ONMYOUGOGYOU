using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterSelect : Button
{
    [SerializeField] private CharacterDataBase _characterDataBase;
    [SerializeField] private SelectStepManager _selectStepManager;

    private string _characterIdFormat = "Character{0}";
    private int _selectCharacterId;


    public override void PointerEnter(GameObject gameObject)
    {
        if (_selectStepManager.NowSelectStep == 2)
        {
            EventSystem.current.SetSelectedGameObject(null);
            return;
        }

        base.PointerEnter(gameObject);
    }

    public void Submit(GameObject gameObject)
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

        // プレイヤーキャラクターの選択
        if (_selectStepManager.NowSelectStep == 0)
        {
            Debug.Log("PLchara" + _selectCharacterId);
            _selectStepManager.NextAICharacterSelect(_selectCharacterId);

            return;
        }

        // AIキャラクターの選択
        if (_selectStepManager.NowSelectStep == 1)
        {
            Debug.Log("AIchara" + _selectCharacterId);
            _selectStepManager.NextAILevelSelect(_selectCharacterId);

            return;
        }

    }

}
