using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    private bool _isSelect = false;

    private GameObject _sliderObj;
    private Slider _volumeSlider;
    public const float VolumeUnit = 0.1f;

    private const string _inputVertical = "Vertical";


    public virtual void Update()
    {
        // スライダーを選択中
        if (_isSelect)
        {
            EventSystem.current.SetSelectedGameObject(_sliderObj);

            // 右入力
            if (Input.GetAxisRaw(_inputVertical) > 0) { VolumeUp(); }

            // 左入力
            if (Input.GetAxisRaw(_inputVertical) < 0) { VolumeDown(); }
        }
    }

    // スライダーをSubmit
    public virtual void Submit()
    {
        // スライダーを取得
        _sliderObj = EventSystem.current.currentSelectedGameObject;
        _volumeSlider = _sliderObj.GetComponent<Slider>();

        _isSelect = true;
    }

    // スライダーをCancel
    public virtual void Cancel()
    {
        _isSelect = false;
    }

    // 音量を1段階上げる
    public virtual void VolumeUp()
    {
        _volumeSlider.value += VolumeUnit;
    }

    // 音量を1段階下げる
    public virtual void VolumeDown()
    {
        _volumeSlider.value -= VolumeUnit;
    }

}
