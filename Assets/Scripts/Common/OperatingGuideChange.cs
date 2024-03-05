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
    [SerializeField] private Sprite[] _operatingGuideSprites = new Sprite[2];
    private const string _operatingGuideObjName = "ControllerGuide";
    private Image _operatingGuideImg;
    public bool _isPC { get; private set; } = true;            // �����PC�ɐ؂�ւ�������ł���
    public bool _isController { get; private set; } = false;   // ������R���g���[���[�ɐ؂�ւ�������ł���

    [Header("Audio")]
    [SerializeField] private AudioClip ControllerChangeSE;  // ��������摜�؂�ւ�����SE


    public void SetUp(GameObject view)
    {
        // ������@�؂�ւ��{�^�����擾
        _operatingGuideImg = view.transform.Find(_operatingGuideObjName).GetComponent<Image>();
        _changePCButtonImg = view.transform.Find(_changePCButtonObjName).GetComponent<Image>();
        _changeControllerButtonImg = view.transform.Find(_changeControllerButtonObjName).GetComponent<Image>();
    }

    // ���������PC�łɐ؂�ւ���
    public void PCButton(AudioSource audio)
    {
        audio.PlayOneShot(ControllerChangeSE);

        // ��������摜��ύX
        _operatingGuideImg.sprite = _operatingGuideSprites[0];

        // �؂�ւ��{�^���̉摜��ύX
        _changeControllerButtonImg.sprite = _changeControllerButtonSprites[0];
        _changePCButtonImg.sprite = _changePCButtonSprites[1];

        _isPC = true;
        _isController = false;
    }

    // ����������R���g���[���[�łɐ؂�ւ���
    public void ControllerButton(AudioSource audio)
    {
        audio.PlayOneShot(ControllerChangeSE);

        // ��������摜��ύX
        _operatingGuideImg.sprite = _operatingGuideSprites[1];

        // �؂�ւ��{�^���̉摜��ύX
        _changePCButtonImg.sprite = _changePCButtonSprites[0];
        _changeControllerButtonImg.sprite = _changeControllerButtonSprites[1];

        _isController = true;
        _isPC = false;
    }
}
