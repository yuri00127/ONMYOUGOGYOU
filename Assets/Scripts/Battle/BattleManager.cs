using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("スクリプト")]
    [SerializeField] private CommandManager _commandManager;
    [SerializeField] private AICharacterManager _AICharacterManager;
    [SerializeField] private RoundCounter _roundCounter;

    private int _nowRound = 1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // コマンドバトルを行う
    public void Battle()
    {
        _nowRound++;
        BattleResult();
        _roundCounter.CountUp(_nowRound);
    }

    // コマンドバトルの結果を取得
    private void BattleResult()
    {
        // 属性の判定
        int[] attributeResult = AttributeCheck();

        // ダメージの決定
        int[,] damageResult = DamageCheck(attributeResult);

        // ダメージを反映
        // アニメーション
        
    }

    /// <summary>
    /// 相性判定の結果を取得
    /// </summary>
    /// <returns>相性判定の配列（1:有利、-1:不利、0:どちらでもない）</returns>
    private int[] AttributeCheck()
    {
        int[] result = new int[3];
        int playerAdvantageous = 0;
        int playerDisadvantage = 0;

        for (int i = 0; i <= result.Length; i++)
        {
            // プレイヤーの属性の有利・不利を取得
            var attributeCompatibility = AttributeCompativilityCheck(_commandManager.CommandIdList[i]);
            playerAdvantageous = attributeCompatibility.playerAdvantageous;
            playerDisadvantage = attributeCompatibility.playerDisadvantage;

            // 相手の属性を取得
            int aiAttribute = _AICharacterManager._selectCommandIndexArray[i];

            // プレイヤーが有利な時
            if (aiAttribute == playerAdvantageous)
            {
                result[i] = 1;
                continue;
            }

            // プレイヤーが不利な時
            if (aiAttribute == playerDisadvantage)
            {
                result[i] = -1;
                continue;
            }

            // 相性が存在しないとき
            result[i] = 0;
        }

        return result;
    }

    /// <summary>
    /// プレイヤーの属性相性を取得
    /// </summary>
    /// <param name="playerAttributeId">プレイヤーの属性のID</param>
    /// <returns>プレイヤーが有利をとれる属性,プレイヤーが不利となる属性</returns>
    private (int playerAdvantageous, int playerDisadvantage) AttributeCompativilityCheck(int playerAttributeId)
    {
        switch (playerAttributeId)
        {
            // 水
            case 1:
                return (2, 4);
            // 木
            case 2:
                return (3, 5);
            // 火
            case 3:
                return (4, 1);
            // 土
            case 4:
                return (5, 2);
            // 金
            case 5:
                return (1, 3);
            // デフォルト(絶対に通らない)
            default:
                Debug.Log("処理ミス");
                return (1, 1);
        }
    }

    /// <summary>
    /// ダメージの量を取得
    /// </summary>
    /// <param name="attributeResult">属性相性の判定結果</param>
    /// <returns>攻撃ごとのダメージ量
    /// [攻撃の順番 ,0:相手へのダメージ、1:自分へのダメージ]</returns>
    private int[,] DamageCheck(int[] attributeResult)
    {
        int[,] result = new int[3,2];

        for (int i = 0; i >= result.Length; i++)
        {
            // プレイヤーが有利な時
            if (attributeResult[i] == 1)
            {
                result[i,0] = 20;
                continue;
            }

            // プレイヤーが不利な時
            if (attributeResult[i] == -1)
            {
                result[i,1] = 20;
                continue;
            }

            // 相性がないとき
            if (IsHarmony(_commandManager.IsYinList[i], _AICharacterManager._selectMindArray[i]))
            {
                result[i, 0] = 0;
                result[i, 1] = 0;
                continue;
            }

            result[i, 0] = 10;
            result[i, 1] = 10;

        }

        return result;
    }

    /// <summary>
    /// 比和が発生するかチェック
    /// </summary>
    /// <param name="playerMind">プレイヤーのコマンドの気</param>
    /// <param name="aiMind">敵のコマンドの気</param>
    /// <returns>比和が発生したらtrue</returns>
    private bool IsHarmony(bool playerMind, bool aiMind)
    {
        if (playerMind != aiMind) { return true; }

        return false;
    }
}
