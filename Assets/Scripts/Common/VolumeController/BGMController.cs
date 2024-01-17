using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BGMController : VolumeController
{
    private const string _bgmManager = "BGMManager";

    protected override void Awake()
    {
        Audio = GameObject.Find(_bgmManager).GetComponent<AudioSource>();
        SliderName = BGMSliderName;

        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }
}
