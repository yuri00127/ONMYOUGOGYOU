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

    [Header("アイコン")]
    [SerializeField] private Sprite[] _sourceImage = new Sprite[2];
    private Image _buttonIcon;

    private const string _assignmentButton = "Menu";


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

    // オプションビューの開閉
    public override void Submit()
    {
        CanInput = false;

        // 開く
        if (!_isOpenOptionView)
        {
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
            // ビューの設定
            _optionView.SetActive(false);
            EventSystem.current.SetSelectedGameObject(_titleDefaultForcus);
            _isOpenOptionView = false;

            // ボタン画像の設定
            _buttonIcon.sprite = _sourceImage[0];

            return;
        }
        
    }

}
