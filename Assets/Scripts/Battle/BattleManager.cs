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
    private int _maxHp = 100;

    private bool _isFirstAnimation = false;    // 最初のアニメーションが行われたか
    private int _nowRound = 1;                 // 現在のラウンド
    private bool _isFinish = false;            // 勝敗が決定しているか

    // Audio
    private const string _seManagerObjName = "SEManager";
    private const string _bgmManagerObjName = "BGMManager";
    private AudioSource _seAudio;
    private AudioSource _bgmAudio;
    [SerializeField] private AudioClip _disadbantageAttackSE;
    [SerializeField] private AudioClip _attackSE;
    [SerializeField] private AudioClip _advantageousAttackSE;
    [SerializeField] private AudioClip _harmonySE;             // 比和発生時のSE
    [SerializeField] private AudioClip _contradictSE;          // 打ち消し発生時のSE
    [SerializeField] private AudioClip _battleFinishJingle;    // バトル終了時のジングル

    // Animation
    [SerializeField] private ParticleSystem _waterAttackParticle;
    [SerializeField] private ParticleSystem _treeAttackParticle;
    [SerializeField] private ParticleSystem _fireAttackParticle;
    [SerializeField] private ParticleSystem _soilAttackParticle;
    [SerializeField] private ParticleSystem _goldAttackParticle;
    private Vector3 _playerPos;
    private const string _playerObjName = "PlayerCharacter";
    private Vector3 _aiPos;
    private const string _aiObjName = "AICharacter";


    private void Awake()
    {
        // スクリプト取得
        _playerCommandManager = GameObject.Find(_playerCommandManagerObjName).GetComponent<PlayerCommandManager>();
        _aiCommandManager = GameObject.Find(_aiCommandManagerObjName).GetComponent<AICommandManager>();
        _roundCounter = GameObject.Find(_roundCounterObjName).GetComponent<RoundCounter>();

        // HPのSlider取得
        _playerHpSlider = GameObject.Find(_playerHpObjName).GetComponent<Slider>();
        _aiHpSlider = GameObject.Find(_auHpObjName).GetComponent<Slider>();

        // HPの初期設定
        _playerHpSlider.value = 0;
        _aiHpSlider.value = 0;

        _seAudio = GameObject.Find(_seManagerObjName).GetComponent<AudioSource>();
        _bgmAudio = GameObject.Find(_bgmManagerObjName).GetComponent<AudioSource>();

        _playerPos = GameObject.Find(_playerObjName).transform.position;
        _aiPos = GameObject.Find(_aiObjName).transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        // 開始時アニメーション
        StartCoroutine(StartAnimation());
        
    }

    private void Update()
    {
        // 開始時アニメーション
        if (!_isFirstAnimation)
        {
            _playerHpSlider.value = Mathf.Lerp(_playerHpSlider.value, _maxHp, 0.04f);
            _aiHpSlider.value = Mathf.Lerp(_aiHpSlider.value, _maxHp, 0.04f);
        }
    }

    /// <summary>
    /// 開始時のアニメーション
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(1f);

        // HPバーを満タンにする
        _isFirstAnimation = true;   // アニメーション
        _playerHpSlider.value = _maxHp;
        _aiHpSlider.value = _maxHp;
    }

    /// <summary>
    /// コマンドバトルを行う
    /// </summary>
    public void Battle()
    {
        // 属性の判定
        int[,] attributeResult = AttributeCheck();

        // ダメージの決定
        int[,] damageResult = DamageCheck(attributeResult);

        // ダメージを反映
        StartCoroutine(DamageResult(damageResult));

    }

    /// <summary>
    /// コマンド属性の相性判定
    /// [n,0] 相性判定の結果(1:有利、-1:不利、0:どちらでもない)
    /// [n,1] 比和の有無(0:無、1:自分、-1:敵)
    /// </summary>
    /// <returns>判定結果の配列</returns>
    private int[,] AttributeCheck()
    {
        int[,] result = new int[3,2];
        int playerAdvantageous = 0;
        int playerDisadvantage = 0;

        // コマンドの順番ごとに結果を配列に格納
        for (int i = 0; i < result.GetLength(0); i++)
        {
            int playerCommandAttributeId = _playerCommandManager.CommandIdList[i] + 1;
                
            // プレイヤーのコマンド属性の有利・不利を取得
            var attributeCompatibility = AttributeCompativilityCheck(playerCommandAttributeId);
            playerAdvantageous = attributeCompatibility.playerAdvantageous;
            playerDisadvantage = attributeCompatibility.playerDisadvantage;

            // 敵コマンドの属性を取得
            int aiCommandAttributeId = _aiCommandManager.CommandIdList[i] + 1;

            // 比和のチェック
            result[i, 1] = 0;

            // キャラクターとコマンドの属性が一致しているかチェック
            if (IsHarmony(_playerCommandManager.SelectCharacter.AttributeId, playerCommandAttributeId))
            {
                result[i, 1] = 1;
            }
            
            // 敵キャラクターと敵コマンドの属性が一致しているかチェック
            if (IsHarmony(_aiCommandManager.SelectCharacter.AttributeId, aiCommandAttributeId))
            {
                result[i, 1] = -1;
            }

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
    private (int playerAdvantageous, int playerDisadvantage) AttributeCompativilityCheck(int playerCommandAttributeId)
    {
        switch (playerCommandAttributeId)
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
    /// [n,2] 属性相性(-1:不利,0:普通,1:有利)
    /// [n,3] 打ち消しの有無(0:なし,1:あり)
    /// [n,4] 比和の有無(0:なし,1:あり)
    /// [n,5] 敵の比和の有無(0:なし,1:あり)
    /// </summary>
    /// <param name="attributeResult">[n,0]相性、[n,1]比和</param>
    /// <returns>コマンドごとのダメージ量の配列</returns>
    private int[,] DamageCheck(int[,] attributeResult)
    {
        int[,] result = new int[3,6];               // 確定したダメージの配列
        int[] damaged = new int[] { 15, 5, 10 };    // 有利、不利、通常のダメージ量
        bool isContradict = false;                  // 攻撃打ち消し発生の有無

        // コマンドの順番ごとに結果を配列に格納
        for (int i = 0; i < result.GetLength(0); i++)
        {
            // 攻撃の打ち消しが発生しているかをチェック
            isContradict = IsContradict(_playerCommandManager.IsYinList[i], _aiCommandManager.IsYinList[i]);

            result[i, 3] = 0;

            if (isContradict)
            {
                result[i, 3] = 1;
            }

            // プレイヤーが有利な時
            if (attributeResult[i,0] == 1)
            {   
                result[i, 2] = 1;

                // ダメージ
                result[i, 0] = damaged[0];
                result[i, 1] = damaged[1];

                // 打ち消し発生時
                if (isContradict)
                {
                    result[i, 1] = 0;
                }

                continue;
            }

            // プレイヤーが不利な時
            if (attributeResult[i,0] == -1)
            {
                result[i, 2] = -1;

                // ダメージ
                result[i, 0] = damaged[1];
                result[i,1] = damaged[0];

                continue;
            }

            // 相性がないとき
            if (attributeResult[i,0] == 0)
            {
                result[i, 2] = 0;

                // ダメージ
                result[i, 0] = damaged[2];
                result[i, 1] = damaged[2];

                // 打ち消し発生時
                if (isContradict)
                {
                    result[i, 1] = 0;
                }

                continue;
            }
        }

        // 比和の処理(ダメージを1.5倍にする)
        for (int i = 0; i < result.GetLength(0); i++)
        {
            result[i, 4] = 0;
            result[i, 5] = 0;

            // 自コマンド
            if (attributeResult[i, 1] == 1)
            {
                result[i, 0] = (int)(result[i, 0] * 1.5);
                result[i, 4] = 1;
            }

            // 相手コマンド
            if (attributeResult[i, 1] == -1)
            {
                result[i, 1] = (int)(result[i, 1] * 1.5);
                result[i, 5] = 1;
            }

            //Debug.Log(string.Format("{0}番目のコマンド ", i + 1) + "相手ダメージ:" + result[i, 0]);
            //Debug.Log(string.Format("{0}番目のコマンド ", i + 1) + "自ダメージ:" + result[i, 1]);
        }
        
        return result;
    }

    /// <summary>
    /// キャラクターの属性とコマンドの属性が一致しているか
    /// </summary>
    /// <param name="characterAttributeId">キャラクターの属性</param>
    /// <param name="commandAttributeId">コマンドの属性</param>
    /// <returns></returns>
    private bool IsHarmony(int characterAttributeId, int commandAttributeId)
    { 
        if (characterAttributeId == commandAttributeId) { return true; }

        return false;
    }

    /// <summary>
    /// 攻撃の打ち消しが発生するか
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

    /// <summary>
    /// ダメージをゲージに反映する
    /// </summary>
    /// <param name="damageResult"></param>
    /// <returns></returns>
    private IEnumerator DamageResult(int[,] damageResult)
    {
        var wait = new WaitForSeconds(2f);
        yield return new WaitForSeconds(0.5f);

        // コマンドの順番にダメージを確定
        for (int i = 0; i < damageResult.GetLength(0); i++)
        {
            // 攻撃アニメーション
            if (damageResult[i, 2] == 1)
            {
                // 有利
                _seAudio.PlayOneShot(_advantageousAttackSE);
            }

            if (damageResult[i, 2] == 0)
            {
                // 普通
                _seAudio.PlayOneShot(_attackSE);
            }

            if (damageResult[i, 2] == -1)
            {
                // 不利
                _seAudio.PlayOneShot(_disadbantageAttackSE);
            }

            // 打ち消しアニメーション
            bool isContradict = false;
            if (damageResult[i, 3] == 1)
            {
                _seAudio.PlayOneShot(_contradictSE);
                isContradict = true;
            }

            // 比和アニメーション
            bool isPlayerHarmony = false;
            if (damageResult[i, 4] == 1)
            {
                _seAudio.PlayOneShot(_harmonySE);
                isPlayerHarmony = true;
            }

            bool isAiHarmony = false;
            if (!isContradict && damageResult[i, 5] == 1)
            {
                _seAudio.PlayOneShot(_harmonySE);
                isAiHarmony = true;
            }

            AttackAnimation(_playerCommandManager.CommandIdList[i] + 1, _aiCommandManager.CommandIdList[i] + 1,
                isContradict, isPlayerHarmony, isAiHarmony);

            // 敵へのダメージを確定
            _aiHpSlider.value -= damageResult[i, 0];

            // 自分へのダメージを確定
            _playerHpSlider.value -= damageResult[i, 1];

            yield return wait;

            // 勝利
            if (_aiHpSlider.value <= 0)
            {
                _isFinish = true;
                StartCoroutine(BattleFinish(true));
                break;
            }

            // 敗北
            if (_playerHpSlider.value <= 0)
            {
                _isFinish = true;
                StartCoroutine(BattleFinish(false));
                break;
            }

            //yield return wait;
        }

        // お互いのHPが残っていれば、次のラウンドへ
        if (!_isFinish)
        {
            StartCoroutine(NextRound());
        }
    }

    private void AttackAnimation(int playerCommandAttributeId, int aiCommandAttributeId, bool isContradict, bool isPlayerHarmony, bool isAiHarmony)
    {
        ParticleSystem playerDamageParticle = null;
        ParticleSystem aiDamageParticle = null;

        // プレイヤーへの攻撃
        switch (playerCommandAttributeId)
        {
            case 1:
                aiDamageParticle = Instantiate(_waterAttackParticle, _aiPos, Quaternion.identity);
                break;
            case 2:
                aiDamageParticle = Instantiate(_treeAttackParticle, _aiPos, Quaternion.identity);
                break;
            case 3:
                aiDamageParticle = Instantiate(_fireAttackParticle, _aiPos, Quaternion.identity);
                break;
            case 4:
                aiDamageParticle = Instantiate(_soilAttackParticle, _aiPos, Quaternion.identity);
                break;
            case 5:
                aiDamageParticle = Instantiate(_goldAttackParticle, _aiPos, Quaternion.identity);
                break;
            default:
                break;
        }

        // 比和
        if (isPlayerHarmony)
        {
            aiDamageParticle.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }

        aiDamageParticle.Play();
        Destroy(aiDamageParticle.gameObject, 0.5f);

        // プレイヤーへの攻撃
        if (!isContradict)
        {
            switch (aiCommandAttributeId)
            {
                case 1:
                    playerDamageParticle = Instantiate(_waterAttackParticle, _playerPos, Quaternion.identity);
                    break;
                case 2:
                    playerDamageParticle = Instantiate(_treeAttackParticle, _playerPos, Quaternion.identity);
                    break;
                case 3:
                    playerDamageParticle = Instantiate(_fireAttackParticle, _playerPos, Quaternion.identity);
                    break;
                case 4:
                    playerDamageParticle = Instantiate(_soilAttackParticle, _playerPos, Quaternion.identity);
                    break;
                case 5:
                    playerDamageParticle = Instantiate(_goldAttackParticle, _playerPos, Quaternion.identity);
                    break;
                default:
                    break;
            }

            // 比和
            if (isAiHarmony)
            {
                playerDamageParticle.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }

            playerDamageParticle.Play();
            Destroy(playerDamageParticle.gameObject, 0.5f);
        }
    }

    private IEnumerator NextRound()
    {
        //yield return new WaitForSeconds(3f);

        // コマンド初期化
        _playerCommandManager.SelectingCommandSequence = 0;
        _playerCommandManager.IsAllSelect = false;
        _playerCommandManager.CommandReset();
        _playerCommandManager.CommandIdList.Clear();
        _playerCommandManager.IsYinList.Clear();
        _aiCommandManager.CommandIdList.Clear();
        _aiCommandManager.IsYinList.Clear();
        _aiCommandManager.SetAICommand();

        yield return new WaitForSeconds(0.5f);

        // ラウンド表示を更新
        _nowRound++;
        _roundCounter.CountUp(_nowRound);

        yield return new WaitForSeconds(1f);
    }

    private IEnumerator BattleFinish(bool playerWin)
    {
        Debug.Log("バトル終了");

        _bgmAudio.clip = _battleFinishJingle;
        _bgmAudio.Play();

        if (playerWin) { Debug.Log("プレイヤーの勝ち"); }
        if (!playerWin) { Debug.Log("敵の勝ち"); }

        yield return new WaitForSeconds(1f);

        // 勝敗アニメーション
    }
}
