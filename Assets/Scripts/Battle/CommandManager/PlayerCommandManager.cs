using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCommandManager : CommandManager
{
    // スクリプトを取得するオブジェクト
    private const string _changeButtonObj = "ChangeButton";

    // スクリプト
    private YinYangChangeButton _yinYangChangeButton;

    [Header("コマンドObject")]
    [SerializeField] private GameObject[] _commandButtonArray = new GameObject[5];  // コマンドボタンのObject
    private Image[] _commandButtonImageArray = new Image[5];                        // コマンドボタンのImageコンポーネント
    private const string _playerSelectCommandObjName = "PlayerCommands";            // 選択したコマンドの表示領域の名前
    [SerializeField] private Sprite _nullSprite;                                    // 何も選択していないときの画像

    private int _selectingCommandSequence = 0;      // 選択されたコマンドの数
    private int _maxSelectingCommandSequence = 3;   // 選択できるコマンドの最大数
    private bool _isAllSelect = false;              // コマンドが上限まで選択されたかどうか

    // Input
    protected bool CanInput = true;                 // 入力を可能とする制御
    private const string _inputCancel = "Cancel";   // コマンドの取消に対応する入力


    protected override void Awake()
    {
        // スクリプトの取得
        _yinYangChangeButton = GameObject.Find(_changeButtonObj).GetComponent<YinYangChangeButton>();

        // コマンドボタンのImageコンポーネントの取得
        for (int i = 0; i < _commandButtonArray.Length; i++)
        {
            _commandButtonImageArray[i] = _commandButtonArray[0].GetComponent<Image>();
        }

        base.Awake();

    }

    protected override void Update()
    {
        // 入力
        if (!_isAllSelect && Input.GetAxis(_inputCancel) > 0 && CanInput)
        {
            CancelCommand();
        }

        // 一度入力をやめると再入力可能
        if (!_isAllSelect && Input.GetAxisRaw(_inputCancel) == 0)
        {
            CanInput = true;
        }
    }


    /// <summary>
    /// 選択された自機キャラクターを取得
    /// </summary>
    /// <param name="character">選択されたキャラクター</param>
    public void SetCommand(Character character)
    {
        SelectCharacter = character;
    }

    /// <summary>
    /// プレイヤーが選択したコマンドをセットする
    /// </summary>
    /// <param name="command">選択されたコマンドのObject</param>
    public override void SelectCommand(GameObject command, int selectCommandIndex)
    {
        // コマンドが上限まで選択されていなければ
        if (!_isAllSelect)
        {
            // 選択されたコマンドの情報をリストに登録
            CommandIdList.Add(selectCommandIndex - 1);
            IsYinList.Add(_yinYangChangeButton.IsYin);

            // 選択されたコマンドを表示領域にセットする
            base.SelectCommand(command, _selectingCommandSequence);

            // 選択されたコマンドの数をカウントアップ
            _selectingCommandSequence++;
        }

        // コマンド選択数が上限に達したとき
        // 攻撃開始
        if (_selectingCommandSequence >= _maxSelectingCommandSequence)
        {
            Debug.Log("攻撃開始");
            CanInput = false;
            _isAllSelect = true;
            //_battleManager.Battle();
        }

    }


    // 直前のコマンドの選択を取り消す
    private void CancelCommand()
    {
        CanInput = false;

        if (_selectingCommandSequence >= 0)
        {
            // コマンド選択数を減らす
            _selectingCommandSequence--;

            // 直前に追加したコマンドを表示エリアから消す
            SelectCommandImageArray[_selectingCommandSequence].sprite = _nullSprite;
            MindImageArray[_selectingCommandSequence].sprite = _nullSprite;

            // 選択リストから取り除く
            CommandIdList.RemoveAt(_selectingCommandSequence);
            IsYinList.RemoveAt(_selectingCommandSequence);
        }
    }
}
