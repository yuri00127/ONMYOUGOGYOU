using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommandButton : Button
{
    // スクリプト
    [SerializeField] private PlayerCommandManager _playerCommandManager;
    [SerializeField] private PlayerCharacterManager _playerCharacterManager;
    private Character _selectCharacter;

    // コマンドボタン
    private Image _commandImage;      // このコマンドのImageコンポーネント
    private int _commandIndex = 0;    // このコマンドの順番


    public override void Start()
    {
        // 選択されたキャラクターを取得
        _selectCharacter = _playerCharacterManager.SelectCharacter;

        // コマンドのImageコンポーネント取得
        _commandImage = this.gameObject.GetComponent<Image>();

        // コマンドの順番を取得
        _commandIndex = int.Parse(this.gameObject.name);

        // コマンドの画像をセット
        _commandImage.sprite = _selectCharacter.CommandSprites[_commandIndex - 1];
    }

    /// <summary>
    /// コマンドがフォーカスされたとき
    /// </summary>
    public override void Select()
    {
        // 選択状態の画像を表示
        _commandImage.sprite = _selectCharacter.SelectCommandSprites[_commandIndex - 1];
    }

    /// <summary>
    /// コマンドからフォーカスが外れたとき
    /// </summary>
    public override void Deselect()
    {
        // 通常の画像を表示
        _commandImage.sprite = _selectCharacter.CommandSprites[_commandIndex - 1];
    }

    /// <summary>
    /// コマンドが選択されたとき
    /// </summary>
    public override void Submit()
    {
        // コマンドが選択された処理を行う
        _playerCommandManager.SelectCommand(_commandIndex);
    }

    public override void PointerEnter(GameObject gameObject)
    {
        base.PointerEnter(gameObject);
    }
}
