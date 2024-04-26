using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommandButton : Button
{
    // �X�N���v�g
    [SerializeField] private PlayerCommandManager _playerCommandManager;
    [SerializeField] private PlayerCharacterManager _playerCharacterManager;
    private Character _selectCharacter;

    // �R�}���h�{�^��
    private Image _commandImage;      // ���̃R�}���h��Image�R���|�[�l���g
    private int _commandIndex = 0;    // ���̃R�}���h�̏���

    [SerializeField] private GameObject _selectIconObj;
    private Image _selectIconImage;
    private Animator _selectIconAnim;
    private const string _selectBoolName = "Select";
    private Vector3 _selectIconPosition;


    public override void Start()
    {
        // �I�����ꂽ�L�����N�^�[���擾
        _selectCharacter = _playerCharacterManager.SelectCharacter;

        // �R�}���h��Image�R���|�[�l���g�擾
        _commandImage = this.gameObject.GetComponent<Image>();

        // �R�}���h�̏��Ԃ��擾
        _commandIndex = int.Parse(this.gameObject.name);

        // �R�}���h�̉摜���Z�b�g
        _commandImage.sprite = _selectCharacter.CommandSprites[_commandIndex - 1];

        _selectIconImage = _selectIconObj.GetComponent<Image>();
        _selectIconAnim = _selectIconObj.GetComponent<Animator>();
        _selectIconPosition = this.transform.localPosition;
        _selectIconPosition.x -= 10f;
        _selectIconPosition.y -= 80f;
    }

    /// <summary>
    /// �R�}���h���t�H�[�J�X���ꂽ�Ƃ�
    /// </summary>
    public override void Select()
    {
        // �I����Ԃ̉摜��\��
        _commandImage.sprite = _selectCharacter.SelectCommandSprites[_commandIndex - 1];

        _selectIconObj.transform.localPosition = _selectIconPosition;
        _selectIconImage.color += new Color(0f, 0f, 0f, 255f);
        _selectIconAnim.SetBool(_selectBoolName, true);
    }

    /// <summary>
    /// �R�}���h����t�H�[�J�X���O�ꂽ�Ƃ�
    /// </summary>
    public override void Deselect()
    {
        // �ʏ�̉摜��\��
        _commandImage.sprite = _selectCharacter.CommandSprites[_commandIndex - 1];

        _selectIconImage.color -= new Color(0f, 0f, 0f, 255f);
        _selectIconAnim.SetBool(_selectBoolName, false);
    }

    /// <summary>
    /// �R�}���h���I�����ꂽ�Ƃ�
    /// </summary>
    public override void Submit()
    {
        // �R�}���h���I�����ꂽ�������s��
        _playerCommandManager.SelectCommand(_commandIndex);
    }

    public override void PointerEnter(GameObject gameObject)
    {
        base.PointerEnter(gameObject);
    }
}
