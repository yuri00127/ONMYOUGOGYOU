using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YinYangChangeButton : Button
{
    private const string _AnimName = "IsYin";
    private const string _inputName = "X";

    public bool IsYin { get; private set; } = true;

    public override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        // 入力
        if (Input.GetAxis(_inputName) > 0 && CanInput)
        {
            Submit();
        }

        // 一度入力をやめると再入力可能
        if (Input.GetAxisRaw(_inputName) == 0)
        {
            CanInput = true;
        }
    }

    public override void Submit()
    {
        CanInput = false;

        if (IsYin)
        {
            Anim.SetBool(_AnimName, false);
            IsYin = false;

            return;
        }

        Anim.SetBool(_AnimName, true);
        IsYin = true;
    }
}
