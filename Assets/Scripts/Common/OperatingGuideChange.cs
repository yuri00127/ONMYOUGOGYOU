using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OperatingGuideChange : MonoBehaviour
{
    private GameObject _view;     // �\����ʂ̃I�u�W�F�N�g

    [Header("PC����")]
    [SerializeField] private Sprite[] _changePCButtonSprites = new Sprite[2];
    private const string _changePCButtonObjName = "ChangeKeyBoardAndMouseButton";
    private Image _changePCButtonImg;

    [Header("�R���g���[���[����")]
    [SerializeField] private Sprite[] _changeControllerButtonSprites = new Sprite[2];
    private const string _changeControllerButtonObjName = "ChangeControllerButton";
    private Image _changeControllerButtonImg;

    [Header("��������摜")]
    [SerializeField] private Sprite[] _operatingGuideSprites = new Sprite[4];
    private const string _operatingGuideObjName = "ControllerGuide";
    private Image _operatingGuideImg;
    public bool _isPC { get; private set; } = true;            // �����PC�ɐ؂�ւ�������ł���
    public bool _isController { get; private set; } = false;   // ������R���g���[���[�ɐ؂�ւ�������ł���

    [Header("Audio")]
    [SerializeField] private AudioClip ControllerChangeSE;  // ��������摜�؂�ւ�����SE


    /// <summary>
    /// �\���̏���
    /// </summary>
    /// <param name="view">�\������̈�̃I�u�W�F�N�g</param>
    public void SetUp(GameObject view)
    {
        // ������@�؂�ւ��{�^�����擾
        _operatingGuideImg = view.transform.Find(_operatingGuideObjName).GetComponent<Image>();
        _changePCButtonImg = view.transform.Find(_changePCButtonObjName).GetComponent<Image>();
        _changeControllerButtonImg = view.transform.Find(_changeControllerButtonObjName).GetComponent<Image>();
    }

    /// <summary>
    /// ���������PC�łɐ؂�ւ���
    /// </summary>
    /// <param name="audio">SEManager��Audio</param>
    /// <param name="sceneNum">Unity�ł̃V�[���ԍ�</param>
    public void PCButton(AudioSource audio, int sceneNum)
    {
        audio.PlayOneShot(ControllerChangeSE);

        // ��������摜��ύX
        _operatingGuideImg.sprite = _operatingGuideSprites[sceneNum];

        // �؂�ւ��{�^���̉摜��ύX
        _changeControllerButtonImg.sprite = _changeControllerButtonSprites[0];
        _changePCButtonImg.sprite = _changePCButtonSprites[1];

        _isPC = true;
        _isController = false;
    }

    /// <summary>
    /// ����������R���g���[���[�łɐ؂�ւ���
    /// </summary>
    /// <param name="audio">SEManager��Audio</param>
    /// <param name="sceneNum">Unity�ł̃V�[���ԍ�</param>
    public void ControllerButton(AudioSource audio, int sceneNum)
    {
        audio.PlayOneShot(ControllerChangeSE);

        // ��������摜��ύX
        _operatingGuideImg.sprite = _operatingGuideSprites[sceneNum + 1];

        // �؂�ւ��{�^���̉摜��ύX
        _changePCButtonImg.sprite = _changePCButtonSprites[0];
        _changeControllerButtonImg.sprite = _changeControllerButtonSprites[1];

        _isController = true;
        _isPC = false;
    }
}
