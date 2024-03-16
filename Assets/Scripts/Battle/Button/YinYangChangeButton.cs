using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YinYangChangeButton : Button
{
    private const string _animName = "IsYin";
    private string _inputName;

    public bool IsYin { get; private set; } = true;

    public override void Start()
    {
        base.Start();
        _inputName = InputTypeManager.InputType.X.ToString();
    }

    private void Update()
    {
        // ����
        if (Input.GetAxisRaw(_inputName) > 0 && CanInput)
        {
            Submit();
        }

        // ��x���͂���߂�ƍē��͉\
        if (Input.GetAxisRaw(_inputName) == 0)
        {
            CanInput = true;
        }
    }

    public override void Submit()
    {
        CanInput = false;
        Audio.PlayOneShot(SubmitSE);

        if (IsYin)
        {
            Anim.SetBool(_animName, false);
            IsYin = false;

            return;
        }

        Anim.SetBool(_animName, true);
        IsYin = true;
    }
}
