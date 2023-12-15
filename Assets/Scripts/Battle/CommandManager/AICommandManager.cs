using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AICommandManager : CommandManager
{
    // �X�N���v�g���擾����I�u�W�F�N�g
    private const string _aiCharacterManagerObjName = "AICharacterManager";

    // �X�N���v�g
    private AICharacterManager _aiCharacterManager;

    // �\��
    private const string _aiSelectCommandObjName = "AICommands";        // �I�������R�}���h�̕\���̈�̖��O
    [SerializeField] private Sprite _unknownSprite;                     // �s���ȃR�}���h��Sprite
    private int _showCommandNumber;                                     // ���x���ʂ̕\������R�}���h�̐�
    private int _showMindNumber;                                        // ���x���ʂ̕\������C�̐�

    // �R�}���h�I��
    private const int _startCommandRange = 0;   // �R�}���h�͈̔͂̍ŏ��l
    private const int _endCommandRange = 5;     // �R�}���h�͈̔͂̍ő�l


    protected override void Awake()
    {
        // �\���̈��Object�擾
        SelectCommandObj = GameObject.Find(_aiSelectCommandObjName);
        base.Awake();

        // �X�N���v�g���擾
        _aiCharacterManager = GameObject.Find(_aiCharacterManagerObjName).GetComponent<AICharacterManager>();
    }

    private void Start()
    {
        // �I�����ꂽ�G�L�����N�^�[���擾
        SelectCharacter = _aiCharacterManager.SelectCharacter;

        // ���x���ɂ��Ƃ̃R�}���h�\�������擾
        ShowCommandCheck();

        // 1�^�[���ڂ̃R�}���h������
        SetAICommand();
    }

    public void SetAICommand()
    {
        // �S�Ă̕\�������Z�b�g
        for (int i = 0; i < SelectCommandObjArray.Length; i++)
        {
            SelectCommandImageArray[i].sprite = _unknownSprite;
            MindImageArray[i].sprite = _unknownSprite;
        }

        // �I�����X�g�����Z�b�g
        CommandIdList.Clear();
        IsYinList.Clear();

        // �G�̃R�}���h������
        int selectCommandIndex = 0;
        for (int i = 0; i < SelectCommandObjArray.Length; i++)
        {
            selectCommandIndex = Random.Range(_startCommandRange, _endCommandRange);
            CommandIdList.Add(selectCommandIndex);

            //Debug.Log("�G�̃R�}���h�F" + selectCommandIndex);
        }

        // �C������(0�Ȃ�A�A1�Ȃ�z)
        for (int i = 0; i < SelectCommandObjArray.Length; i++)
        {
            if (Random.Range(0, 2) == 0)
            {
                IsYinList.Add(true);
            }
            IsYinList.Add(false);

            Debug.Log("�G�̉A�z�F" + IsYinList[i]);
        }

        // �R�}���h�̕\��
        List<int> showCommandPositionList = ShowPositionDecide(_showCommandNumber);

        for (int i = 0; i < showCommandPositionList.Count; i++)
        {
            base.SelectCommand(showCommandPositionList[i]);
        }

        // �C�̕\��
        List<int> showMindPositionList = ShowPositionDecide(_showMindNumber);

        for (int i = 0; i < showMindPositionList.Count; i++)
        {
            base.SelectMind(showMindPositionList[i]);
        }
    }

    // AI���x���ɂ���ĕ\������ʂ�ύX
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

        if (_aiCharacterManager.AILevel == 3)
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

            //Debug.Log("�ʒu" + num);
        }

        return _returnPositionIndex;
    }
}
