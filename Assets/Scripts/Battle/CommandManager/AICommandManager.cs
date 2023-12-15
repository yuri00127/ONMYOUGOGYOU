using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AICommandManager : CommandManager
{
    // スクリプトを取得するオブジェクト
    private const string _aiCharacterManagerObjName = "AICharacterManager";

    // スクリプト
    private AICharacterManager _aiCharacterManager;

    // 表示
    private const string _aiSelectCommandObjName = "AICommands";        // 選択したコマンドの表示領域の名前
    [SerializeField] private Sprite _unknownSprite;                     // 不明なコマンドのSprite
    private int _showCommandNumber;                                     // レベル別の表示するコマンドの数
    private int _showMindNumber;                                        // レベル別の表示する気の数

    // コマンド選択
    private const int _startCommandRange = 0;   // コマンドの範囲の最小値
    private const int _endCommandRange = 5;     // コマンドの範囲の最大値


    protected override void Awake()
    {
        // 表示領域のObject取得
        SelectCommandObj = GameObject.Find(_aiSelectCommandObjName);
        base.Awake();

        // スクリプトを取得
        _aiCharacterManager = GameObject.Find(_aiCharacterManagerObjName).GetComponent<AICharacterManager>();
    }

    private void Start()
    {
        // 選択された敵キャラクターを取得
        SelectCharacter = _aiCharacterManager.SelectCharacter;

        // レベルにごとのコマンド表示数を取得
        ShowCommandCheck();

        // 1ターン目のコマンドを決定
        SetAICommand();
    }

    public void SetAICommand()
    {
        // 全ての表示をリセット
        for (int i = 0; i < SelectCommandObjArray.Length; i++)
        {
            SelectCommandImageArray[i].sprite = _unknownSprite;
            MindImageArray[i].sprite = _unknownSprite;
        }

        // 選択リストをリセット
        CommandIdList.Clear();
        IsYinList.Clear();

        // 敵のコマンドを決定
        int selectCommandIndex = 0;
        for (int i = 0; i < SelectCommandObjArray.Length; i++)
        {
            selectCommandIndex = Random.Range(_startCommandRange, _endCommandRange);
            CommandIdList.Add(selectCommandIndex);

            //Debug.Log("敵のコマンド：" + selectCommandIndex);
        }

        // 気を決定(0なら陰、1なら陽)
        for (int i = 0; i < SelectCommandObjArray.Length; i++)
        {
            if (Random.Range(0, 2) == 0)
            {
                IsYinList.Add(true);
            }
            IsYinList.Add(false);

            Debug.Log("敵の陰陽：" + IsYinList[i]);
        }

        // コマンドの表示
        List<int> showCommandPositionList = ShowPositionDecide(_showCommandNumber);

        for (int i = 0; i < showCommandPositionList.Count; i++)
        {
            base.SelectCommand(showCommandPositionList[i]);
        }

        // 気の表示
        List<int> showMindPositionList = ShowPositionDecide(_showMindNumber);

        for (int i = 0; i < showMindPositionList.Count; i++)
        {
            base.SelectMind(showMindPositionList[i]);
        }
    }

    // AIレベルによって表示する量を変更
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

        if (_aiCharacterManager.AILevel == 3)
        {
            _showCommandNumber = 1;
            _showMindNumber = 0;
            return;
        }
    }

    // 表示位置を決める
    private List<int> ShowPositionDecide(int decideCount)
    {
        List<int> _positionIndex = new List<int>() { 0, 1, 2 };
        List<int> _returnPositionIndex = new List<int>() { };

        // 表示する個数分の位置決め
        for (int i = 0; i < decideCount; i++)
        {
            int num = Random.Range(0, _positionIndex.Count);
            _returnPositionIndex.Add(_positionIndex[num]);
            _positionIndex.RemoveAt(num);

            //Debug.Log("位置" + num);
        }

        return _returnPositionIndex;
    }
}
