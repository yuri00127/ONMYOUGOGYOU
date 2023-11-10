using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectButton : Button
{
    [SerializeField] private SelectStepManager _selectStepManager;

    public void Submit(GameObject gameObject)
    {
        // プレイヤーキャラクターの選択
        if (_selectStepManager.NowSelectStep == 0)
        {
            _selectStepManager.PlayerCharacterSelect(gameObject);

            return;
        }

        // AIキャラクターの選択
        if (_selectStepManager.NowSelectStep == 1)
        {
            _selectStepManager.AICharacterSelect(gameObject);

            return;
        }

    }

}
