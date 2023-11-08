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
        // ����ݒ莞�̂݁ABGM��AudioSource���擾
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
        // ���ʂ��グ��
        _BGMAudio.volume += VolumeUnit;

        base.VolumeUp();
    }

    public override void VolumeDown()
    {
        // ���ʂ�������
        _BGMAudio.volume -= VolumeUnit;

        base.VolumeDown();
    }
}
