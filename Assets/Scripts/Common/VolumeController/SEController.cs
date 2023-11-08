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
        // ����ݒ莞�̂݁ASE��AudioSource���擾
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
        // ���ʂ��グ��
        _SEAudio.volume += VolumeUnit;

        base.VolumeUp();
    }

    public override void VolumeDown()
    {
        // ���ʂ�������
        _SEAudio.volume -= VolumeUnit;

        base.VolumeDown();
    }
}
