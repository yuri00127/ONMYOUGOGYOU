using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundCounter : MonoBehaviour
{
    [Header("カウント")]
    [SerializeField] private GameObject _roundCounterObj;      // カウント背景の画像
    [SerializeField] private GameObject _firstPlaceObj;        // 1の位のオブジェクト
    [SerializeField] private GameObject _decimalPlaceObj;      // 2の位のオブジェクト
    private Image _firstPlaceImage;
    private Image _decimalPlaceImage;
    [SerializeField] private Sprite[] _oneDigitArray = new Sprite[9];       // 1桁の時の画像
    [SerializeField] private Sprite[] _twoDigitArray = new Sprite[10];       // 2桁の時の画像
    private Vector2 _oneDigitFirstPlacePosition = new Vector2(5f, 0f);      // 1桁の時の1の位の位置
    private Vector2 _twoDigitFirstPlacePosition = new Vector2(20f, 0f);     // 2桁の時の1の位の位置

    // カウントアニメーション
    private Animator _roundCounterAnim;
    private const string _countUpAnimationBoolName = "CountUpBool";

    private bool _isOneDigit = true;    // 現在のラウンドが1桁かどうか
    private int _roundCount;


    private void Start()
    {
        // Imageコンポーネントを取得
        _firstPlaceImage = _firstPlaceObj.GetComponent<Image>();

        // Animatorコンポーネントを取得
        _roundCounterAnim = _roundCounterObj.GetComponent<Animator>();
        //StartCoroutine(StartRoundAnimation());
    }

    public void CountUp(int round)
    {
        _roundCount = round;

        // 2桁目を表示する処理
        if (_isOneDigit && _roundCount >= 10)
        {
            _isOneDigit = false;
            _decimalPlaceObj.SetActive(true);
            _decimalPlaceImage = _decimalPlaceObj.GetComponent<Image>();
        }

        // ラウンドが1桁の時の処理
        if (_isOneDigit)
        {
            FirstPlaceUpdate(_roundCount - 1);

            // ラウンド開始のアニメーション
            StartCoroutine(StartRoundAnimation());

            return;
        }

        // ラウンドが2桁の時の処理
        FirstPlaceUpdate(_roundCount % 10);
        DecimalPlaceUpdate(_roundCount / 10);

        // ラウンド開始のアニメーション
        StartCoroutine(StartRoundAnimation());
    }

    // 1桁目の更新
    private void FirstPlaceUpdate(int countNumber)
    {
        if (_isOneDigit)
        {
            _firstPlaceImage.sprite = _oneDigitArray[countNumber];
            return;
        }

        _firstPlaceImage.sprite = _twoDigitArray[countNumber];
    }

    // 2桁目の更新
    private void DecimalPlaceUpdate(int countNumber)
    {
        _decimalPlaceImage.sprite = _twoDigitArray[countNumber];
    }

    // ラウンド開始時のアニメーション
    private IEnumerator StartRoundAnimation()
    {
        _roundCounterAnim.SetBool(_countUpAnimationBoolName, true);

        yield return new WaitForSeconds(0.3f);

        _roundCounterAnim.SetBool(_countUpAnimationBoolName, false);
    }

}
