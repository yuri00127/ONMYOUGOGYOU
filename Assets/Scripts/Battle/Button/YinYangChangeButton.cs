using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YinYangChangeButton : Button
{
    private const string _AnimName = "IsYin";

    public bool IsYin { get; private set; } = true;

    public override void Start()
    {
        base.Start();
    }

    public override void Submit()
    {
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
