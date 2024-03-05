using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PouseButtonManager : Button
{
    private OperatingGuideChange _operatingGuideChange;

    [Header("�K�C�h�r���[")]
    [SerializeField] private GameObject _pouseView;
    [SerializeField] private GameObject _pouseDefaultForcus;
    [SerializeField] private GameObject _battleDefaultForcus;
    private bool _isOpenPouseView = false;

    [Header("�A�C�R��")]
    [SerializeField] private Sprite[] _sourceImage = new Sprite[2];
    private Image _buttonIcon;

    private const string _assignmentButton = "Menu";


    public override void Start()
    {
        base.Start();

        _buttonIcon = this.GetComponent<Image>();

        // ��������؂�ւ������̏���
        _operatingGuideChange = this.GetComponent<OperatingGuideChange>();
        _operatingGuideChange.SetUp(_pouseView);
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

    // �|�[�Y�̊J��
    public override void Submit()
    {
        CanInput = false;

        // �J��
        if (!_isOpenPouseView)
        {
            // �r���[�̐ݒ�
            _pouseView.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_pouseDefaultForcus);
            _isOpenPouseView = true;

            // �{�^���摜�̐ݒ�
            _buttonIcon.sprite = _sourceImage[1];

            Debug.Log(EventSystem.current.currentSelectedGameObject);
            return;
        }

        // ����
        if (_isOpenPouseView)
        {
            // �r���[�̐ݒ�
            _pouseView.SetActive(false);
            EventSystem.current.SetSelectedGameObject(_battleDefaultForcus);
            _isOpenPouseView = false;

            // �{�^���摜�̐ݒ�
            _buttonIcon.sprite = _sourceImage[0];

            return;
        }
    }

    public override void PointerEnter(GameObject gameObject)
    {
        base.PointerEnter(gameObject);
    }

    // ���������PC�łɐ؂�ւ���
    public void ChangePCButton()
    {
        if (!_operatingGuideChange._isPC)
        {
            _operatingGuideChange.PCButton(Audio, 2);
        }

    }

    // ����������R���g���[���[�łɐ؂�ւ���
    public void ChangeControllerButton()
    {
        if (!_operatingGuideChange._isController)
        {
            _operatingGuideChange.ControllerButton(Audio, 2);
        }

    }
}
