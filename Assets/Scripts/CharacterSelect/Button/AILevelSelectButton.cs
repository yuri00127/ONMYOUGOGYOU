using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILevelSelectButton : Button
{
    [SerializeField] private SelectStepManager _selectStepManager;

    public void Submit(GameObject gameObject)
    {
        _selectStepManager.AILevelSelect(gameObject);
    }

}
