using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCommandManager : CommandManager
{
    // �X�N���v�g
    [SerializeField] private YinYangChangeButton _yinYangChangeButton;
    [SerializeField] private PlayerCharacterManager _playerCharacterManager;
    [SerializeField] private BattleManager _battleManager;

    [Header("�R�}���hObject")]
    [SerializeField] private GameObject[] _commandButtonArray = new GameObject[5];  // �R�}���h�{�^����Object
    private Image[] _commandButtonImageArray = new Image[5];                        // �R�}���h�{�^����Image�R���|�[�l���g
    [SerializeField] private Sprite _nullSprite;                                    // �����I�����Ă��Ȃ��Ƃ��̉摜

    public int SelectingCommandSequence = 0;        // �I�����ꂽ�R�}���h�̐�
    private int _maxSelectingCommandSequence = 3;   // �I���ł���R�}���h�̍ő吔
    public bool IsAllSelect = false;                // �R�}���h������܂őI�����ꂽ���ǂ���

    // Input
    public bool CanInput = true;                    // ���͂��\�Ƃ��鐧��
    private const string _inputCancel = "Cancel";   // �R�}���h�̎���ɑΉ��������

    [Header("Audio")]
    [SerializeField] private AudioSource _seAudio;
    [SerializeField] private AudioClip _submitSE;
    [SerializeField] private AudioClip _submitFinishSE;
    [SerializeField] private AudioClip _cancelSE;


    protected override void Awake()
    {
        base.Awake();

        // �R�}���h�{�^����Image�R���|�[�l���g�̎擾
        for (var i = 0; i < _commandButtonArray.Length; i++)
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
        // �R�}���h��3�Z�b�g����Ă��Ȃ���
        if (!IsAllSelect)
        {
            _seAudio.PlayOneShot(_submitSE);

            // �I�����ꂽ�R�}���h�̏������X�g�ɓo�^
            CommandIdList.Add(selectCommandIndex - 1);
            IsYinList.Add(_yinYangChangeButton.IsYin);

            // �I�����ꂽ�R�}���h��\���̈�ɃZ�b�g����
            base.SelectCommand(SelectingCommandSequence);
            base.SelectMind(SelectingCommandSequence);

            // �I�����ꂽ�R�}���h�̐����J�E���g�A�b�v
            SelectingCommandSequence++;
        }

        // �U���J�n
        if (SelectingCommandSequence >= _maxSelectingCommandSequence)
        {
            _seAudio.PlayOneShot(_submitFinishSE);

            CanInput = false;
            IsAllSelect = true;
            StartCoroutine(_battleManager.CoBattleStart());
        }
    }

    /// <summary>
    /// ���O�̃R�}���h�̑I����������
    /// </summary>
    public void CancelCommand()
    {
        CanInput = false;

        if (SelectingCommandSequence > 0)
        {
            _seAudio.PlayOneShot(_cancelSE);

            // �R�}���h�I�𐔂����炷
            SelectingCommandSequence--;

            // ���O�ɒǉ������R�}���h��\���G���A�������
            SelectCommandAttributeImageArray[SelectingCommandSequence].sprite = _nullSprite;
            SelectCommandMindImageArray[SelectingCommandSequence].sprite = _nullSprite;

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
        for (var i = 0; i < _maxSelectingCommandSequence; i++)
        {
            SelectCommandAttributeImageArray[i].sprite = _nullSprite;
            SelectCommandMindImageArray[i].sprite = _nullSprite;
        }
    }
}
