using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AICommandManager : CommandManager
{
    // �X�N���v�g
    [SerializeField] private AICharacterManager _aiCharacterManager;
    [SerializeField] private SelectCharacterData _selectCharacterData;
    [SerializeField] private CharacterDataBase _characterDataBase;

    // �\��
    [SerializeField] private Sprite _unknownCommandSprite;    // �s���ȃR�}���h��Sprite
    [SerializeField] private Sprite _unknownMindSprite;       // �s���ȋC��Sprite
    private int _showCommandNumber;                           // ���x���ʂ̕\������R�}���h�̐�
    private int _showMindNumber;                              // ���x���ʂ̕\������C�̐�

    // �R�}���h�I��
    private const int _minCommandAttributeRange = 0;            // ����ID�͈̔͂̍ŏ��l
    private const int _maxCommandAttributeRange = 5;            // ����ID�͈̔͂̍ő�l
    private int _aiAttributeId;                                 // �G�̑���ID
    private const int _maxAiLevel = 3;                          // �GAI�̍ő僌�x��
    private bool _isFirstRound = true;                          // �ŏ��̃^�[�����ǂ���
    private int[] commandArray = new int[] { 1, 1, 1, 1, 1 };   // �R�}���h�̏d�݂Â�


    protected override void Awake()
    {
        base.Awake();

        // �G�̑������擾
        int aiCharacterId = PlayerPrefs.GetInt(_selectCharacterData.SaveAICharacterId);
        _aiAttributeId = _characterDataBase.CharacterList[aiCharacterId - 1].AttributeId;
    }

    private void Start()
    {
        // �I�����ꂽ�G�L�����N�^�[���擾
        SelectCharacter = _aiCharacterManager.SelectCharacter;

        // ���x���ɂ��Ƃ̃R�}���h�\�������擾
        ShowCommandCheck();

        // 1�^�[���ڂ̃R�}���h������
        ShowAICommand();
    }

    /// <summary>
    /// AI���I�񂾃R�}���h��\������
    /// </summary>
    public void ShowAICommand()
    {
        // �S�Ă̕\�������Z�b�g
        for (var i = 0; i < SelectCommandAttributeObjArray.Length; i++)
        {
            SelectCommandAttributeImageArray[i].sprite = _unknownCommandSprite;
            SelectCommandMindImageArray[i].sprite = _unknownMindSprite;
        }

        // �I�����X�g�����Z�b�g
        CommandIdList.Clear();
        IsYinList.Clear();

        // �G�̃R�}���h������
        var selectCommandIndex = 0;

        for (var i = 0; i < SelectCommandAttributeObjArray.Length; i++)
        {
            selectCommandIndex = SetAICommand(_aiCharacterManager.AILevel);
            CommandIdList.Add(selectCommandIndex);
        }

        // �C������(0�Ȃ�A�A1�Ȃ�z)
        for (var i = 0; i < SelectCommandAttributeObjArray.Length; i++)
        {
            if (Random.Range(0, 2) == 0)
            {
                IsYinList.Add(true);
            }
            IsYinList.Add(false);
        }

        // �R�}���h�̕\��
        List<int> showCommandPositionList = ShowPositionDecide(_showCommandNumber);

        for (var i = 0; i < showCommandPositionList.Count; i++)
        {
            base.SelectCommand(showCommandPositionList[i]);
        }

        // �C�̕\��
        List<int> showMindPositionList = ShowPositionDecide(_showMindNumber);

        for (var i = 0; i < showMindPositionList.Count; i++)
        {
            base.SelectMind(showMindPositionList[i]);
        }
    }

    /// <summary>
    /// AI���x���ɂ���āA�R�}���h��\������ʂ�ύX
    /// </summary>
    private void ShowCommandCheck()
    {
        if (_aiCharacterManager.AILevel == 1)
        {
            _showCommandNumber = 2;
            _showMindNumber = 2;
            return;
        }

        if (_aiCharacterManager.AILevel == 2)
        {
            _showCommandNumber = 1;
            _showMindNumber = 1;
            return;
        }

        if (_aiCharacterManager.AILevel == _maxAiLevel)
        {
            _showCommandNumber = 1;
            _showMindNumber = 0;
            return;
        }
    }

    /// <summary>
    ///  �R�}���h�̕\���ʒu�����߂�
    /// </summary>
    /// <param name="decideCount">�\�������</param>
    /// <returns>�\���ʒu�̃��X�g</returns>
    private List<int> ShowPositionDecide(int decideCount)
    {
        List<int> _positionIndex = new List<int>() { 0, 1, 2 };
        List<int> _returnPositionIndex = new List<int>() { };

        // �ʒu����
        for (var i = 0; i < decideCount; i++)
        {
            int num = Random.Range(0, _positionIndex.Count);
            _returnPositionIndex.Add(_positionIndex[num]);
            _positionIndex.RemoveAt(num);
        }

        return _returnPositionIndex;
    }

    /// <summary>
    /// �G�̃R�}���h����
    /// </summary>
    /// <param name="aiLevel">�GAI�̃��x��</param>
    /// <returns>���肵���R�}���h��index</returns>
    private int SetAICommand(int aiLevel)
    {
        // AI���x��3(����̂ݏ���)
        if (_isFirstRound)
        {
            if (aiLevel == _maxAiLevel)
            {
                switch (_aiAttributeId)
                {
                    // ��
                    case 1:
                        commandArray[0] = 3;
                        break;
                    // ��
                    case 2:
                        commandArray[1] = 3;
                        break;
                    // ��
                    case 3:
                        commandArray[2] = 3;
                        break;
                    // �y
                    case 4:
                        commandArray[3] = 3;
                        break;
                    // ��
                    case 5:
                        commandArray[4] = 3;
                        break;
                    default:
                        break;
                }
            }
        }

        // AI���x��3
        var total = 0.0f;

        foreach (float elem in commandArray)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < commandArray.Length; i++)
        {
            if (randomPoint < commandArray[i])
            {
                return i;
            }

            randomPoint -= commandArray[i];
        }

        // AI���x��1,2
        return Random.Range(_minCommandAttributeRange, _maxCommandAttributeRange);
    }

}
