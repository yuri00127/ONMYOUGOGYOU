using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AICommandManager : CommandManager
{
    // スクリプト
    [SerializeField] private AICharacterManager _aiCharacterManager;
    [SerializeField] private SelectCharacterData _selectCharacterData;
    [SerializeField] private CharacterDataBase _characterDataBase;

    // 表示
    [SerializeField] private Sprite _unknownCommandSprite;    // 不明なコマンドのSprite
    [SerializeField] private Sprite _unknownMindSprite;       // 不明な気のSprite
    private int _showCommandNumber;                           // レベル別の表示するコマンドの数
    private int _showMindNumber;                              // レベル別の表示する気の数

    // コマンド選択
    private const int _minCommandAttributeRange = 0;            // 属性IDの範囲の最小値
    private const int _maxCommandAttributeRange = 5;            // 属性IDの範囲の最大値
    private int _aiAttributeId;                                 // 敵の属性ID
    private const int _maxAiLevel = 3;                          // 敵AIの最大レベル
    private bool _isFirstRound = true;                          // 最初のターンかどうか
    private int[] commandArray = new int[] { 1, 1, 1, 1, 1 };   // コマンドの重みづけ


    protected override void Awake()
    {
        base.Awake();

        // 敵の属性を取得
        int aiCharacterId = PlayerPrefs.GetInt(_selectCharacterData.SaveAICharacterId);
        _aiAttributeId = _characterDataBase.CharacterList[aiCharacterId - 1].AttributeId;
    }

    private void Start()
    {
        // 選択された敵キャラクターを取得
        SelectCharacter = _aiCharacterManager.SelectCharacter;

        // レベルにごとのコマンド表示数を取得
        ShowCommandCheck();

        // 1ターン目のコマンドを決定
        ShowAICommand();
    }

    /// <summary>
    /// AIが選んだコマンドを表示する
    /// </summary>
    public void ShowAICommand()
    {
        // 全ての表示をリセット
        for (var i = 0; i < SelectCommandAttributeObjArray.Length; i++)
        {
            SelectCommandAttributeImageArray[i].sprite = _unknownCommandSprite;
            SelectCommandMindImageArray[i].sprite = _unknownMindSprite;
        }

        // 選択リストをリセット
        CommandIdList.Clear();
        IsYinList.Clear();

        // 敵のコマンドを決定
        var selectCommandIndex = 0;

        for (var i = 0; i < SelectCommandAttributeObjArray.Length; i++)
        {
            selectCommandIndex = SetAICommand(_aiCharacterManager.AILevel);
            CommandIdList.Add(selectCommandIndex);
        }

        // 気を決定(0なら陰、1なら陽)
        for (var i = 0; i < SelectCommandAttributeObjArray.Length; i++)
        {
            if (Random.Range(0, 2) == 0)
            {
                IsYinList.Add(true);
            }
            IsYinList.Add(false);
        }

        // コマンドの表示
        List<int> showCommandPositionList = ShowPositionDecide(_showCommandNumber);

        for (var i = 0; i < showCommandPositionList.Count; i++)
        {
            base.SelectCommand(showCommandPositionList[i]);
        }

        // 気の表示
        List<int> showMindPositionList = ShowPositionDecide(_showMindNumber);

        for (var i = 0; i < showMindPositionList.Count; i++)
        {
            base.SelectMind(showMindPositionList[i]);
        }
    }

    /// <summary>
    /// AIレベルによって、コマンドを表示する量を変更
    /// </summary>
    private void ShowCommandCheck()
    {
        if (_aiCharacterManager.AILevel == 1)
        {
            _showCommandNumber = 2;
            _showMindNumber = 2;
            return;
        }

        if (_aiCharacterManager.AILevel == 2)
        {
            _showCommandNumber = 1;
            _showMindNumber = 1;
            return;
        }

        if (_aiCharacterManager.AILevel == _maxAiLevel)
        {
            _showCommandNumber = 1;
            _showMindNumber = 0;
            return;
        }
    }

    /// <summary>
    ///  コマンドの表示位置を決める
    /// </summary>
    /// <param name="decideCount">表示する個数</param>
    /// <returns>表示位置のリスト</returns>
    private List<int> ShowPositionDecide(int decideCount)
    {
        List<int> _positionIndex = new List<int>() { 0, 1, 2 };
        List<int> _returnPositionIndex = new List<int>() { };

        // 位置決め
        for (var i = 0; i < decideCount; i++)
        {
            int num = Random.Range(0, _positionIndex.Count);
            _returnPositionIndex.Add(_positionIndex[num]);
            _positionIndex.RemoveAt(num);
        }

        return _returnPositionIndex;
    }

    /// <summary>
    /// 敵のコマンド決定
    /// </summary>
    /// <param name="aiLevel">敵AIのレベル</param>
    /// <returns>決定したコマンドのindex</returns>
    private int SetAICommand(int aiLevel)
    {
        // AIレベル3(初回のみ処理)
        if (_isFirstRound)
        {
            if (aiLevel == _maxAiLevel)
            {
                switch (_aiAttributeId)
                {
                    // 水
                    case 1:
                        commandArray[0] = 3;
                        break;
                    // 木
                    case 2:
                        commandArray[1] = 3;
                        break;
                    // 火
                    case 3:
                        commandArray[2] = 3;
                        break;
                    // 土
                    case 4:
                        commandArray[3] = 3;
                        break;
                    // 金
                    case 5:
                        commandArray[4] = 3;
                        break;
                    default:
                        break;
                }
            }
        }

        // AIレベル3
        var total = 0.0f;

        foreach (float elem in commandArray)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < commandArray.Length; i++)
        {
            if (randomPoint < commandArray[i])
            {
                return i;
            }

            randomPoint -= commandArray[i];
        }

        // AIレベル1,2
        return Random.Range(_minCommandAttributeRange, _maxCommandAttributeRange);
    }

}
