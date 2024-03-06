using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleManager0 : MonoBehaviour
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
    private int _aiDamageBase = 15;         // 敵の基礎ダメージ量
    private int _damageMagnification = 2;   // 有利(*)、不利(/)時の倍率
    private int _harmonyDamageUpValue = 3;  // 比和のダメージUP量
    private int _damageUpValue = 5;         // 相生によるダメージUP量

    [Header("ダメージ表示")]
    [SerializeField] private Sprite[] _damageIcon = new Sprite[5];  // 増幅、比和、相性有利、相性不利、打ち消し
    private Sprite[,] _playerDamageSprite = new Sprite[3, 4];       // プレイヤーのダメージ表示
    private Sprite[,] _aiDamageSprite = new Sprite[3, 3];           // 敵のダメージ表示
    [SerializeField] private GameObject[] _playerDamageIconObj;     // 画像を適用するObject(プレイヤー)
    [SerializeField] private GameObject[] _aiDamageIconObj;         // 画像を適用するObject(敵)

    [Header("勝敗結果")]
    [SerializeField] private GameObject _resultCanvas;
    private const string _resultImageObjName = "ResultImage";
    private Image _resultImage;
    private const string _characterImageObjName = "CharacterImage";
    private GameObject _characterImageObj;
    private Image _characterImage;
    private const string _resultDefaultButtonObjName = "BackCharacterSelectButton";
    private GameObject _resultDefaultButtonObj;
    [SerializeField] private Sprite[] _resultSprites = new Sprite[2];   // 勝敗画像

    [Header("Audio")]
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

    [Header("Animation")]
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
        _wuXingCheck = this.GetComponent<WuXingCheck>();
        _yinYangCheck = this.GetComponent <YinYangCheck>();

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
        StartCoroutine(DamageResult(attributeResult, damageResult));

    }

    /// <summary>
    /// コマンド属性の相性判定
    /// [n,0] 相性判定の結果(1:有利、-1:不利、0:どちらでもない)
    /// [n,1] プレイヤーの比和判定(0:無、1:有)
    /// [n,2] 敵の比和判定(0:無、1:有)
    /// [n,3] ダメージ増幅判定(0:無、1:有)
    /// </summary>
    /// <returns>判定結果の配列</returns>
    private int[,] AttributeCheck()
    {
        int[,] result = new int[3,4];   // 判定結果の配列
        int beforeAiCommandId = -1;      // 直前の敵のコマンド

        // コマンドの順番ごとに結果を配列に格納
        for (int i = 0; i < result.GetLength(0); i++)
        {
            int playerCommandAttributeId = _playerCommandManager.CommandIdList[i] + 1;
                
            // プレイヤーのコマンド属性の有利・不利を取得
            var attributeCompatibility = AttributeCompativilityCheck(playerCommandAttributeId);
            int playerAdvantageous = attributeCompatibility.playerAdvantageous;
            int playerDisadvantage = attributeCompatibility.playerDisadvantage;
            int playerDamageUp = attributeCompatibility.playerDamageUp;

            // 敵コマンドの属性を取得
            int aiCommandAttributeId = _aiCommandManager.CommandIdList[i] + 1;

            // 比和のチェック
            result[i, 1] = 0;
            result[i, 2] = 0;

            if (IsHarmony(_playerCommandManager.SelectCharacter.AttributeId, playerCommandAttributeId))
            {
                result[i, 1] = 1;
            }
            
            if (IsHarmony(_aiCommandManager.SelectCharacter.AttributeId, aiCommandAttributeId))
            {
                result[i, 2] = 1;
            }

            // ダメージ増幅判定
            result[i, 3] = 0;

            if (i >= 1 && beforeAiCommandId == playerDamageUp)
            {
                result[i, 3] = 1;
            }

            // 敵のコマンドを記録
            beforeAiCommandId = aiCommandAttributeId;

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
    private (int playerAdvantageous, int playerDisadvantage, int playerDamageUp) AttributeCompativilityCheck(int playerCommandAttributeId)
    {
        switch (playerCommandAttributeId)
        {
            // 水
            case 1:
                return (3, 4, 5);
            // 木
            case 2:
                return (4, 5, 1);
            // 火
            case 3:
                return (5, 1, 2);
            // 土
            case 4:
                return (1, 2, 3);
            // 金
            case 5:
                return (2, 3, 4);
            // デフォルト(絶対に通らない)
            default:
                Debug.Log("処理ミス");
                return (1, 1, 1);
        }
    }

    /// <summary>
    /// ダメージの量を取得
    /// [n,0] 敵へのダメージ量
    /// [n,1] プレイヤーへのダメージ量
    /// [n,2] 打ち消しの有無(0:なし,1:あり)
    /// </summary>
    /// <param name="attributeResult">[n,0]相性、[n,1]プレイヤー比和、[n,2]敵比和、[n,3]増幅</param>
    /// <returns>コマンドごとのダメージ量の配列</returns>
    private int[,] DamageCheck(int[,] attributeResult)
    {
        int[,] result = new int[3,3];            // 確定したダメージの配列

        // コマンドの順番ごとに結果を配列に格納
        for (int i = 0; i < result.GetLength(0); i++)
        {
            int sumPlayerDamageBase = _playerDamageBase;   // プレイヤーが与える基礎ダメージの合計
            int sumPlayerDamage = 0;                       // プレイヤーが与えるダメージの合計
            int sumAiDamageBase = _aiDamageBase;           // 敵が与える基礎ダメージの合計
            int sumAiDamage = 0;                           // 敵が与えるダメージの合計
            List<Sprite> playerDamageIcon = new List<Sprite>();    // プレイヤーのダメージ要素
            List<Sprite> aiDamageIcon = new List<Sprite>();        // 敵のダメージ要素

            // 直前の属性相性によるダメージUP
            if (attributeResult[i, 3] == 1)
            {
                sumPlayerDamageBase += _damageUpValue;
                playerDamageIcon.Add(_damageIcon[0]);
            }

            // 比和発生によるダメージUP
            if (attributeResult[i, 1] == 1)
            {
                sumPlayerDamageBase += _harmonyDamageUpValue;
                playerDamageIcon.Add(_damageIcon[1]);
            }

            if (attributeResult[i, 2] == 1)
            {
                sumAiDamageBase += _harmonyDamageUpValue;
                aiDamageIcon.Add(_damageIcon[1]);
            }

            // プレイヤーが有利な時
            if (attributeResult[i,0] == 1)
            {
                sumPlayerDamage = sumPlayerDamageBase * _damageMagnification;
                sumAiDamage = sumAiDamageBase / _damageMagnification;
                playerDamageIcon.Add(_damageIcon[2]);
                aiDamageIcon.Add(_damageIcon[3]);
            }

            // プレイヤーが不利な時
            if (attributeResult[i,0] == -1)
            {
                sumPlayerDamage = sumPlayerDamageBase / _damageMagnification;
                sumAiDamage = sumAiDamageBase * _damageMagnification;
                playerDamageIcon.Add(_damageIcon[3]);
                aiDamageIcon.Add(_damageIcon[2]);
            }

            // 相性がないとき
            if (attributeResult[i,0] == 0)
            {
                sumPlayerDamage = sumPlayerDamageBase;
                sumAiDamage = sumAiDamageBase;
            }

            // 攻撃の打ち消しが発生しているかをチェック
            result[i, 2] = 0;

            if (attributeResult[i, 0] != -1 && IsContradict(_playerCommandManager.IsYinList[i], _aiCommandManager.IsYinList[i]))
            {
                result[i, 2] = 1;
                sumAiDamage /= 5;
                playerDamageIcon.Add(_damageIcon[4]);
            }

            // ダメージ確定
            result[i, 0] = sumPlayerDamage;
            result[i, 1] = sumAiDamage;

            // ダメージアイコン表示
            for (int j = 0; j < playerDamageIcon.Count; j++)
            {
                Debug.Log("playerDamageIconの長さ：" + playerDamageIcon.Count);
                Debug.Log("_playerDamageSpriteの長さ：" + _playerDamageSprite[i, j]);
                _playerDamageSprite[i, j] = playerDamageIcon[j];
            }

            for (int j = 0; j < aiDamageIcon.Count; j++)
            {
                _aiDamageSprite[i, j] = aiDamageIcon[j];
            }
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
    private IEnumerator DamageResult(int[,] attributeResult, int[,] damageResult)
    {
        var wait = new WaitForSeconds(2f);
        yield return new WaitForSeconds(0.5f);

        // コマンドの順番にダメージを確定
        for (int i = 0; i < damageResult.GetLength(0); i++)
        {
            // ダメージ表示
            for (int j = 0; j < _playerDamageSprite.GetLength(0); j++)
            {
                Debug.Log("playerDamageSpriteの長さ：" + _playerDamageSprite.Length);
                _playerDamageIconObj[j].GetComponent<Image>().sprite = _playerDamageSprite[i, j];
            }

            for (int j = 0; j < _aiDamageSprite.Length; j++)
            {
                _aiDamageIconObj[j].GetComponent<Image>().sprite = _aiDamageSprite[i, j];
            }


            // 攻撃アニメーション
            if (attributeResult[i, 0] == 1)
            {
                // 有利
                _seAudio.PlayOneShot(_advantageousAttackSE);
            }

            if (attributeResult[i, 0] == 0)
            {
                // 普通
                _seAudio.PlayOneShot(_attackSE);
            }

            if (attributeResult[i, 0] == -1)
            {
                // 不利
                _seAudio.PlayOneShot(_disadbantageAttackSE);
            }

            // 打ち消しアニメーション
            bool isContradict = false;
            if (damageResult[i, 2] == 1)
            {
                _seAudio.PlayOneShot(_contradictSE);
                isContradict = true;
            }

            // 比和アニメーション
            bool isPlayerHarmony = false;
            if (attributeResult[i, 1] == 1)
            {
                _seAudio.PlayOneShot(_harmonySE);
                isPlayerHarmony = true;
            }

            bool isAiHarmony = false;
            if (!isContradict && attributeResult[i, 2] == 1)
            {
                _seAudio.PlayOneShot(_harmonySE);
                isAiHarmony = true;
            }

            AttackAnimation(_playerCommandManager.CommandIdList[i] + 1, _aiCommandManager.CommandIdList[i] + 1,
                isContradict, isPlayerHarmony, isAiHarmony);
            
            // デバッグ用
            Debug.Log("相性：" + attributeResult[i, 0]);
            Debug.Log("自分の比和：" + attributeResult[i, 1] + "\n相手の比和：" + attributeResult[i, 2]);
            Debug.Log("増幅の有無：" + attributeResult[i, 3] + "\n打ち消しの有無：" + damageResult[i, 2]);
            Debug.Log("敵へのダメージ：" + damageResult[i, 0] + "\nプレイヤーへのダメージ：" + damageResult[i, 1]);
            

            // 敵へのダメージを確定
            _aiHpSlider.value -= damageResult[i, 0];

            // プレイヤーへのダメージを確定
            _playerHpSlider.value -= damageResult[i, 1];

            yield return wait;

            // 敗北
            if (_playerHpSlider.value <= 0)
            {
                _isFinish = true;
                StartCoroutine(BattleFinish(false));
                break;
            }

            // 勝利
            if (_aiHpSlider.value <= 0)
            {
                _isFinish = true;
                StartCoroutine(BattleFinish(true));
                break;
            }
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
        _aiCommandManager.ShowAICommand();

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
        _bgmAudio.loop = false;

        // 勝敗結果の表示
        _resultCanvas.SetActive(true);
        _resultImage = GameObject.Find(_resultImageObjName).GetComponent<Image>();
        _characterImageObj = GameObject.Find(_characterImageObjName);
        _characterImage = _characterImageObj.GetComponent<Image>();
        
        // プレイヤー勝利
        if (playerWin)
        {
            _resultImage.sprite = _resultSprites[0];
        }

        // プレイヤー敗北
        if (!playerWin)
        {
            _resultImage.sprite = _resultSprites[1];
        }

        _characterImage.sprite = GameObject.Find("PlayerCharacter").GetComponent<Image>().sprite;
        _characterImageObj.GetComponent<Animator>().SetTrigger("First");

        yield return new WaitForSeconds(1f);

        _resultCanvas.transform.Find("Button").gameObject.SetActive(true);
        _resultDefaultButtonObj = GameObject.Find(_resultDefaultButtonObjName);
        EventSystem.current.SetSelectedGameObject(_resultDefaultButtonObj);

    }
    
}
