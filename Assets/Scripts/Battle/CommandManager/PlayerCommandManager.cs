using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCommandManager : CommandManager
{
    // �X�N���v�g���擾����I�u�W�F�N�g
    private const string _changeButtonObj = "ChangeButton";

    // �X�N���v�g
    private YinYangChangeButton _yinYangChangeButton;

    [Header("�R�}���hObject")]
    [SerializeField] private GameObject[] _commandButtonArray = new GameObject[5];  // �R�}���h�{�^����Object
    private Image[] _commandButtonImageArray = new Image[5];                        // �R�}���h�{�^����Image�R���|�[�l���g
    private const string _playerSelectCommandObjName = "PlayerCommands";            // �I�������R�}���h�̕\���̈�̖��O
    [SerializeField] private Sprite _nullSprite;                                    // �����I�����Ă��Ȃ��Ƃ��̉摜

    private int _selectingCommandSequence = 0;      // �I�����ꂽ�R�}���h�̐�
    private int _maxSelectingCommandSequence = 3;   // �I���ł���R�}���h�̍ő吔
    private bool _isAllSelect = false;              // �R�}���h������܂őI�����ꂽ���ǂ���

    // Input
    protected bool CanInput = true;                 // ���͂��\�Ƃ��鐧��
    private const string _inputCancel = "Cancel";   // �R�}���h�̎���ɑΉ��������


    protected override void Awake()
    {
        // �X�N���v�g�̎擾
        _yinYangChangeButton = GameObject.Find(_changeButtonObj).GetComponent<YinYangChangeButton>();

        // �R�}���h�{�^����Image�R���|�[�l���g�̎擾
        for (int i = 0; i < _commandButtonArray.Length; i++)
        {
            _commandButtonImageArray[i] = _commandButtonArray[0].GetComponent<Image>();
        }

        base.Awake();

    }

    protected override void Update()
    {
        // ����
        if (!_isAllSelect && Input.GetAxis(_inputCancel) > 0 && CanInput)
        {
            CancelCommand();
        }

        // ��x���͂���߂�ƍē��͉\
        if (!_isAllSelect && Input.GetAxisRaw(_inputCancel) == 0)
        {
            CanInput = true;
        }
    }


    /// <summary>
    /// �I�����ꂽ���@�L�����N�^�[���擾
    /// </summary>
    /// <param name="character">�I�����ꂽ�L�����N�^�[</param>
    public void SetCommand(Character character)
    {
        SelectCharacter = character;
    }

    /// <summary>
    /// �v���C���[���I�������R�}���h���Z�b�g����
    /// </summary>
    /// <param name="command">�I�����ꂽ�R�}���h��Object</param>
    public override void SelectCommand(GameObject command, int selectCommandIndex)
    {
        // �R�}���h������܂őI������Ă��Ȃ����
        if (!_isAllSelect)
        {
            // �I�����ꂽ�R�}���h�̏������X�g�ɓo�^
            CommandIdList.Add(selectCommandIndex - 1);
            IsYinList.Add(_yinYangChangeButton.IsYin);

            // �I�����ꂽ�R�}���h��\���̈�ɃZ�b�g����
            base.SelectCommand(command, _selectingCommandSequence);

            // �I�����ꂽ�R�}���h�̐����J�E���g�A�b�v
            _selectingCommandSequence++;
        }

        // �R�}���h�I�𐔂�����ɒB�����Ƃ�
        // �U���J�n
        if (_selectingCommandSequence >= _maxSelectingCommandSequence)
        {
            Debug.Log("�U���J�n");
            CanInput = false;
            _isAllSelect = true;
            //_battleManager.Battle();
        }

    }


    // ���O�̃R�}���h�̑I����������
    private void CancelCommand()
    {
        CanInput = false;

        if (_selectingCommandSequence >= 0)
        {
            // �R�}���h�I�𐔂����炷
            _selectingCommandSequence--;

            // ���O�ɒǉ������R�}���h��\���G���A�������
            SelectCommandImageArray[_selectingCommandSequence].sprite = _nullSprite;
            MindImageArray[_selectingCommandSequence].sprite = _nullSprite;

            // �I�����X�g�����菜��
            CommandIdList.RemoveAt(_selectingCommandSequence);
            IsYinList.RemoveAt(_selectingCommandSequence);
        }
    }
}
