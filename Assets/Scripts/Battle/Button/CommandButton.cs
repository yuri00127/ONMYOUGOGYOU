using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommandButton : Button
{
    // �X�N���v�g���擾����I�u�W�F�N�g
    private const string _playerCharacterManagerObjName = "PlayerCharacterManager";
    private const string _playerCommandManagerObjName = "PlayerCommandManager";

    // �X�N���v�g
    private PlayerCommandManager _playerCommandManager;

    private Character _selectCharacter; // �I�����ꂽ�L�����N�^�[

    // �R�}���h�{�^��
    private Image _commandImage;      // ���̃R�}���h��Image�R���|�[�l���g
    private int _commandIndex = 0;    // ���̃R�}���h�̏���


    public override void Start()
    {
        // �X�N���v�g�̎擾
        _playerCommandManager = GameObject.Find(_playerCommandManagerObjName).GetComponent<PlayerCommandManager>();

        // �R�}���h��Image�R���|�[�l���g�擾
        _commandImage = this.gameObject.GetComponent<Image>();

        // �R�}���h�̏��Ԃ��擾
        _commandIndex = int.Parse(this.gameObject.name);

        /* ��ԍ��̃R�}���h�{�^�����f�t�H���g�őI������
        if (_commandIndex == 1)
        {
            EventSystem.current.SetSelectedGameObject(this.gameObject);
        }*/

        // �I�����ꂽ�L�����N�^�[���擾
        _selectCharacter = GameObject.Find(_playerCharacterManagerObjName).GetComponent<PlayerCharacterManager>().SelectCharacter;

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
        //Debug.Log("�R�}���h�I��:" + this.gameObject.name);

        // �R�}���h���I�����ꂽ�������s��
        _playerCommandManager.SelectCommand(_commandIndex);
    }

    public override void PointerEnter(GameObject gameObject)
    {
        base.PointerEnter(gameObject);
    }
}
