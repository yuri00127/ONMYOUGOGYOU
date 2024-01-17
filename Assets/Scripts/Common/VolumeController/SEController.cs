using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SEController : VolumeController
{
    private const string _seManager = "SEManager";


    protected override void Awake()
    {
        Audio = GameObject.Find(_seManager).GetComponent<AudioSource>();
        SliderName = SESliderName;

        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }
}
