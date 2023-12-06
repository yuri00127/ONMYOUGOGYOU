using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AICharacterManager : CharacterManager
{
    private int _aiLevel;
    private const int _startCommandRange = 0;
    private const int _endCommandRange = 5;

    [Header("コマンド表示")]
    [SerializeField] private GameObject[] _commandObjArray = new GameObject[3];
    private Image[] _commandImageArray = new Image[3];
    private Image[] _mindImageArray = new Image[3];
    private const string _mindObjName = "Mind";
    [SerializeField] private Sprite[] _yinYanSprites = new Sprite[2];
    [SerializeField] private Sprite _unknownSprite;
    private int _showCommandNumber;     // 表示するコマンドの数
    private int _showMindNumber;        // 表示する気の数

    // コマンドの決定
    private int[] _selectCommandIndexArray = new int[3];
    private int[] _selectMindIndexArray = new int[3];


    protected override void Start()
    {
        // 選択された敵キャラクターを取得
        int aiCharacterId = 1;
        /*本番用
        int aiCharacterId = PlayerPrefs.GetInt(SelectCharacterData.SaveAICharacterId, 1);
        */
        SelectCharacter = CharacterDataBase.CharacterList[aiCharacterId - 1];


        // 選択された敵の強さを取得
        _aiLevel = 1;
        /*本番用
        _aiLevel = PlayerPrefs.GetInt(SelectCharacterData.SaveAILevel, 1);
        */

        // コマンド表示オブジェクトのImageコンポーネントを取得
        for (int i = 0; i < _commandObjArray.Length; i++)
        {
            _commandImageArray[i] = _commandObjArray[i].GetComponent<Image>();
            _mindImageArray[i] = _commandObjArray[i].transform.Find(_mindObjName).GetComponent<Image>();
        }

        // 表示するコマンドの数を取得
        ShowCommandCheck();

        // 1ターン目のコマンドを決定
        SetAICommand();
    }

    public void SetAICommand()
    {
        // 全ての表示をリセット
        for (int i = 0; i < _commandObjArray.Length; i++)
        {
            _commandImageArray[i].sprite = _unknownSprite;
            _mindImageArray[i].sprite = _unknownSprite;
        }


        // 敵のコマンドを決定
        for (int i = 0; i < _selectCommandIndexArray.Length; i++)
        {
            _selectCommandIndexArray[i] = Random.Range(_startCommandRange, _endCommandRange);
            Debug.Log("敵のコマンド：" + _selectCommandIndexArray[i]);
        }

        // 気を決定(0なら陰、1なら陽)
        for (int i = 0; i < _selectMindIndexArray.Length; i++)
        {
            _selectMindIndexArray[i] = Random.Range(0, 2);
            Debug.Log("敵の陰陽：" + _selectMindIndexArray[i]);
        }

        // コマンドを表示する
        List<int> showCommandPositionList = ShowPositionDecide(_showCommandNumber);

        for (int i = 0; i < showCommandPositionList.Count; i++)
        {
            int positionNumber = showCommandPositionList[i];
            _commandImageArray[positionNumber].sprite = SelectCharacter.SelectCommandSprites[_selectCommandIndexArray[positionNumber]];
        }

        // 気を表示する
        List<int> showMindPositionList = ShowPositionDecide(_showMindNumber);

        for (int i = 0; i < showMindPositionList.Count; i++)
        {
            int positionNumber = showMindPositionList[i];
            _mindImageArray[positionNumber].sprite = _yinYanSprites[_selectMindIndexArray[i]];
        }

    }

    // AIレベルによって表示する量を変更
    private void ShowCommandCheck()
    {
        if (_aiLevel == 1)
        {
            _showCommandNumber = 2;
            _showMindNumber = 2;
            return;
        }

        if (_aiLevel == 2)
        {
            _showCommandNumber = 1;
            _showMindNumber = 1;
            return;
        }

        if (_aiLevel == 3)
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
        }

        return _returnPositionIndex;
    }
}
