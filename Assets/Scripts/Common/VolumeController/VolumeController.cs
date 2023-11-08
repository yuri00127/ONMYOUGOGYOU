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
        // �X���C�_�[��I��
        if (_isSelect)
        {
            EventSystem.current.SetSelectedGameObject(_sliderObj);

            // �E����
            if (Input.GetAxisRaw(_inputVertical) > 0) { VolumeUp(); }

            // ������
            if (Input.GetAxisRaw(_inputVertical) < 0) { VolumeDown(); }
        }
    }

    // �X���C�_�[��Submit
    public virtual void Submit()
    {
        // �X���C�_�[���擾
        _sliderObj = EventSystem.current.currentSelectedGameObject;
        _volumeSlider = _sliderObj.GetComponent<Slider>();

        _isSelect = true;
    }

    // �X���C�_�[��Cancel
    public virtual void Cancel()
    {
        _isSelect = false;
    }

    // ���ʂ�1�i�K�グ��
    public virtual void VolumeUp()
    {
        _volumeSlider.value += VolumeUnit;
    }

    // ���ʂ�1�i�K������
    public virtual void VolumeDown()
    {
        _volumeSlider.value -= VolumeUnit;
    }

}
