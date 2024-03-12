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
    private WuXingCheck _wuXingCheck;
    private YinYangCheck _yinYangCheck;
    private BattleResult _battleResult;

    // HP
    private const string _playerHpObjName = "PlayerCharacterHP";
    private const string _auHpObjName = "AICharacterHP";
    private Slider _playerHpSlider;
    private Slider _aiHpSlider;
    private int _maxHp = 100;
    private bool _isFirstAnimation = false;    // 最初のアニメーションが行われたか
    private int _nowRound = 1;                 // 現在のラウンド
    private bool _isFinish = false;            // 勝敗が決定しているか

    // ダメージ計算
    private int _playerDamageBase = 5;      // プレイヤーの基礎ダメージ量
    private int _aiDamageBase = 8;         // 敵の基礎ダメージ量

    [Header("Animation")]
    [SerializeField] private ParticleSystem _waterAttackParticle;   // 水属性攻撃エフェクト
    [SerializeField] private ParticleSystem _treeAttackParticle;    // 木属性攻撃エフェクト
    [SerializeField] private ParticleSystem _fireAttackParticle;    // 火属性攻撃エフェクト
    [SerializeField] private ParticleSystem _soilAttackParticle;    // 土属性攻撃エフェクト
    [SerializeField] private ParticleSystem _goldAttackParticle;    // 金属性攻撃エフェクト
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
        _wuXingCheck = this.GetComponent<WuXingCheck>();
        _yinYangCheck = this.GetComponent<YinYangCheck>();
        _battleResult = this.GetComponent<BattleResult>();

        // アニメーション描画位置を取得
        _playerPos = GameObject.Find(_playerObjName).transform.position;
        _aiPos = GameObject.Find(_aiObjName).transform.position;

        // HPのSlider取得
        _playerHpSlider = GameObject.Find(_playerHpObjName).GetComponent<Slider>();
        _aiHpSlider = GameObject.Find(_auHpObjName).GetComponent<Slider>();

        // HPの初期設定
        _playerHpSlider.value = 0;
        _aiHpSlider.value = 0;
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
    /// 選択されたコマンドによるバトルを開始する
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoBattleStart()
    {
        List<int> playerCommandList = _playerCommandManager.CommandIdList;  // プレイヤーのコマンドの属性IDリスト
        List<bool> playerIsYinList = _playerCommandManager.IsYinList;       // プレイヤーのコマンドの陰陽リスト
        List<int> aiCommandList = _aiCommandManager.CommandIdList;          // AIのコマンドの属性IDリスト
        List<bool> aiIsYinList = _aiCommandManager.IsYinList;               // AIのコマンドの陰陽リスト

        var isPlayerReinforce = false;
        var isAiReinforce = false;

        var wait = new WaitForSeconds(2.0f);

        // 1ラウンドの処理
        for (var i = 0; i < playerCommandList.Count; i++)
        {
            int playerDamage = _playerDamageBase;
            int aiDamage = _aiDamageBase;

            // 【陰陽制約】コマンドの陰陽が全て同じなら、どれかを違うものに変更する
            _yinYangCheck.Restriction(ref playerIsYinList, _playerCommandManager, true);
            _yinYangCheck.Restriction(ref aiIsYinList, _aiCommandManager, false);

            // 【陰陽互根】お互いの陰陽が同じなら、ダメージは発生しない
            if (!_yinYangCheck.Differ(playerIsYinList[i], aiIsYinList[i]))
            {
                yield return wait;
                _yinYangCheck.AnimParametersReset();
                continue;
            }

            // 【比和】キャラクターとコマンドの属性が同じなら、以降の効果を増幅する
            isPlayerReinforce = _wuXingCheck.Reinforce(playerCommandList[i] + 1, _playerCommandManager.SelectCharacter, true);
            isAiReinforce = _wuXingCheck.Reinforce(aiCommandList[i] + 1, _aiCommandManager.SelectCharacter, false);

            // 【相剋】属性相性により、ダメージを増減
            var rivalryDamage = _wuXingCheck.Rivalry(playerDamage, aiDamage, playerCommandList[i] + 1, aiCommandList[i] + 1, isPlayerReinforce, isAiReinforce);
            playerDamage = rivalryDamage.playerDamaged;
            aiDamage = rivalryDamage.aiDamaged;

            // 【相生】属性相性により、ダメージを増幅
            playerDamage = _wuXingCheck.Amplification(playerDamage, playerCommandList[i] + 1, aiCommandList[i] + 1, isPlayerReinforce, true);
            aiDamage = _wuXingCheck.Amplification(aiDamage, aiCommandList[i] + 1, playerCommandList[i] + 1, isAiReinforce, false);

            // 攻撃アニメーション
            AttackAnimation(playerCommandList[i] + 1, isPlayerReinforce, _aiPos);
            AttackAnimation(aiCommandList[i] + 1, isAiReinforce, _playerPos);

            // 敵へのダメージを確定
            _aiHpSlider.value -= playerDamage;

            // プレイヤーへのダメージを確定
            _playerHpSlider.value -= aiDamage;

            yield return wait;

            _wuXingCheck.AnimParametersReset();

            yield return new WaitForSeconds(1.0f);

            // 敗北
            if (_playerHpSlider.value <= 0)
            {
                _isFinish = true;
                StartCoroutine(_battleResult.CoBattleFinish(false));
                break;
            }

            // 勝利
            if (_aiHpSlider.value <= 0)
            {
                _isFinish = true;
                StartCoroutine(_battleResult.CoBattleFinish(true));
                break;
            }
        }

        // お互いのHPが残っていれば、次のラウンドへ
        if (!_isFinish)
        {
            StartCoroutine(NextRound());
        }
    }

    /// <summary>
    /// 攻撃アニメーション
    /// </summary>
    /// <param name="commandAttributeId">コマンドの属性ID</param>
    /// <param name="isReinforce">比和の有無</param>
    private void AttackAnimation(int commandAttributeId, bool isReinforce, Vector3 pos)
    {
        ParticleSystem damageParticle = null;

        switch (commandAttributeId)
        {
            case 1:
                damageParticle = Instantiate(_waterAttackParticle, pos, Quaternion.identity);
                break;
            case 2:
                damageParticle = Instantiate(_treeAttackParticle, pos, Quaternion.identity);
                break;
            case 3:
                damageParticle = Instantiate(_fireAttackParticle, pos, Quaternion.identity);
                break;
            case 4:
                damageParticle = Instantiate(_soilAttackParticle, pos, Quaternion.identity);
                break;
            case 5:
                damageParticle = Instantiate(_goldAttackParticle, pos, Quaternion.identity);
                break;
            default:
                break;
        }

        if (isReinforce)
        {
            damageParticle.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }

        damageParticle.Play();
        Destroy(damageParticle.gameObject, 0.5f);
    }

    private IEnumerator NextRound()
    {
        // コマンド初期化
        _playerCommandManager.SelectingCommandSequence = 0;
        _playerCommandManager.IsAllSelect = false;
        _playerCommandManager.CommandReset();
        _playerCommandManager.CommandIdList.Clear();
        _playerCommandManager.IsYinList.Clear();
        _aiCommandManager.CommandIdList.Clear();
        _aiCommandManager.IsYinList.Clear();
        _aiCommandManager.ShowAICommand();

        yield return new WaitForSeconds(0.5f);

        // ラウンド表示を更新
        _nowRound++;
        _roundCounter.CountUp(_nowRound);
        _yinYangCheck.RestrictionAnimParametersReset();

        yield return new WaitForSeconds(1f);
    }
}
