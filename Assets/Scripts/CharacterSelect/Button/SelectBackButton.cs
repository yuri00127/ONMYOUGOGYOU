using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// キャラクター選択画面での選択ステップを前の段階に戻す
public class SelectBackButton : Button
{
    [SerializeField] private SelectStepManager _selectStepManager;

    private const string _cancelButton = "Cancel";

    private void Update()
    {
        // 入力
        if (Input.GetAxisRaw(_cancelButton) > 0　&& CanInput)
        {
            _selectStepManager.BackStep();
        }

        // 一度入力をやめると再入力可能にする
        if (Input.GetAxisRaw(_cancelButton) == 0)
        {
            CanInput = true;
        }
    }

    
}
