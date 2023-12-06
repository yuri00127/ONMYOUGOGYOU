using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandManager : MonoBehaviour
{
    [SerializeField] private YinYangChangeButton _yinYangChangeButton;
    private Character _playerCharacter;

    [Header("�R�}���h")]
    [SerializeField] private GameObject[] _commandArray = new GameObject[5];
    private Image[] _commandImageArray = new Image[5];
    private int _isSelectingCommandSequence = 0;
    public Sprite[] CommandSprites { get; private set; } = new Sprite[5];
    public Sprite[] SelectCommandSprites { get; private set; } = new Sprite[5];

    [SerializeField] private GameObject[] _playerCommands = new GameObject[3];
    private const string _mindObjName = "Mind";

    [Header("�I�����ꂽ�R�}���h")]
    private List<int> _commandIdList = new List<int>();
    private List<bool> _isYinList = new List<bool>();    // �A���ǂ���
    private bool _isAllSelect = false;

    [SerializeField] private Sprite _nullSprite;

    private bool _canInput = true;
    private const string _inputCancel = "Cancel";


    // Update is called once per frame
    void Update()
    {
        // ����
        if (!_isAllSelect && Input.GetAxis(_inputCancel) > 0 && _canInput)
        {
            CancelCommand();
        }

        // ��x���͂���߂�ƍē��͉\
        if (!_isAllSelect && Input.GetAxisRaw(_inputCancel) == 0)
        {
            _canInput = true;
        }
    }

    // �I�����ꂽ���@�L�����N�^�[�ɑΉ������R�}���h���Z�b�g
    public void SetCommand(Character character)
    {
        _playerCharacter = character;

        // �R�}���h��Image�R���|�[�l���g��Sprite���Z�b�g
        for (int i = 0; i < _commandArray.Length; i++)
        {
            _commandArray[i].GetComponent<Image>().sprite = _playerCharacter.CommandSprites[i];
        }

        // �L�����N�^�[���Ƃ̃R�}���h�摜���擾
        for (int i = 0; i < _commandArray.Length; i++)
        {
            CommandSprites[i] = character.CommandSprites[i];
            SelectCommandSprites[i] = character.SelectCommandSprites[i];
        }
    }

    // �I�����ꂽ�R�}���h��I���ς݂ɂ���
    public void SelectCommand(GameObject command)
    {
        if (!_isAllSelect)
        {
            _commandIdList.Add(int.Parse(command.name));
            _isYinList.Add(_yinYangChangeButton.IsYin);

            // �I�������R�}���h�̉摜��ݒ�
            _playerCommands[_isSelectingCommandSequence].GetComponent<Image>().sprite
                = command.GetComponent<Image>().sprite;

            // �Ή�����A�z�̉摜��ݒ�
            _playerCommands[_isSelectingCommandSequence].transform.Find(_mindObjName).GetComponent<Image>().sprite
                = _yinYangChangeButton.GetComponent<Image>().sprite;

            _isSelectingCommandSequence++;
        }

        // �U���J�n
        if (_isSelectingCommandSequence >= 3)
        {
            _canInput = false;
            _isAllSelect = true;
        }
    }

    // ���O�̃R�}���h�̑I����������
    private void CancelCommand()
    {
        _canInput = false;

        if (_isSelectingCommandSequence >= 0)
        {
            _isSelectingCommandSequence--;

            _playerCommands[_isSelectingCommandSequence].GetComponent<Image>().sprite = _nullSprite;
            _playerCommands[_isSelectingCommandSequence].transform.Find(_mindObjName).GetComponent<Image>().sprite = _nullSprite;
        }
    }
}
