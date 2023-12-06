using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommandButton : Button
{
    [Header("�X�N���v�g")]
    [SerializeField] private CommandManager _commandManager;

    private int _commandIndex = 0;

    // �摜
    private Image _commandImg;


    public override void Start()
    {
        // �R�}���h��Image�R���|�[�l���g�擾
        _commandImg = this.gameObject.GetComponent<Image>();

        // �R�}���h�̏��Ԃ��擾
        _commandIndex = int.Parse(this.gameObject.name);

        if (_commandIndex == 1)
        {
            EventSystem.current.SetSelectedGameObject(this.gameObject);
        }
    }

    public override void Select()
    {
        _commandImg.sprite = _commandManager.SelectCommandSprites[_commandIndex - 1];
    }

    public override void Deselect()
    {
        _commandImg.sprite = _commandManager.CommandSprites[_commandIndex - 1];
    }

    public override void Submit()
    {
        _commandManager.SelectCommand(this.gameObject);
    }

    public override void PointerEnter(GameObject gameObject)
    {
        base.PointerEnter(gameObject);
    }
}
