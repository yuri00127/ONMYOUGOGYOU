using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEController : VolumeController
{
    private AudioSource _SEAudio;

    private bool _isFirst = true;
    private const string _seManager = "SEManager";

    public override void Update()
    {
        base.Update();
    }

    public override void Submit()
    {
        // ‰‰ñİ’è‚Ì‚İASE‚ÌAudioSource‚ğæ“¾
        if (_isFirst)
        {
            _SEAudio = GameObject.Find(_seManager).GetComponent<AudioSource>();
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
        _SEAudio.volume += VolumeUnit;

        base.VolumeUp();
    }

    public override void VolumeDown()
    {
        // ‰¹—Ê‚ğ‰º‚°‚é
        _SEAudio.volume -= VolumeUnit;

        base.VolumeDown();
    }
}
