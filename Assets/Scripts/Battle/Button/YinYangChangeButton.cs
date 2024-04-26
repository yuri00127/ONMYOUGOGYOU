using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YinYangChangeButton : Button
{
    private const string _boolName = "IsYin";
    private string _inputName;
    private const string _focusBoolName = "Focus";
    private Animator _iconAnim;

    public bool IsYin { get; private set; } = true;

    public override void Start()
    {
        base.Start();
        _inputName = InputTypeManager.InputType.X.ToString();
        _iconAnim = this.transform.GetChild(0).GetComponent<Animator>();
    }

    private void Update()
    {
        // “ü—Í
        if (Input.GetAxisRaw(_inputName) > 0 && CanInput)
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
            Anim.SetBool(_boolName, false);
            IsYin = false;

            return;
        }

        Anim.SetBool(_boolName, true);
        IsYin = true;
    }

    public override void Select()
    {
        _iconAnim.SetBool(_focusBoolName, true);
    }

    public override void Deselect()
    {
        _iconAnim.SetBool(_focusBoolName, false);
    }
}
