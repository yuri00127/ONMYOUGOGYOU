using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCommandManager : CommandManager
{
    // スクリプトを取得するオブジェクト
    private const string _changeButtonObjName = "ChangeButton";
    private const string _playerCharacterManagerObjName = "PlayerCharacterManager";
    private const string _battleManagerObjName = "BattleManager";

    // スクリプト
    private YinYangChangeButton _yinYangChangeButton;
    private PlayerCharacterManager _playerCharacterManager;
    private BattleManager _battleManager;

    [Header("コマンドObject")]
    [SerializeField] private GameObject[] _commandButtonArray = new GameObject[5];  // コマンドボタンのObject
    private Image[] _commandButtonImageArray = new Image[5];                        // コマンドボタンのImageコンポーネント
    private const string _playerSelectCommandObjName = "PlayerCommands";            // 選択したコマンドの表示領域の名前
    [SerializeField] private Sprite _nullSprite;                                    // 何も選択していないときの画像

    private int _selectingCommandSequence = 0;      // 選択されたコマンドの数
    private int _maxSelectingCommandSequence = 3;   // 選択できるコマンドの最大数
    private bool _isAllSelect = false;              // コマンドが上限まで選択されたかどうか

    // Input
    public bool CanInput = true;                 // 入力を可能とする制御
    private const string _inputCancel = "Cancel";   // コマンドの取消に対応する入力


    protected override void Awake()
    {
        // スクリプトの取得
        _yinYangChangeButton = GameObject.Find(_changeButtonObjName).GetComponent<YinYangChangeButton>();
        _playerCharacterManager = GameObject.Find(_playerCharacterManagerObjName).GetComponent<PlayerCharacterManager>();
        _battleManager = GameObject.Find(_battleManagerObjName).GetComponent<BattleManager>();

        // 表示領域のObject取得
        SelectCommandObj = GameObject.Find(_playerSelectCommandObjName);
        base.Awake();

        // コマンドボタンのImageコンポーネントの取得
        for (int i = 0; i < _commandButtonArray.Length; i++)
        {
            _commandButtonImageArray[i] = _commandButtonArray[0].GetComponent<Image>();
        }
    }

    private void Start()
    {
        // 選択された自機キャラクターを取得
        SelectCharacter = _playerCharacterManager.SelectCharacter;
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
    /// プレイヤーが選択したコマンドをセットする
    /// </summary>
    /// <param name="command">選択されたコマンドのObject</param>
    public override void SelectCommand(int selectCommandIndex)
    {
        // コマンドが上限まで選択されていなければ
        if (!_isAllSelect)
        {
            // 選択されたコマンドの情報をリストに登録
            CommandIdList.Add(selectCommandIndex - 1);
            IsYinList.Add(_yinYangChangeButton.IsYin);

            // 選択されたコマンドを表示領域にセットする
            base.SelectCommand(_selectingCommandSequence);
            base.SelectMind(_selectingCommandSequence);

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
            _battleManager.Battle();
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
