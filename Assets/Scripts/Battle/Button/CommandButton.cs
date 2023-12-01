using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandButton : Button
{
    [Header("スクリプト")]
    [SerializeField] private CommandManager _commandManager;

    private Image _commandImg;
    [SerializeField] private Sprite[] _commandSprites = new Sprite[2];


    public override void Start()
    {
        // コマンドのImageコンポーネント取得
        _commandImg = this.gameObject.GetComponent<Image>();
    }

    public override void Select()
    {
        _commandImg.sprite = _commandSprites[0];
    }

    public override void Deselect()
    {
        _commandImg.sprite = _commandSprites[1];
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
