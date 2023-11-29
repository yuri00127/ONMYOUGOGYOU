using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PouseButtonManager : Button
{
    [Header("�K�C�h�r���[")]
    [SerializeField] private GameObject _pouseView;
    [SerializeField] private GameObject _pouseDefaultForcus;
    [SerializeField] private GameObject _battleDefaultForcus;
    private bool _isOpenPouseView = false;

    [Header("�A�C�R��")]
    [SerializeField] private Sprite[] _sourceImage = new Sprite[2];
    private Image _buttonIcon;

    private const string _assignmentButton = "X";


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
}
