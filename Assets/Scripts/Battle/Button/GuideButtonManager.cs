using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GuideButtonManager : Button
{
    [Header("ガイドビュー")]
    [SerializeField] private GameObject _guideView;
    [SerializeField] private GameObject _battleDefaultForcus;
    public bool IsOpenGuideView { get; private set; } = false;
    private bool _isPointerDown = false;

    [SerializeField] private GameObject[] _pageObjcts = new GameObject[2];
    private int pageNo = 0;

    private const string _assignmentButton = "Y";
    private const string _horizontalButton = "Horizontal";

    [SerializeField] private AudioClip _pageSE;


    private void Update()
    {
        // ガイドボタン入力
        if (Input.GetAxis(_assignmentButton) > 0 && CanInput)
        {
            Submit();
        }

        // ページめくり入力
        if (IsOpenGuideView && Input.GetAxisRaw(_horizontalButton) > 0 && CanInput)
        {
            NextPage();
        }

        if (IsOpenGuideView && Input.GetAxisRaw(_horizontalButton) < 0 && CanInput)
        {
            BackPage();
        }

        // 一度入力をやめると再入力可能
        if (!CanInput && Input.GetAxisRaw(_assignmentButton) == 0 && Input.GetAxisRaw(_horizontalButton) == 0)
        {
            CanInput = true;
        }
    }
    
    /// <summary>
    /// ガイドビューの表示/非表示
    /// </summary>
    public override void Submit()
    {
        CanInput = false;
        Audio.PlayOneShot(SubmitSE);

        // ガイドを開く
        if (!IsOpenGuideView)
        {
            _guideView.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            IsOpenGuideView = true;
            return;
        }

        // ガイドを閉じる
        if (IsOpenGuideView)
        {
            _guideView.SetActive(false);
            EventSystem.current.SetSelectedGameObject(_battleDefaultForcus);
            IsOpenGuideView = false;
            return;
        }
    }

    /// <summary>
    /// 次のページへ
    /// </summary>
    public void NextPage()
    {
        _pageObjcts[pageNo].SetActive(false);
        pageNo++;
        PageSwitch();
    }

    /// <summary>
    /// 前のページへ
    /// </summary>
    public void BackPage()
    {
        _pageObjcts[pageNo].SetActive(false);
        pageNo--;
        PageSwitch();
    }

    /// <summary>
    /// ページをループさせる
    /// </summary>
    private void PageSwitch()
    {
        CanInput = false;
        Audio.PlayOneShot(_pageSE);

        // ページNoのループ
        if (pageNo < 0)
        {
            pageNo = _pageObjcts.Length - 1;
        }

        if (pageNo > _pageObjcts.Length - 1)
        {
            pageNo = 0;
        }

        // 表示
        _pageObjcts[pageNo].SetActive(true);
    }
}
