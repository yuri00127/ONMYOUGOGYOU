using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// �^�C�g����ʂŃI�v�V�������J��
public class OptionButtonManager : Button
{
    private OperatingGuideChange _operatingGuideChange;

    [Header("�I�v�V�����r���[")]
    [SerializeField] private GameObject _optionView;
    [SerializeField] private GameObject _optionDefaultForcus;
    [SerializeField] private GameObject _titleDefaultForcus;
    private bool _isOpenOptionView = false;

    [Header("�{�^���A�C�R��")]
    [SerializeField] private Sprite[] _sourceImage = new Sprite[2];
    private Image _buttonIcon;

    private const string _assignmentButton = "Menu";

    // Audio
    [SerializeField] private AudioClip SecondSubmitSE;      // �r���[�����SE


    public override void Start()
    {
        base.Start();

        // ��������؂�ւ������̏���
        _operatingGuideChange = this.GetComponent<OperatingGuideChange>();
        _operatingGuideChange.SetUp(_optionView);

        // ���j���[�{�^����Image�R���|�[�l���g���擾
        _buttonIcon = this.GetComponent<Image>();        
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
        if (!_operatingGuideChange._isPC)
        {
            _operatingGuideChange.PCButton(Audio);
        }
        
    }

    // ����������R���g���[���[�łɐ؂�ւ���
    public void ChangeControllerButton()
    {
        if (!_operatingGuideChange._isController)
        {
            _operatingGuideChange.ControllerButton(Audio);
        }
        
    }

}
