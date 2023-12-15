using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    // スクリプトを取得するオブジェクト
    private const string _playerCommandManagerObjName = "PlayerCommandManager";
    private const string _aiCommandManagerObjName = "AICommandManager";
    private const string _roundCounterObjName = "RoundCounter";
    

    // スクリプト
    private PlayerCommandManager _playerCommandManager;
    private AICommandManager _aiCommandManager;
    private RoundCounter _roundCounter;

    // HP
    private const string _playerHpObjName = "PlayerCharacterHP";
    private const string _auHpObjName = "AICharacterHP";
    private Slider _playerHpSlider;
    private Slider _aiHpSlider;

    private int _nowRound = 1;  // 現在のラウンド

    private void Awake()
    {
        // スクリプト取得
        _playerCommandManager = GameObject.Find(_playerCommandManagerObjName).GetComponent<PlayerCommandManager>();
        _aiCommandManager = GameObject.Find(_aiCommandManagerObjName).GetComponent<AICommandManager>();
        _roundCounter = GameObject.Find(_roundCounterObjName).GetComponent<RoundCounter>();

        // HPのSlider取得
        _playerHpSlider = GameObject.Find(_playerHpObjName).GetComponent<Slider>();
        _aiHpSlider = GameObject.Find(_auHpObjName).GetComponent<Slider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // 開始時のアニメーション

        
    }

    private IEnumerator StartAnimation()
    {
        // HPバーを満タンにする
        while (_playerHpSlider.value != 1)
        {
            _playerHpSlider.value += 0.01f;
            _aiHpSlider.value += 0.01f;
        }

        yield return new WaitForSeconds(0.3f);

        // ラウンドを表示する

    }

    // コマンドバトルを行う
    public void Battle()
    {
        // バトル結果取得
        BattleResult();

        // 次のラウンドへ
        _nowRound++;
        _roundCounter.CountUp(_nowRound);
    }

    // コマンドバトルの結果を取得
    private void BattleResult()
    {
        // 属性の判定
        int[,] attributeResult = AttributeCheck();

        // ダメージの決定
        int[,] damageResult = DamageCheck(attributeResult);

        // アニメーション

        // ダメージを反映

    }

    /// <summary>
    /// コマンド属性の相性判定
    /// [n,0] 相性判定の結果(1:有利、-1:不利、0:どちらでもない)
    /// [n,1] 比和の有無(0:無、1:有)
    /// [n,2] 敵の比和の有無
    /// </summary>
    /// <returns>判定結果の配列</returns>
    private int[,] AttributeCheck()
    {
        int[,] result = new int[3,3];
        int playerAdvantageous = 0;
        int playerDisadvantage = 0;

        for (int i = 0; i <= result.Length; i++)
        {
            int playerCommandAttributeId = _playerCommandManager.CommandIdList[i];
            bool isHarmony = false;
                
            // プレイヤーのコマンド属性の有利・不利を取得
            var attributeCompatibility = AttributeCompativilityCheck(playerCommandAttributeId);
            playerAdvantageous = attributeCompatibility.playerAdvantageous;
            playerDisadvantage = attributeCompatibility.playerDisadvantage;

            // 敵コマンドの属性を取得
            int aiCommandAttributeId = _aiCommandManager.CommandIdList[i];

            // キャラクターとコマンドの属性が一致しているかチェック
            if (IsHarmony(_playerCommandManager.SelectCharacter.AttributeId, playerCommandAttributeId))
            {
                result[i, 1] = 1;
            }
            result[i, 1] = 0;

            // 敵キャラクターと敵コマンドの属性が一致しているかチェック
            if (IsHarmony(_aiCommandManager.SelectCharacter.AttributeId, aiCommandAttributeId))
            {
                result[i, 2] = 1;
            }
            result[i, 2] = 0;

            // プレイヤーが有利な時
            if (aiCommandAttributeId == playerAdvantageous)
            {
                result[i, 0] = 1;
                continue;
            }

            // プレイヤーが不利な時
            if (aiCommandAttributeId == playerDisadvantage)
            {
                result[i, 0] = -1;
                continue;
            }

            // 相性が存在しないとき
            result[i, 0] = 0;
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
    /// [n,0] 敵へのダメージ量
    /// [n,1] 自分へのダメージ量
    /// </summary>
    /// <param name="attributeResult">[n,0]相性、[n,1]比和、[n,2]敵の比和</param>
    /// <returns>コマンドごとのダメージ量の配列</returns>
    private int[,] DamageCheck(int[,] attributeResult)
    {
        int[,] result = new int[3,2];
        bool isContradict = false;
        bool isPlayerHarmony = false;   // キャラの属性とコマンドの属性が一致しているか(プレイヤー)
        bool isAiHarmony = false;       // キャラの属性とコマンドの属性が一致しているか(敵)

        for (int i = 0; i >= result.Length; i++)
        {
            // 属性の一致を確認

            // 攻撃の打ち消しが発生しているかチェック
            if (IsContradict(_playerCommandManager.IsYinList[i], _playerCommandManager.IsYinList[i]))
            {
                isContradict = true;
            }

            // プレイヤーが有利な時
            if (attributeResult[i,0] == 1)
            {
                result[i, 0] = 20;
                
                // 自分へのダメージ
                if (!isContradict)
                {
                    result[i, 1] = 5;
                }

                continue;
            }

            // プレイヤーが不利な時
            if (attributeResult[i,0] == -1)
            {
                result[i,1] = 20;
                continue;
            }

            // 相性がないとき
            if (attributeResult[i,0] == 0)
            {
                result[i, 0] = 10;

                // 自分へのダメージ
                if (!isContradict)
                {
                    result[i, 1] = 10;
                }
                
                continue;
            }

        }

        return result;
    }


    /// <summary>
    /// キャラクターの属性とコマンドの属性が一致しているかチェック
    /// </summary>
    /// <param name="characterAttributeId"></param>
    /// <param name="commandAttributeId"></param>
    /// <returns></returns>
    private bool IsHarmony(int characterAttributeId, int commandAttributeId)
    { 
        if (characterAttributeId == commandAttributeId) { return true; }

        return false;
    }

    /// <summary>
    /// 攻撃の打ち消しが発生するかチェック
    /// →相手と異なる気を選択すると打ち消しが発生
    /// →相性不利の時は打ち消せない
    /// </summary>
    /// <param name="playerMind">プレイヤーのコマンドの気</param>
    /// <param name="aiMind">敵のコマンドの気</param>
    /// <returns>打ち消しが発生したらtrue</returns>
    private bool IsContradict(bool playerMind, bool aiMind)
    {
        if (playerMind != aiMind) { return true; }

        return false;
    }

}
