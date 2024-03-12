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

    [Header("AiLevelObj")]
    [SerializeField] private GameObject _level2Obj;
    [SerializeField] private Sprite _levelSprite;
    [SerializeField] private Sprite _nullSprite;


    public override void PointerEnter(GameObject gameObject)
    {
        base.PointerEnter(gameObject);

        if (gameObject.name == string.Format(_AILevelFormat, 3))
        {
            _level2Obj.GetComponent<Image>().sprite = _levelSprite;
            Debug.Log("3");
        }

        if (gameObject.name == string.Format(_AILevelFormat, 1))
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
        for (int i = 1; i <= 3; i++)
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
