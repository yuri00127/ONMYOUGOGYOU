using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// �^�C�g����ʂŃI�v�V�������J��
public class OptionButtonManager : Button
{
    [Header("�I�v�V�����r���[")]
    [SerializeField] private GameObject _optionView;
    [SerializeField] private GameObject _optionDefaultForcus;
    [SerializeField] private GameObject _titleDefaultForcus;
    private bool _isOpenOptionView = false;

    // �r���[���I�u�W�F�N�g
    [SerializeField] private Sprite[] _changePCButtonSprites = new Sprite[2];
    private const string _changePCButtonObjName = "ChangeKeyBoardAndMouseButton";
    private Image _changePCButtonImg;
    [SerializeField] private Sprite[] _changeControllerButtonSprites = new Sprite[2];
    private const string _changeControllerButtonObjName = "ChangeControllerButton";
    private Image _changeControllerButtonImg;
    [SerializeField] private Sprite[] _controllerGuideSprites = new Sprite[2];
    private const string _controllerGuideObjName = "ControllerGuide";
    private Image _controllerGuideImg;
    private bool _isPCChange = true;            // �����PC�ɐ؂�ւ�������
    private bool _isControllerChange = false;   // ������R���g���[���[�ɐ؂�ւ�������

    [Header("�{�^���A�C�R��")]
    [SerializeField] private Sprite[] _sourceImage = new Sprite[2];
    private Image _buttonIcon;

    private const string _assignmentButton = "Menu";

    // Audio
    [SerializeField] private AudioClip SecondSubmitSE;      // �r���[�����SE
    [SerializeField] private AudioClip ControllerChangeSE;  // ��������摜�؂�ւ�����SE


    public override void Start()
    {
        base.Start();

        // ���j���[�{�^����Image�R���|�[�l���g���擾
        _buttonIcon = this.GetComponent<Image>();

        // ������@�؂�ւ��{�^�����擾
        _controllerGuideImg = _optionView.transform.Find(_controllerGuideObjName).GetComponent<Image>();
        _changePCButtonImg = _optionView.transform.Find(_changePCButtonObjName).GetComponent<Image>();
        _changeControllerButtonImg = _optionView.transform.Find(_changeControllerButtonObjName).GetComponent<Image>();
    }

    private void Update()
    {
        // ����
        if (Input.GetAxis(_assignmentButton) > 0 && CanInput)
        {
            Submit();
        }

        // ��x���͂���߂�ƍē��͉\
        if (Input.GetAxisRaw(_assignmentButton) == 0)
        {
            CanInput = true;
        }
    }

    // �I�v�V�����r���[�̊J��
    public override void Submit()
    {
        CanInput = false;

        // �J��
        if (!_isOpenOptionView)
        {
            Audio.PlayOneShot(SubmitSE);

            // �r���[�̐ݒ�
            _optionView.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_optionDefaultForcus);
            _isOpenOptionView = true;

            // �{�^���摜�̐ݒ�
            _buttonIcon.sprite = _sourceImage[1];

            return;
        }

        // ����
        if (_isOpenOptionView)
        {
            Audio.PlayOneShot(SecondSubmitSE);

            // �r���[�̐ݒ�
            _optionView.SetActive(false);
            EventSystem.current.SetSelectedGameObject(_titleDefaultForcus);
            _isOpenOptionView = false;

            // �{�^���摜�̐ݒ�
            _buttonIcon.sprite = _sourceImage[0];

            return;
        }
        
    }

    // ���������PC�łɐ؂�ւ���
    public void ChangePCButton()
    {
        if (!_isPCChange)
        {
            Audio.PlayOneShot(ControllerChangeSE);

            // ��������摜��ύX
            _controllerGuideImg.sprite = _controllerGuideSprites[0];

            // �؂�ւ��{�^���̉摜��ύX
            _changeControllerButtonImg.sprite = _changeControllerButtonSprites[0];
            _changePCButtonImg.sprite = _changePCButtonSprites[1];

            _isPCChange = true;
            _isControllerChange = false;
        }
        
    }

    // ����������R���g���[���[�łɐ؂�ւ���
    public void ChangeControllerButton()
    {
        if (!_isControllerChange)
        {
            Audio.PlayOneShot(ControllerChangeSE);

            // ��������摜��ύX
            _controllerGuideImg.sprite = _controllerGuideSprites[1];

            // �؂�ւ��{�^���̉摜��ύX
            _changePCButtonImg.sprite = _changePCButtonSprites[0];
            _changeControllerButtonImg.sprite = _changeControllerButtonSprites[1];

            _isControllerChange = true;
            _isPCChange = false;
        }
        
    }

}
