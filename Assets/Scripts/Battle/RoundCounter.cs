using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundCounter : MonoBehaviour
{
    [Header("�J�E���g")]
    [SerializeField] private GameObject _roundCounterObj;      // �J�E���g�w�i�̉摜
    [SerializeField] private GameObject _firstPlaceObj;        // 1�̈ʂ̃I�u�W�F�N�g
    [SerializeField] private GameObject _decimalPlaceObj;      // 2�̈ʂ̃I�u�W�F�N�g
    private Image _firstPlaceImage;
    private Image _decimalPlaceImage;
    [SerializeField] private Sprite[] _oneDigitArray = new Sprite[9];       // 1���̎��̉摜
    [SerializeField] private Sprite[] _twoDigitArray = new Sprite[10];       // 2���̎��̉摜
    private Vector2 _oneDigitFirstPlacePosition = new Vector2(5f, 0f);      // 1���̎���1�̈ʂ̈ʒu
    private Vector2 _twoDigitFirstPlacePosition = new Vector2(20f, 0f);     // 2���̎���1�̈ʂ̈ʒu

    // �J�E���g�A�j���[�V����
    private Animator _roundCounterAnim;
    private const string _countUpAnimationBoolName = "CountUpBool";

    private bool _isOneDigit = true;    // ���݂̃��E���h��1�����ǂ���
    private int _roundCount;


    private void Start()
    {
        // Image�R���|�[�l���g���擾
        _firstPlaceImage = _firstPlaceObj.GetComponent<Image>();

        // Animator�R���|�[�l���g���擾
        _roundCounterAnim = _roundCounterObj.GetComponent<Animator>();
        //StartCoroutine(StartRoundAnimation());
    }

    public void CountUp(int round)
    {
        _roundCount = round;

        // 2���ڂ�\�����鏈��
        if (_isOneDigit && _roundCount >= 10)
        {
            _isOneDigit = false;
            _decimalPlaceObj.SetActive(true);
            _decimalPlaceImage = _decimalPlaceObj.GetComponent<Image>();
        }

        // ���E���h��1���̎��̏���
        if (_isOneDigit)
        {
            FirstPlaceUpdate(_roundCount - 1);

            // ���E���h�J�n�̃A�j���[�V����
            StartCoroutine(StartRoundAnimation());

            return;
        }

        // ���E���h��2���̎��̏���
        FirstPlaceUpdate(_roundCount % 10);
        DecimalPlaceUpdate(_roundCount / 10);

        // ���E���h�J�n�̃A�j���[�V����
        StartCoroutine(StartRoundAnimation());
    }

    // 1���ڂ̍X�V
    private void FirstPlaceUpdate(int countNumber)
    {
        if (_isOneDigit)
        {
            _firstPlaceImage.sprite = _oneDigitArray[countNumber];
            return;
        }

        _firstPlaceImage.sprite = _twoDigitArray[countNumber];
    }

    // 2���ڂ̍X�V
    private void DecimalPlaceUpdate(int countNumber)
    {
        _decimalPlaceImage.sprite = _twoDigitArray[countNumber];
    }

    // ���E���h�J�n���̃A�j���[�V����
    private IEnumerator StartRoundAnimation()
    {
        _roundCounterAnim.SetBool(_countUpAnimationBoolName, true);

        yield return new WaitForSeconds(0.3f);

        _roundCounterAnim.SetBool(_countUpAnimationBoolName, false);
    }

}
