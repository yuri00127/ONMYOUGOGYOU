using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandManager : MonoBehaviour
{
    [SerializeField] private YinYangChangeButton _yinYangChangeButton;
    private Character _playerCharacter;

    [Header("コマンド")]
    [SerializeField] private GameObject[] _commandArray = new GameObject[5];
    private Image[] _commandImageArray = new Image[5];
    private int _isSelectingCommandSequence = 0;
    public Sprite[] CommandSprites { get; private set; } = new Sprite[5];
    public Sprite[] SelectCommandSprites { get; private set; } = new Sprite[5];

    [SerializeField] private GameObject[] _playerCommands = new GameObject[3];
    private const string _mindObjName = "Mind";

    [Header("選択されたコマンド")]
    private List<int> _commandIdList = new List<int>();
    private List<bool> _isYinList = new List<bool>();    // 陰かどうか
    private bool _isAllSelect = false;

    [SerializeField] private Sprite _nullSprite;

    private bool _canInput = true;
    private const string _inputCancel = "Cancel";


    // Update is called once per frame
    void Update()
    {
        // 入力
        if (!_isAllSelect && Input.GetAxis(_inputCancel) > 0 && _canInput)
        {
            CancelCommand();
        }

        // 一度入力をやめると再入力可能
        if (!_isAllSelect && Input.GetAxisRaw(_inputCancel) == 0)
        {
            _canInput = true;
        }
    }

    // 選択された自機キャラクターに対応したコマンドをセット
    public void SetCommand(Character character)
    {
        _playerCharacter = character;

        // コマンドのImageコンポーネントにSpriteをセット
        for (int i = 0; i < _commandArray.Length; i++)
        {
            _commandArray[i].GetComponent<Image>().sprite = _playerCharacter.CommandSprites[i];
        }

        // キャラクターごとのコマンド画像を取得
        for (int i = 0; i < _commandArray.Length; i++)
        {
            CommandSprites[i] = character.CommandSprites[i];
            SelectCommandSprites[i] = character.SelectCommandSprites[i];
        }
    }

    // 選択されたコマンドを選択済みにする
    public void SelectCommand(GameObject command)
    {
        if (!_isAllSelect)
        {
            _commandIdList.Add(int.Parse(command.name));
            _isYinList.Add(_yinYangChangeButton.IsYin);

            // 選択したコマンドの画像を設定
            _playerCommands[_isSelectingCommandSequence].GetComponent<Image>().sprite
                = command.GetComponent<Image>().sprite;

            // 対応する陰陽の画像を設定
            _playerCommands[_isSelectingCommandSequence].transform.Find(_mindObjName).GetComponent<Image>().sprite
                = _yinYangChangeButton.GetComponent<Image>().sprite;

            _isSelectingCommandSequence++;
        }

        // 攻撃開始
        if (_isSelectingCommandSequence >= 3)
        {
            _canInput = false;
            _isAllSelect = true;
        }
    }

    // 直前のコマンドの選択を取り消す
    private void CancelCommand()
    {
        _canInput = false;

        if (_isSelectingCommandSequence >= 0)
        {
            _isSelectingCommandSequence--;

            _playerCommands[_isSelectingCommandSequence].GetComponent<Image>().sprite = _nullSprite;
            _playerCommands[_isSelectingCommandSequence].transform.Find(_mindObjName).GetComponent<Image>().sprite = _nullSprite;
        }
    }
}
