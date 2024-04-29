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
    public bool CanOpenGuideView = true;

    [SerializeField] private GameObject[] _pageObjcts = new GameObject[2];
    private int pageNo = 0;

    private string _assignmentButton;
    private string _horizontalButton;

    [SerializeField] private AudioClip _pageSE;

    private void Awake()
    {
        _assignmentButton = InputTypeManager.InputType.Y.ToString();
        _horizontalButton = InputTypeManager.InputType.Horizontal.ToString();
    }

    private void Update()
    {
        // �K�C�h�{�^������
        if (Input.GetAxisRaw(_assignmentButton) > 0 && CanInput)
        {
            Submit();
        }

        // �y�[�W�߂������
        if (IsOpenGuideView && Input.GetAxisRaw(_horizontalButton) > 0 && CanInput)
        {
            NextPage();
        }

        if (IsOpenGuideView && Input.GetAxisRaw(_horizontalButton) < 0 && CanInput)
        {
            BackPage();
        }

        // ��x���͂���߂�ƍē��͉\
        if (!CanInput && Input.GetAxisRaw(_assignmentButton) == 0 && Input.GetAxisRaw(_horizontalButton) == 0)
        {
            CanInput = true;
        }
    }
    
    /// <summary>
    /// �K�C�h�r���[�̕\��/��\��
    /// </summary>
    public override void Submit()
    {
        if (!CanOpenGuideView)
        {
            return;
        }

        CanInput = false;
        Audio.PlayOneShot(SubmitSE);

        // �K�C�h���J��
        if (!IsOpenGuideView)
        {
            _guideView.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            IsOpenGuideView = true;
            return;
        }

        // �K�C�h�����
        if (IsOpenGuideView)
        {
            _guideView.SetActive(false);
            EventSystem.current.SetSelectedGameObject(_battleDefaultForcus);
            IsOpenGuideView = false;
            return;
        }
    }

    /// <summary>
    /// ���̃y�[�W��
    /// </summary>
    public void NextPage()
    {
        _pageObjcts[pageNo].SetActive(false);
        pageNo++;
        PageSwitch();
    }

    /// <summary>
    /// �O�̃y�[�W��
    /// </summary>
    public void BackPage()
    {
        _pageObjcts[pageNo].SetActive(false);
        pageNo--;
        PageSwitch();
    }

    /// <summary>
    /// �y�[�W�����[�v������
    /// </summary>
    private void PageSwitch()
    {
        CanInput = false;
        Audio.PlayOneShot(_pageSE);

        // �y�[�WNo�̃��[�v
        if (pageNo < 0)
        {
            pageNo = _pageObjcts.Length - 1;
        }

        if (pageNo > _pageObjcts.Length - 1)
        {
            pageNo = 0;
        }

        // �\��
        _pageObjcts[pageNo].SetActive(true);
    }
}
