using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GuideButtonManager : Button
{
    [Header("�K�C�h�r���[")]
    [SerializeField] private GameObject _guideView;
    [SerializeField] private GameObject _battleDefaultForcus;
    public bool IsOpenGuideView { get; private set; } = false;
    private bool _isPointerDown = false;

    private const string _assignmentButton = "Y";


    private void Update()
    {
        // ���͒��̂݃K�C�h��\������
        if (Input.GetAxis(_assignmentButton) > 0 && CanInput)
        {
            Select();
        }

        if (!CanInput && Input.GetAxisRaw(_assignmentButton) == 0 && !_isPointerDown)
        {
            Deselect();
            CanInput = true;
        }
    }

    // �K�C�h���J��
    public override void Select()
    {
        CanInput = false;

        // �r���[�̐ݒ�
        _guideView.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        IsOpenGuideView = true;
    }

    // �K�C�h�����
    public override void Deselect()
    {
        // �r���[�̐ݒ�
        _guideView.SetActive(false);
        EventSystem.current.SetSelectedGameObject(_battleDefaultForcus);
        IsOpenGuideView = false;
    }

    // �}�E�X�ł̑���
    public void PointerDown()
    {
        _isPointerDown = true;
        Select();
    }

    public void PointerUp()
    {
        _isPointerDown = false;
        Select();
    }
}
