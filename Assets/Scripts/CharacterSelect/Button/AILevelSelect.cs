using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AILevelSelect : Button
{
    [SerializeField] private SelectStepManager _selectStepManager;

    private string _AILevelFormat = "Level{0}";
    private int _selectAILevel;


    public override void PointerEnter(GameObject gameObject)
    {
        base.PointerEnter(gameObject);
    }

    public void Submit(GameObject gameObject)
    {
        // �I�����ꂽAI�̃��x�����擾
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
