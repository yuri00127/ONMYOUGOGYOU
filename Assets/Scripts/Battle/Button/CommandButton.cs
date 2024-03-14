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
    }

    /// <summary>
    /// �R�}���h���t�H�[�J�X���ꂽ�Ƃ�
    /// </summary>
    public override void Select()
    {
        // �I����Ԃ̉摜��\��
        _commandImage.sprite = _selectCharacter.SelectCommandSprites[_commandIndex - 1];
    }

    /// <summary>
    /// �R�}���h����t�H�[�J�X���O�ꂽ�Ƃ�
    /// </summary>
    public override void Deselect()
    {
        // �ʏ�̉摜��\��
        _commandImage.sprite = _selectCharacter.CommandSprites[_commandIndex - 1];
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
