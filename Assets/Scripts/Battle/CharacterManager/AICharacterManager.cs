using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AICharacterManager : CharacterManager
{
    private int _aiLevel;
    private const int _startCommandRange = 0;
    private const int _endCommandRange = 5;

    [Header("�R�}���h�\��")]
    [SerializeField] private GameObject[] _commandObjArray = new GameObject[3];
    private Image[] _commandImageArray = new Image[3];
    private Image[] _mindImageArray = new Image[3];
    private const string _mindObjName = "Mind";
    [SerializeField] private Sprite[] _yinYanSprites = new Sprite[2];
    [SerializeField] private Sprite _unknownSprite;
    private int _showCommandNumber;     // �\������R�}���h�̐�
    private int _showMindNumber;        // �\������C�̐�

    // �R�}���h�̌���
    private int[] _selectCommandIndexArray = new int[3];
    private int[] _selectMindIndexArray = new int[3];


    protected override void Start()
    {
        // �I�����ꂽ�G�L�����N�^�[���擾
        int aiCharacterId = 1;
        /*�{�ԗp
        int aiCharacterId = PlayerPrefs.GetInt(SelectCharacterData.SaveAICharacterId, 1);
        */
        SelectCharacter = CharacterDataBase.CharacterList[aiCharacterId - 1];


        // �I�����ꂽ�G�̋������擾
        _aiLevel = 1;
        /*�{�ԗp
        _aiLevel = PlayerPrefs.GetInt(SelectCharacterData.SaveAILevel, 1);
        */

        // �R�}���h�\���I�u�W�F�N�g��Image�R���|�[�l���g���擾
        for (int i = 0; i < _commandObjArray.Length; i++)
        {
            _commandImageArray[i] = _commandObjArray[i].GetComponent<Image>();
            _mindImageArray[i] = _commandObjArray[i].transform.Find(_mindObjName).GetComponent<Image>();
        }

        // �\������R�}���h�̐����擾
        ShowCommandCheck();

        // 1�^�[���ڂ̃R�}���h������
        SetAICommand();
    }

    public void SetAICommand()
    {
        // �S�Ă̕\�������Z�b�g
        for (int i = 0; i < _commandObjArray.Length; i++)
        {
            _commandImageArray[i].sprite = _unknownSprite;
            _mindImageArray[i].sprite = _unknownSprite;
        }


        // �G�̃R�}���h������
        for (int i = 0; i < _selectCommandIndexArray.Length; i++)
        {
            _selectCommandIndexArray[i] = Random.Range(_startCommandRange, _endCommandRange);
            Debug.Log("�G�̃R�}���h�F" + _selectCommandIndexArray[i]);
        }

        // �C������(0�Ȃ�A�A1�Ȃ�z)
        for (int i = 0; i < _selectMindIndexArray.Length; i++)
        {
            _selectMindIndexArray[i] = Random.Range(0, 2);
            Debug.Log("�G�̉A�z�F" + _selectMindIndexArray[i]);
        }

        // �R�}���h��\������
        List<int> showCommandPositionList = ShowPositionDecide(_showCommandNumber);

        for (int i = 0; i < showCommandPositionList.Count; i++)
        {
            int positionNumber = showCommandPositionList[i];
            _commandImageArray[positionNumber].sprite = SelectCharacter.SelectCommandSprites[_selectCommandIndexArray[positionNumber]];
        }

        // �C��\������
        List<int> showMindPositionList = ShowPositionDecide(_showMindNumber);

        for (int i = 0; i < showMindPositionList.Count; i++)
        {
            int positionNumber = showMindPositionList[i];
            _mindImageArray[positionNumber].sprite = _yinYanSprites[_selectMindIndexArray[i]];
        }

    }

    // AI���x���ɂ���ĕ\������ʂ�ύX
    private void ShowCommandCheck()
    {
        if (_aiLevel == 1)
        {
            _showCommandNumber = 2;
            _showMindNumber = 2;
            return;
        }

        if (_aiLevel == 2)
        {
            _showCommandNumber = 1;
            _showMindNumber = 1;
            return;
        }

        if (_aiLevel == 3)
        {
            _showCommandNumber = 1;
            _showMindNumber = 0;
            return;
        }
    }

    // �\���ʒu�����߂�
    private List<int> ShowPositionDecide(int decideCount)
    {
        List<int> _positionIndex = new List<int>() { 0, 1, 2 };
        List<int> _returnPositionIndex = new List<int>() { };

        // �\����������̈ʒu����
        for (int i = 0; i < decideCount; i++)
        {
            int num = Random.Range(0, _positionIndex.Count);
            _returnPositionIndex.Add(_positionIndex[num]);
            _positionIndex.RemoveAt(num);
        }

        return _returnPositionIndex;
    }
}
