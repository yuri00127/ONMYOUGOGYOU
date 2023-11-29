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

    [Header("�A�C�R��")]
    [SerializeField] private Sprite[] _sourceImage = new Sprite[2];
    private Image _buttonIcon;

    private const string _assignmentButton = "Menu";


    public override void Start()
    {
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
            // �r���[�̐ݒ�
            _optionView.SetActive(false);
            EventSystem.current.SetSelectedGameObject(_titleDefaultForcus);
            _isOpenOptionView = false;

            // �{�^���摜�̐ݒ�
            _buttonIcon.sprite = _sourceImage[0];

            return;
        }
        
    }

}
