using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCommandManager : CommandManager
{
    // �X�N���v�g���擾����I�u�W�F�N�g
    private const string _changeButtonObjName = "ChangeButton";
    private const string _playerCharacterManagerObjName = "PlayerCharacterManager";

    // �X�N���v�g
    private YinYangChangeButton _yinYangChangeButton;
    private PlayerCharacterManager _playerCharacterManager;

    [Header("�R�}���hObject")]
    [SerializeField] private GameObject[] _commandButtonArray = new GameObject[5];  // �R�}���h�{�^����Object
    private Image[] _commandButtonImageArray = new Image[5];                        // �R�}���h�{�^����Image�R���|�[�l���g
    private const string _playerSelectCommandObjName = "PlayerCommands";            // �I�������R�}���h�̕\���̈�̖��O
    [SerializeField] private Sprite _nullSprite;                                    // �����I�����Ă��Ȃ��Ƃ��̉摜

    public int SelectingCommandSequence = 0;      // �I�����ꂽ�R�}���h�̐�
    private int _maxSelectingCommandSequence = 3;   // �I���ł���R�}���h�̍ő吔
    public bool IsAllSelect = false;              // �R�}���h������܂őI�����ꂽ���ǂ���

    // Input
    public bool CanInput = true;                 // ���͂��\�Ƃ��鐧��
    private const string _inputCancel = "Cancel";   // �R�}���h�̎���ɑΉ��������


    protected override void Awake()
    {
        // �X�N���v�g�̎擾
        _yinYangChangeButton = GameObject.Find(_changeButtonObjName).GetComponent<YinYangChangeButton>();
        _playerCharacterManager = GameObject.Find(_playerCharacterManagerObjName).GetComponent<PlayerCharacterManager>();

        // �\���̈��Object�擾
        SelectCommandObj = GameObject.Find(_playerSelectCommandObjName);
        base.Awake();

        // �R�}���h�{�^����Image�R���|�[�l���g�̎擾
        for (int i = 0; i < _commandButtonArray.Length; i++)
        {
            _commandButtonImageArray[i] = _commandButtonArray[0].GetComponent<Image>();
        }
    }

    private void Start()
    {
        // �I�����ꂽ���@�L�����N�^�[���擾
        SelectCharacter = _playerCharacterManager.SelectCharacter;
    }

    protected override void Update()
    {
        // ����
        if (!IsAllSelect && Input.GetAxis(_inputCancel) > 0 && CanInput)
        {
            CancelCommand();
        }

        // ��x���͂���߂�ƍē��͉\
        if (!IsAllSelect && Input.GetAxisRaw(_inputCancel) == 0)
        {
            CanInput = true;
        }
    }

    /// <summary>
    /// �v���C���[���I�������R�}���h���Z�b�g����
    /// </summary>
    /// <param name="command">�I�����ꂽ�R�}���h��Object</param>
    public override void SelectCommand(int selectCommandIndex)
    {
        // �R�}���h������܂őI������Ă��Ȃ����
        if (!IsAllSelect)
        {
            // �I�����ꂽ�R�}���h�̏������X�g�ɓo�^
            CommandIdList.Add(selectCommandIndex - 1);
            IsYinList.Add(_yinYangChangeButton.IsYin);

            // �I�����ꂽ�R�}���h��\���̈�ɃZ�b�g����
            base.SelectCommand(SelectingCommandSequence);
            base.SelectMind(SelectingCommandSequence);

            // �I�����ꂽ�R�}���h�̐����J�E���g�A�b�v
            SelectingCommandSequence++;
        }

        // �R�}���h�I�𐔂�����ɒB�����Ƃ�
        // �U���J�n
        if (SelectingCommandSequence >= _maxSelectingCommandSequence)
        {
            //Debug.Log("�U���J�n");

            CanInput = false;
            IsAllSelect = true;
            _battleManager.Battle();
        }

    }


    // ���O�̃R�}���h�̑I����������
    private void CancelCommand()
    {
        CanInput = false;

        if (SelectingCommandSequence >= 0)
        {
            // �R�}���h�I�𐔂����炷
            SelectingCommandSequence--;

            // ���O�ɒǉ������R�}���h��\���G���A�������
            SelectCommandImageArray[SelectingCommandSequence].sprite = _nullSprite;
            MindImageArray[SelectingCommandSequence].sprite = _nullSprite;

            // �I�����X�g�����菜��
            CommandIdList.RemoveAt(SelectingCommandSequence);
            IsYinList.RemoveAt(SelectingCommandSequence);
        }
    }

    /// <summary>
    /// �\������Ă���R�}���h��S�ă��Z�b�g����
    /// </summary>
    public void CommandReset()
    {
        for (int i = 0; i < _maxSelectingCommandSequence; i++)
        {
            SelectCommandImageArray[i].sprite = _nullSprite;
            MindImageArray[i].sprite = _nullSprite;
        }
    }
}
