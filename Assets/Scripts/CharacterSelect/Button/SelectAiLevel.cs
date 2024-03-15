using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectAiLevel : Button
{
    [SerializeField] private SelectStepManager _selectStepManager;

    private string _AILevelFormat = "Level{0}";
    private int _selectAILevel;
    private const int _aiLevelThree = 3;
    private const int _aiLevelOne = 1;

    [Header("AiLevelObj")]
    [SerializeField] private GameObject _level2Obj;
    [SerializeField] private Sprite _levelSprite;
    [SerializeField] private Sprite _nullSprite;


    public override void PointerEnter(GameObject gameObject)
    {
        base.PointerEnter(gameObject);

        if (gameObject.name == string.Format(_AILevelFormat, _aiLevelThree))
        {
            _level2Obj.GetComponent<Image>().sprite = _levelSprite;
        }

        if (gameObject.name == string.Format(_AILevelFormat, _aiLevelOne))
        {
            _level2Obj.GetComponent<Image>().sprite = _nullSprite;
        }
    }

    /// <summary>
    /// AIの強さのレベルを取得
    /// </summary>
    /// <param name="gameObject">レベルオブジェクト</param>
    public void Submit(GameObject gameObject)
    {
        // 選択されたレベルを取得
        for (var i = 1; i <= 3; i++)
        {
            if (gameObject.name == string.Format(_AILevelFormat, i))
            {
                _selectAILevel = i;
                break;
            }
        }

        _selectStepManager.NextBattleScene(_selectAILevel);
    }
}
