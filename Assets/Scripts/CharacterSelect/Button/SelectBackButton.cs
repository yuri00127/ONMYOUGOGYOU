using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �L�����N�^�[�I����ʂł̑I���X�e�b�v��O�̒i�K�ɖ߂�
public class SelectBackButton : Button
{
    [SerializeField] private SelectStepManager _selectStepManager;

    private const string _cancelButton = "Cancel";

    private void Update()
    {
        // ����
        if (Input.GetAxisRaw(_cancelButton) > 0�@&& CanInput)
        {
            _selectStepManager.BackStep();
        }

        // ��x���͂���߂�ƍē��͉\�ɂ���
        if (Input.GetAxisRaw(_cancelButton) == 0)
        {
            CanInput = true;
        }
    }

    
}
