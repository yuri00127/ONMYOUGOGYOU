using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionButtonManager : Button
{
    private OperatingGuideChange _operatingGuideChange;

    [Header("オプションビュー")]
    [SerializeField] private GameObject _optionView;
    [SerializeField] private GameObject _optionDefaultForcus;
    [SerializeField] private GameObject _titleDefaultForcus;
    private bool _isOpenOptionView = false;

    [Header("ボタンアイコン")]
    [SerializeField] private Sprite[] _sourceImage = new Sprite[2];
    private Image _buttonIcon;
    private const int _closeIconSpriteIndex = 0;
    private const int _openIconSpriteIndex = 1;

    private string _assignmentButton;

    // Audio
    [SerializeField] private AudioClip SecondSubmitSE;      // ビューを閉じるSE


    public override void Start()
    {
        base.Start();

        // 操作説明切り替え処理の準備
        _operatingGuideChange = this.GetComponent<OperatingGuideChange>();
        _operatingGuideChange.SetUp(_optionView);

        // メニューボタンのImageコンポーネントを取得
        _buttonIcon = this.GetComponent<Image>();

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
    /// オプションビューの開閉
    /// </summary>
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
            _buttonIcon.sprite = _sourceImage[_openIconSpriteIndex];

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
            _buttonIcon.sprite = _sourceImage[_closeIconSpriteIndex];

            return;
        }
        
    }

    /// <summary>
    /// 操作説明をPC版に切り替える
    /// </summary>
    public void ChangePCButton()
    {
        if (!_operatingGuideChange._isPC)
        {
            _operatingGuideChange.PCButton(Audio, 0);
        }
        
    }

    /// <summary>
    /// 操作説明をコントローラー版に切り替える
    /// </summary>
    public void ChangeControllerButton()
    {
        if (!_operatingGuideChange._isController)
        {
            _operatingGuideChange.ControllerButton(Audio, 0);
        }
        
    }

}
