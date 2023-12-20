using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCommandManager : CommandManager
{
    // スクリプトを取得するオブジェクト
    private const string _changeButtonObjName = "ChangeButton";
    private const string _playerCharacterManagerObjName = "PlayerCharacterManager";

    // スクリプト
    private YinYangChangeButton _yinYangChangeButton;
    private PlayerCharacterManager _playerCharacterManager;

    [Header("コマンドObject")]
    [SerializeField] private GameObject[] _commandButtonArray = new GameObject[5];  // コマンドボタンのObject
    private Image[] _commandButtonImageArray = new Image[5];                        // コマンドボタンのImageコンポーネント
    private const string _playerSelectCommandObjName = "PlayerCommands";            // 選択したコマンドの表示領域の名前
    [SerializeField] private Sprite _nullSprite;                                    // 何も選択していないときの画像

    public int SelectingCommandSequence = 0;      // 選択されたコマンドの数
    private int _maxSelectingCommandSequence = 3;   // 選択できるコマンドの最大数
    public bool IsAllSelect = false;              // コマンドが上限まで選択されたかどうか

    // Input
    public bool CanInput = true;                 // 入力を可能とする制御
    private const string _inputCancel = "Cancel";   // コマンドの取消に対応する入力


    protected override void Awake()
    {
        // スクリプトの取得
        _yinYangChangeButton = GameObject.Find(_changeButtonObjName).GetComponent<YinYangChangeButton>();
        _playerCharacterManager = GameObject.Find(_playerCharacterManagerObjName).GetComponent<PlayerCharacterManager>();

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
        if (!IsAllSelect && Input.GetAxis(_inputCancel) > 0 && CanInput)
        {
            CancelCommand();
        }

        // 一度入力をやめると再入力可能
        if (!IsAllSelect && Input.GetAxisRaw(_inputCancel) == 0)
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
        if (!IsAllSelect)
        {
            // 選択されたコマンドの情報をリストに登録
            CommandIdList.Add(selectCommandIndex - 1);
            IsYinList.Add(_yinYangChangeButton.IsYin);

            // 選択されたコマンドを表示領域にセットする
            base.SelectCommand(SelectingCommandSequence);
            base.SelectMind(SelectingCommandSequence);

            // 選択されたコマンドの数をカウントアップ
            SelectingCommandSequence++;
        }

        // コマンド選択数が上限に達したとき
        // 攻撃開始
        if (SelectingCommandSequence >= _maxSelectingCommandSequence)
        {
            //Debug.Log("攻撃開始");

            CanInput = false;
            IsAllSelect = true;
            _battleManager.Battle();
        }

    }


    // 直前のコマンドの選択を取り消す
    private void CancelCommand()
    {
        CanInput = false;

        if (SelectingCommandSequence >= 0)
        {
            // コマンド選択数を減らす
            SelectingCommandSequence--;

            // 直前に追加したコマンドを表示エリアから消す
            SelectCommandImageArray[SelectingCommandSequence].sprite = _nullSprite;
            MindImageArray[SelectingCommandSequence].sprite = _nullSprite;

            // 選択リストから取り除く
            CommandIdList.RemoveAt(SelectingCommandSequence);
            IsYinList.RemoveAt(SelectingCommandSequence);
        }
    }

    /// <summary>
    /// 表示されているコマンドを全てリセットする
    /// </summary>
    public void CommandReset()
    {
        for (int i = 0; i < _maxSelectingCommandSequence; i++)
        {
            SelectCommandImageArray[i].sprite = _nullSprite;
            MindImageArray[i].sprite = _nullSprite;
        }
    }
}
