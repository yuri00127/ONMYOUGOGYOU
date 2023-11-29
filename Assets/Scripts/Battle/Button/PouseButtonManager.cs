using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PouseButtonManager : Button
{
    [Header("ガイドビュー")]
    [SerializeField] private GameObject _pouseView;
    [SerializeField] private GameObject _pouseDefaultForcus;
    [SerializeField] private GameObject _battleDefaultForcus;
    private bool _isOpenPouseView = false;

    [Header("アイコン")]
    [SerializeField] private Sprite[] _sourceImage = new Sprite[2];
    private Image _buttonIcon;

    private const string _assignmentButton = "X";


    public override void Start()
    {
        _buttonIcon = this.GetComponent<Image>();
    }

    private void Update()
    {
        // 入力
        if (Input.GetAxis(_assignmentButton) > 0 && CanInput)
        {
            Submit();
        }

        // 一度入力をやめると再入力可能
        if (Input.GetAxisRaw(_assignmentButton) == 0)
        {
            CanInput = true;
        }
    }

    // ポーズの開閉
    public override void Submit()
    {
        CanInput = false;

        // 開く
        if (!_isOpenPouseView)
        {
            // ビューの設定
            _pouseView.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_pouseDefaultForcus);
            _isOpenPouseView = true;

            // ボタン画像の設定
            _buttonIcon.sprite = _sourceImage[1];

            return;
        }

        // 閉じる
        if (_isOpenPouseView)
        {
            // ビューの設定
            _pouseView.SetActive(false);
            EventSystem.current.SetSelectedGameObject(_battleDefaultForcus);
            _isOpenPouseView = false;

            // ボタン画像の設定
            _buttonIcon.sprite = _sourceImage[0];

            return;
        }
    }

    public override void PointerEnter(GameObject gameObject)
    {
        base.PointerEnter(gameObject);
    }
}
