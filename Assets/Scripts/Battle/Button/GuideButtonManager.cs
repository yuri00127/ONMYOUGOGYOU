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

    [Header("アイコン")]
    [SerializeField] private Sprite[] _sourceImage = new Sprite[2];
    private Image _buttonIcon;

    private const string _assignmentButton = "Y";

    public override void Start()
    {
        _buttonIcon = this.GetComponent<Image>();
    }

    private void Update()
    {
        // 入力中のみガイドを表示する
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

    // ガイドを開く
    public override void Select()
    {
        CanInput = false;

        // ビューの設定
        _guideView.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        IsOpenGuideView = true;

        // ボタン画像の設定
        _buttonIcon.sprite = _sourceImage[1];
    }

    // ガイドを閉じる
    public override void Deselect()
    {
        // ビューの設定
        _guideView.SetActive(false);
        EventSystem.current.SetSelectedGameObject(_battleDefaultForcus);
        IsOpenGuideView = false;

        // ボタン画像の設定
        _buttonIcon.sprite = _sourceImage[0];
    }

    // マウスでの操作
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
