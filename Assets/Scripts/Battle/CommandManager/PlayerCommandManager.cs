using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCommandManager : CommandManager
{
    // スクリプト
    [SerializeField] private YinYangChangeButton _yinYangChangeButton;
    [SerializeField] private PlayerCharacterManager _playerCharacterManager;
    [SerializeField] private BattleManager _battleManager;

    [Header("コマンドObject")]
    [SerializeField] private GameObject[] _commandButtonArray = new GameObject[5];  // コマンドボタンのObject
    private Image[] _commandButtonImageArray = new Image[5];                        // コマンドボタンのImageコンポーネント
    [SerializeField] private Sprite _nullSprite;                                    // 何も選択していないときの画像

    public int SelectingCommandSequence = 0;        // 選択されたコマンドの数
    private int _maxSelectingCommandSequence = 3;   // 選択できるコマンドの最大数
    public bool IsAllSelect = false;                // コマンドが上限まで選択されたかどうか

    // Input
    public bool CanInput = true;                    // 入力を可能とする制御
    private const string _inputCancel = "Cancel";   // コマンドの取消に対応する入力

    [Header("Audio")]
    [SerializeField] private AudioSource _seAudio;
    [SerializeField] private AudioClip _submitSE;
    [SerializeField] private AudioClip _submitFinishSE;
    [SerializeField] private AudioClip _cancelSE;


    protected override void Awake()
    {
        base.Awake();

        // コマンドボタンのImageコンポーネントの取得
        for (var i = 0; i < _commandButtonArray.Length; i++)
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
        // コマンドが3つセットされていない時
        if (!IsAllSelect)
        {
            _seAudio.PlayOneShot(_submitSE);

            // 選択されたコマンドの情報をリストに登録
            CommandIdList.Add(selectCommandIndex - 1);
            IsYinList.Add(_yinYangChangeButton.IsYin);

            // 選択されたコマンドを表示領域にセットする
            base.SelectCommand(SelectingCommandSequence);
            base.SelectMind(SelectingCommandSequence);

            // 選択されたコマンドの数をカウントアップ
            SelectingCommandSequence++;
        }

        // 攻撃開始
        if (SelectingCommandSequence >= _maxSelectingCommandSequence)
        {
            _seAudio.PlayOneShot(_submitFinishSE);

            CanInput = false;
            IsAllSelect = true;
            StartCoroutine(_battleManager.CoBattleStart());
        }
    }

    /// <summary>
    /// 直前のコマンドの選択を取り消す
    /// </summary>
    public void CancelCommand()
    {
        CanInput = false;

        if (SelectingCommandSequence > 0)
        {
            _seAudio.PlayOneShot(_cancelSE);

            // コマンド選択数を減らす
            SelectingCommandSequence--;

            // 直前に追加したコマンドを表示エリアから消す
            SelectCommandAttributeImageArray[SelectingCommandSequence].sprite = _nullSprite;
            SelectCommandMindImageArray[SelectingCommandSequence].sprite = _nullSprite;

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
        for (var i = 0; i < _maxSelectingCommandSequence; i++)
        {
            SelectCommandAttributeImageArray[i].sprite = _nullSprite;
            SelectCommandMindImageArray[i].sprite = _nullSprite;
        }
    }
}
