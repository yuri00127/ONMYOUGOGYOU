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

    [SerializeField] private GameObject _selectIconObj;
    private Image _selectIconImage;
    private Animator _selectIconAnim;
    private const string _selectBoolName = "Select";
    private Vector3 _selectIconPosition;


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

        _selectIconImage = _selectIconObj.GetComponent<Image>();
        _selectIconAnim = _selectIconObj.GetComponent<Animator>();
        _selectIconPosition = this.transform.localPosition;
        _selectIconPosition.x -= 10f;
        _selectIconPosition.y -= 80f;
    }

    /// <summary>
    /// コマンドがフォーカスされたとき
    /// </summary>
    public override void Select()
    {
        // 選択状態の画像を表示
        _commandImage.sprite = _selectCharacter.SelectCommandSprites[_commandIndex - 1];

        _selectIconObj.transform.localPosition = _selectIconPosition;
        _selectIconImage.color += new Color(0f, 0f, 0f, 255f);
        _selectIconAnim.SetBool(_selectBoolName, true);
    }

    /// <summary>
    /// コマンドからフォーカスが外れたとき
    /// </summary>
    public override void Deselect()
    {
        // 通常の画像を表示
        _commandImage.sprite = _selectCharacter.CommandSprites[_commandIndex - 1];

        _selectIconImage.color -= new Color(0f, 0f, 0f, 255f);
        _selectIconAnim.SetBool(_selectBoolName, false);
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
