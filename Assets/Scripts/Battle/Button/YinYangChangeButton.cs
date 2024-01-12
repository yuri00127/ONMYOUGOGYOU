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
        // “ü—Í
        if (Input.GetAxis(_inputName) > 0 && CanInput)
        {
            Submit();
        }

        // ˆê“x“ü—Í‚ğ‚â‚ß‚é‚ÆÄ“ü—Í‰Â”\
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
            Anim.SetBool(_AnimName, false);
            IsYin = false;

            return;
        }

        Anim.SetBool(_AnimName, true);
        IsYin = true;
    }
}
