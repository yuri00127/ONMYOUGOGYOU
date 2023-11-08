using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : VolumeController
{
    private AudioSource _BGMAudio;

    private bool _isFirst = true;
    private const string _bgmManager = "BGMManager";


    public override void Submit()
    {
        // ‰‰ñİ’è‚Ì‚İABGM‚ÌAudioSource‚ğæ“¾
        if (_isFirst)
        {
            _BGMAudio = GameObject.Find(_bgmManager).GetComponent<AudioSource>();
        }

        base.Submit();

        _isFirst = false;
    }

    public override void Cancel()
    {
        base.Cancel();
    }

    public override void VolumeUp()
    {
        // ‰¹—Ê‚ğã‚°‚é
        _BGMAudio.volume += VolumeUnit;

        base.VolumeUp();
    }

    public override void VolumeDown()
    {
        // ‰¹—Ê‚ğ‰º‚°‚é
        _BGMAudio.volume -= VolumeUnit;

        base.VolumeDown();
    }
}
