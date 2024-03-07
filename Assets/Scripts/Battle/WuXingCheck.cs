using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 五行システムの判定を行う
/// </summary>
public class WuXingCheck : MonoBehaviour
{
    [Header("Icon")]
    [SerializeField] private GameObject _playerRivalryIcon;
    [SerializeField] private GameObject _aiRivalryIcon;
    [SerializeField] private Sprite[] _rivalryIconSprites = new Sprite[2];

    // ダメージ計算
    private int _damageMagnification = 2;             // 相剋の倍率
    private int _damageMagnificationIsReinforce = 3;  // 比和時の相剋倍率
    private int _damageUpValue = 5;                 　// 相生によるダメージUP量
    private int _damageUpValueIsReinforce = 8;        // 比和時の相生ダメージUP量

    [Header("Audio")]
    private const string _seManagerObjName = "SEManager";
    private const string _bgmManagerObjName = "BGMManager";
    private AudioSource _seAudio;
    private AudioSource _bgmAudio;
    [SerializeField] private AudioClip _reinforceSE;            // 比和SE
    [SerializeField] private AudioClip _attackSE;               // 攻撃SE
    [SerializeField] private AudioClip _advantageousAttackSE;   // 有利攻撃SE
    [SerializeField] private AudioClip _disadbantageAttackSE;   // 不利攻撃SE

    // IconAnimation
    [SerializeField] GameObject _playerBattleIconObj;
    [SerializeField] GameObject _aiBattleIconObj;
    private const string _reinforceIconObjName = "WuXingReinforce";
    private const string _rivalryIconObjName = "WuXingRivalry";
    private const string _amplificationIconObjName = "WuXingAmplification";
    private Animator _playerReinforceAnim;
    private Animator _playerRivalryAnim;
    private Animator _playerAmplificationAnim;
    private Animator _aiReinforceAnim;
    private Animator _aiRivalryAnim;
    private Animator _aiAmplificationAnim;
    private const string _reinforceParamName = "IsReinforce";
    private const string _rivalryAdParamName = "IsRivalry_Advantageous";
    private const string _rivalryDisadParamName = "IsRivalry_Disadvantage";
    private const string _amplificationParamName = "IsAmplification";


    private void Awake()
    {
        _seAudio = GameObject.Find(_seManagerObjName).GetComponent<AudioSource>();
        _bgmAudio = GameObject.Find(_bgmManagerObjName).GetComponent<AudioSource>();

        _playerReinforceAnim = _playerBattleIconObj?.transform.Find(_reinforceIconObjName).gameObject?.GetComponent<Animator>();
        _playerRivalryAnim = _playerBattleIconObj?.transform.Find(_rivalryIconObjName).gameObject?.GetComponent<Animator>();
        _playerAmplificationAnim = _playerBattleIconObj?.transform.Find(_amplificationIconObjName).gameObject?.GetComponent<Animator>() ;
        _aiReinforceAnim = _aiBattleIconObj?.transform.Find(_reinforceIconObjName).gameObject?.GetComponent <Animator>() ;
        _aiRivalryAnim = _aiBattleIconObj?.transform.Find(_rivalryIconObjName).gameObject?.GetComponent<Animator>();
        _aiAmplificationAnim = _aiBattleIconObj?.transform.Find(_amplificationIconObjName).gameObject?.GetComponent<Animator>();
    }

    /// <summary>
    /// 【相剋】属性相性によってダメージ倍率を変化
    /// </summary>
    /// <param name="playerDamageBase">プレイヤーが与える処理前ダメージ</param>
    /// <param name="aiDamageBase">AIが与える処理前ダメージ</param>
    /// <param name="playerCommandAttributeId">プレイヤーのコマンドの属性ID</param>
    /// <param name="aiCommandAttributeId">AIのコマンドの属性ID</param>
    /// <returns>プレイヤーが与える処理後ダメージ、AIが与える処理後ダメージ</returns>
    public (int playerDamaged, int aiDamaged) Rivalry (int playerDamageBase, int aiDamageBase,
        int playerCommandAttributeId, int aiCommandAttributeId, bool isPlayerReinforce, bool isAiReinforce)
    {
        int advantageousAttributeId = -1;   // プレイヤーが有利な属性ID
        int disadvantageAttributeId = -1;   // プレイヤーが不利な属性ID

        int playerDamaged;
        int aiDamaged;

        switch (playerCommandAttributeId)
        {
            // 水
            case 1:
                advantageousAttributeId = 3;
                disadvantageAttributeId = 4;
                break;
            // 木
            case 2:
                advantageousAttributeId = 4;
                disadvantageAttributeId = 5;
                break;
            // 火
            case 3:
                advantageousAttributeId = 5;
                disadvantageAttributeId = 1;
                break;
            // 土
            case 4:
                advantageousAttributeId = 1;
                disadvantageAttributeId = 2;
                break;
            // 金
            case 5:
                advantageousAttributeId = 2;
                disadvantageAttributeId = 3;
                break;
            // デフォルト(絶対に通らない)
            default:
                Debug.Log("処理ミス");
                break;
        }

        // プレイヤーが与えるダメージアップ
        if (aiCommandAttributeId == advantageousAttributeId)
        {
            _seAudio.PlayOneShot(_advantageousAttackSE);
            _playerRivalryAnim.SetBool(_rivalryAdParamName, true);
            _aiRivalryAnim.SetBool(_rivalryDisadParamName, true);

            playerDamaged = (!isPlayerReinforce) ? playerDamageBase * _damageMagnification :  playerDamageBase * _damageMagnificationIsReinforce;
            aiDamaged = (!isAiReinforce) ? aiDamageBase / _damageMagnification : aiDamageBase / _damageMagnificationIsReinforce;

            return (playerDamaged, aiDamaged);
        }

        // プレイヤーが与えるダメージダウン
        if (aiCommandAttributeId == disadvantageAttributeId)
        {
            _seAudio.PlayOneShot(_disadbantageAttackSE);
            _playerRivalryAnim.SetBool(_rivalryDisadParamName, true);
            _aiRivalryAnim.SetBool(_rivalryAdParamName, true);

            playerDamaged = (!isPlayerReinforce) ? playerDamageBase / _damageMagnification : playerDamageBase / _damageMagnificationIsReinforce;
            aiDamaged = (!isAiReinforce) ? aiDamageBase * _damageMagnification : aiDamageBase * _damageMagnificationIsReinforce;

            return (playerDamaged, aiDamaged);
        }

        // 基礎ダメージそのまま返す
        _seAudio.PlayOneShot(_attackSE);
        return (playerDamageBase, aiDamageBase);
    }

    /// <summary>
    /// 【相生】属性相性によってダメージを増幅
    /// </summary>
    /// <param name="damageBase">処理前ダメージ</param>
    /// <param name="commandAttributeId">自コマンドの属性ID</param>
    /// <param name="otherCommandAttributeId">相手コマンドの属性ID</param>
    /// <returns>処理後ダメージ</returns>
    public int Amplification(int damageBase, int commandAttributeId, int otherCommandAttributeId, bool isReinforce, bool isPlayer)
    {
        int otherAttributeId = -1;  // 相手のコマンドの属性ID

        switch (commandAttributeId)
        {
            // 水
            case 1:
                otherAttributeId = 5;
                break;
            // 木
            case 2:
                otherAttributeId = 1;
                break;
            // 火
            case 3:
                otherAttributeId = 2;
                break;
            // 土
            case 4:
                otherAttributeId = 3;
                break;
            // 金
            case 5:
                otherAttributeId = 4;
                break;
            // デフォルト(絶対に通らない)
            default:
                Debug.Log("処理ミス");
                break;
        }

        // ダメージアップ
        if (otherCommandAttributeId == otherAttributeId)
        {
            if (isPlayer)
            {
                _playerAmplificationAnim.SetBool(_amplificationParamName, true);
            }

            if (!isPlayer)
            {
                _aiAmplificationAnim.SetBool(_amplificationParamName, true);
            }

            // 比和発生時
            if (isReinforce)
            {
                return damageBase + _damageUpValueIsReinforce;
            }

            return damageBase + _damageUpValue;
        }

        // 基礎ダメージをそのまま返す
        return damageBase;
    }

    /// <summary>
    /// 【比和】キャラクターとコマンドの属性が同じか判定
    /// </summary>
    /// <param name="commandAttributeId">コマンドの属性ID</param>
    /// <param name="character">キャラクター</param>
    /// <returns>true/false</returns>
    public bool Reinforce(int commandAttributeId, Character character, bool isPlayer)
    {
        // 発生
        if (commandAttributeId == character.AttributeId)
        {
            _seAudio.PlayOneShot(_reinforceSE);

            if (isPlayer) 
            { 
                _playerReinforceAnim.SetBool(_reinforceParamName, true);
            }

            if (!isPlayer) 
            {
                _aiReinforceAnim.SetBool(_reinforceParamName, true);
            }

            return true;
        }

        return false;
    }

    public void AnimParametersReset()
    {
        _playerReinforceAnim.SetBool(_reinforceParamName, false);
        _aiReinforceAnim.SetBool(_reinforceParamName, false);
        _playerRivalryAnim.SetBool(_rivalryAdParamName, false);
        _aiRivalryAnim.SetBool(_rivalryDisadParamName, false);
        _playerRivalryAnim.SetBool(_rivalryDisadParamName, false);
        _aiRivalryAnim.SetBool(_rivalryAdParamName, false);
        _playerAmplificationAnim.SetBool(_amplificationParamName, false);
        _aiAmplificationAnim.SetBool(_amplificationParamName, false);
    }
}
