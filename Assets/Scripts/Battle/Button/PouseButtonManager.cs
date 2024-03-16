using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PouseButtonManager : Button
{
    private OperatingGuideChange _operatingGuideChange;

    [Header("ガイドビュー")]
    [SerializeField] private GameObject _pouseView;
    [SerializeField] private GameObject _pouseDefaultForcus;
    [SerializeField] private GameObject _battleDefaultForcus;
    private bool _isOpenPouseView = false;

    [Header("アイコン")]
    [SerializeField] private Sprite[] _sourceImage = new Sprite[2];
    private Image _buttonIcon;

    private string _assignmentButton;


    public override void Start()
    {
        base.Start();
        _buttonIcon = this.GetComponent<Image>();

        // 操作説明切り替え処理の準備
        _operatingGuideChange = this.GetComponent<OperatingGuideChange>();
        _operatingGuideChange.SetUp(_pouseView);

        _assignmentButton = InputTypeManager.InputType.Menu.ToString();
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

    /// <summary>
    /// ポーズビューの開閉
    /// </summary>
    public override void Submit()
    {
        CanInput = false;
        Audio.PlayOneShot(SubmitSE);

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

    /// <summary>
    /// 操作説明をPC版に切り替える
    /// </summary>
    public void ChangePCButton()
    {
        if (!_operatingGuideChange._isPC)
        {
            _operatingGuideChange.PCButton(Audio, 2);
        }
    }

    /// <summary>
    /// 操作説明をコントローラー版に切り替える
    /// </summary>
    public void ChangeControllerButton()
    {
        if (!_operatingGuideChange._isController)
        {
            _operatingGuideChange.ControllerButton(Audio, 2);
        }
    }
}
