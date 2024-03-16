using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    private const int _closeIconSpriteIndex = 0;
    private const int _openIconSpriteIndex = 1;

    private string _assignmentButton;

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

        _assignmentButton = InputTypeManager.InputType.Menu.ToString();
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

    /// <summary>
    /// �I�v�V�����r���[�̊J��
    /// </summary>
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
            _buttonIcon.sprite = _sourceImage[_openIconSpriteIndex];

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
            _buttonIcon.sprite = _sourceImage[_closeIconSpriteIndex];

            return;
        }
        
    }

    /// <summary>
    /// ���������PC�łɐ؂�ւ���
    /// </summary>
    public void ChangePCButton()
    {
        if (!_operatingGuideChange._isPC)
        {
            _operatingGuideChange.PCButton(Audio, 0);
        }
        
    }

    /// <summary>
    /// ����������R���g���[���[�łɐ؂�ւ���
    /// </summary>
    public void ChangeControllerButton()
    {
        if (!_operatingGuideChange._isController)
        {
            _operatingGuideChange.ControllerButton(Audio, 0);
        }
        
    }

}
