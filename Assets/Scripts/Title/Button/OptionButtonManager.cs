using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// タイトル画面でオプションを開く
public class OptionButtonManager : Button
{
    [Header("オプションビュー")]
    [SerializeField] private GameObject _optionView;
    [SerializeField] private GameObject _optionDefaultForcus;
    [SerializeField] private GameObject _titleDefaultForcus;
    private bool _isOpenOptionView = false;

    // ビュー内オブジェクト
    [SerializeField] private Sprite[] _changePCButtonSprites = new Sprite[2];
    private const string _changePCButtonObjName = "ChangeKeyBoardAndMouseButton";
    private Image _changePCButtonImg;
    [SerializeField] private Sprite[] _changeControllerButtonSprites = new Sprite[2];
    private const string _changeControllerButtonObjName = "ChangeControllerButton";
    private Image _changeControllerButtonImg;
    [SerializeField] private Sprite[] _controllerGuideSprites = new Sprite[2];
    private const string _controllerGuideObjName = "ControllerGuide";
    private Image _controllerGuideImg;
    private bool _isPCChange = true;            // 操作をPCに切り替えた直後
    private bool _isControllerChange = false;   // 操作をコントローラーに切り替えた直後

    [Header("ボタンアイコン")]
    [SerializeField] private Sprite[] _sourceImage = new Sprite[2];
    private Image _buttonIcon;

    private const string _assignmentButton = "Menu";

    // Audio
    [SerializeField] private AudioClip SecondSubmitSE;      // ビューを閉じるSE
    [SerializeField] private AudioClip ControllerChangeSE;  // 操作説明画像切り替え時のSE


    public override void Start()
    {
        base.Start();

        // メニューボタンのImageコンポーネントを取得
        _buttonIcon = this.GetComponent<Image>();

        // 操作方法切り替えボタンを取得
        _controllerGuideImg = _optionView.transform.Find(_controllerGuideObjName).GetComponent<Image>();
        _changePCButtonImg = _optionView.transform.Find(_changePCButtonObjName).GetComponent<Image>();
        _changeControllerButtonImg = _optionView.transform.Find(_changeControllerButtonObjName).GetComponent<Image>();
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

    // オプションビューの開閉
    public override void Submit()
    {
        CanInput = false;

        // 開く
        if (!_isOpenOptionView)
        {
            Audio.PlayOneShot(SubmitSE);

            // ビューの設定
            _optionView.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_optionDefaultForcus);
            _isOpenOptionView = true;

            // ボタン画像の設定
            _buttonIcon.sprite = _sourceImage[1];

            return;
        }

        // 閉じる
        if (_isOpenOptionView)
        {
            Audio.PlayOneShot(SecondSubmitSE);

            // ビューの設定
            _optionView.SetActive(false);
            EventSystem.current.SetSelectedGameObject(_titleDefaultForcus);
            _isOpenOptionView = false;

            // ボタン画像の設定
            _buttonIcon.sprite = _sourceImage[0];

            return;
        }
        
    }

    // 操作説明をPC版に切り替える
    public void ChangePCButton()
    {
        if (!_isPCChange)
        {
            Audio.PlayOneShot(ControllerChangeSE);

            // 操作説明画像を変更
            _controllerGuideImg.sprite = _controllerGuideSprites[0];

            // 切り替えボタンの画像を変更
            _changeControllerButtonImg.sprite = _changeControllerButtonSprites[0];
            _changePCButtonImg.sprite = _changePCButtonSprites[1];

            _isPCChange = true;
            _isControllerChange = false;
        }
        
    }

    // 操作説明をコントローラー版に切り替える
    public void ChangeControllerButton()
    {
        if (!_isControllerChange)
        {
            Audio.PlayOneShot(ControllerChangeSE);

            // 操作説明画像を変更
            _controllerGuideImg.sprite = _controllerGuideSprites[1];

            // 切り替えボタンの画像を変更
            _changePCButtonImg.sprite = _changePCButtonSprites[0];
            _changeControllerButtonImg.sprite = _changeControllerButtonSprites[1];

            _isControllerChange = true;
            _isPCChange = false;
        }
        
    }

}
