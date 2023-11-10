using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectButton : Button
{
    [SerializeField] private SelectStepManager _selectStepManager;

    public void Submit(GameObject gameObject)
    {
        // �v���C���[�L�����N�^�[�̑I��
        if (_selectStepManager.NowSelectStep == 0)
        {
            _selectStepManager.PlayerCharacterSelect(gameObject);

            return;
        }

        // AI�L�����N�^�[�̑I��
        if (_selectStepManager.NowSelectStep == 1)
        {
            _selectStepManager.AICharacterSelect(gameObject);

            return;
        }

    }

}
