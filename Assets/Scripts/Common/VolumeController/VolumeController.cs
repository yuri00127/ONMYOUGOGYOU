using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    protected AudioSource Audio;

    [SerializeField] protected bool CanSelect = true;
    protected bool IsFirst = true;

    protected GameObject SliderObj;
    protected string SESliderName { get; private set;} = "SEControl";
    protected string BGMSliderName { get; private set; } = "BGMControl";
    protected string SliderName;
    private Slider _volumeSlider;
    public const float VolumeUnit = 0.1f;

    // input
    protected const string InputHorizontal = "Horizontal";

    // save
    private const string _bgmVolumeName = "bgmVolume";
    private const string _seVolumeName = "seVolume";


    protected virtual void Awake()
    {
        float seVomule = PlayerPrefs.GetFloat(_seVolumeName, 5f);
        float bgmVolume = PlayerPrefs.GetFloat(_bgmVolumeName, 5f);
    }

    protected virtual void Update()
    {
        // �X���C�_�[��I��
        if (EventSystem.current.currentSelectedGameObject?.name != null && EventSystem.current.currentSelectedGameObject?.name == SliderName)
        {
            // �E����
            if (Input.GetAxisRaw(InputHorizontal) > 0 && CanSelect) { VolumeUp(); }

            // ������
            if (Input.GetAxisRaw(InputHorizontal) < 0 && CanSelect) { VolumeDown(); }
        }

        if (Input.GetAxisRaw(InputHorizontal) == 0)
        {
            CanSelect = true;
        }
    }

    // ���ʂ�1�i�K�グ��
    protected virtual void VolumeUp()
    {
        CanSelect = false;

        Audio.volume += VolumeUnit;
        SaveVolumeSetting();
    }

    // ���ʂ�1�i�K������
    protected virtual void VolumeDown()
    {
        CanSelect = false;

        Audio.volume -= VolumeUnit;
        SaveVolumeSetting();
    }

    /// <summary>
    /// ���ʐݒ�𔽉f����
    /// </summary>
    protected void VolumeSet()
    {
        // ����ݒ莞�ɃX���C�_�[���擾
        if (IsFirst)
        {
            SliderObj = EventSystem.current.currentSelectedGameObject;
            _volumeSlider = SliderObj.GetComponent<Slider>();
        }

        // �ۑ�����Ă��鉹�ʐݒ�𔽉f����
        if (SliderName == SESliderName)
        {
            float seVolume = PlayerPrefs.GetFloat(_seVolumeName, 0.5f);
            Audio.volume = seVolume;
            _volumeSlider.value = seVolume;
            IsFirst = false;
            return;
        }

        float bgmVolume = PlayerPrefs.GetFloat(_bgmVolumeName, 0.5f);
        Audio.volume = bgmVolume;
        _volumeSlider.value = bgmVolume;
        IsFirst = false;
    }

    /// <summary>
    /// ���ʐݒ��ۑ�����
    /// </summary>
    protected void SaveVolumeSetting()
    {
        if (EventSystem.current.currentSelectedGameObject?.name == SESliderName)
        {
            PlayerPrefs.SetFloat(_seVolumeName, Audio.volume);
            VolumeSet();
            return;
        }

        PlayerPrefs.SetFloat(_bgmVolumeName, Audio.volume);
        VolumeSet();
    }

    public void MouseVolumeUp(GameObject gameObject)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
        VolumeUp();
    }

    public void MouseVolumeDown(GameObject gameObject)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
        VolumeDown();
    }
}
