using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 五行システムの判定を行う
/// </summary>
public class WuXingCheck : MonoBehaviour
{
    [Header("アイコン")]
    [SerializeField] private GameObject _playerRivalryIcon;
    [SerializeField] private GameObject _aiRivalryIcon;
    [SerializeField] private Sprite[] _rivalryIconSprites = new Sprite[2];

    // ダメージ計算
    private int _damageMagnification = 2;             // 相剋の倍率
    private int _damageMagnificationIsReinforce = 3;  // 比和時の相剋倍率
    private int _damageUpValue = 5;                 　// 相生によるダメージUP量
    private int _damageUpValueIsReinforce = 8;        // 比和時の相生ダメージUP量

    [Header("Audio")]
    [SerializeField] private AudioSource _seAudio;
    [SerializeField] private AudioSource _bgmAudio;
    [SerializeField] private AudioClip _reinforceSE;            // 比和SE
    [SerializeField] private AudioClip _attackSE;               // 攻撃SE
    [SerializeField] private AudioClip _advantageousAttackSE;   // 有利攻撃SE
    [SerializeField] private AudioClip _disadbantageAttackSE;   // 不利攻撃SE

    // IconAnimation
    [SerializeField] GameObject _playerBattleIconObj;
    [SerializeField] GameObject _aiBattleIconObj;
    [SerializeField] private Animator _playerReinforceAnim;
    [SerializeField] private Animator _playerRivalryAnim;
    [SerializeField] private Animator _playerAmplificationAnim;
    [SerializeField] private Animator _aiReinforceAnim;
    [SerializeField] private Animator _aiRivalryAnim;
    [SerializeField] private Animator _aiAmplificationAnim;
    private const string _reinforceParamName = "IsReinforce";
    private const string _rivalryAdParamName = "IsRivalry_Advantageous";
    private const string _rivalryDisadParamName = "IsRivalry_Disadvantage";
    private const string _amplificationParamName = "IsAmplification";

    private enum Attribute
    {
        Water = 1,
        Tree = 2,
        Fire = 3,
        Soil = 4,
        Gold = 5
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
        var advantageousAttributeId = -1;   // プレイヤーが有利な属性ID
        var disadvantageAttributeId = -1;   // プレイヤーが不利な属性ID

        int playerDamaged;  // プレイヤーが与えるダメージ
        int aiDamaged;      // プレイヤーが受けるダメージ

        switch (playerCommandAttributeId)
        {
            case (int)Attribute.Water:
                advantageousAttributeId = (int)Attribute.Fire;
                disadvantageAttributeId = (int)Attribute.Soil;
                break;
            case (int)Attribute.Tree:
                advantageousAttributeId = (int)Attribute.Soil;
                disadvantageAttributeId = (int)Attribute.Gold;
                break;
            case (int)Attribute.Fire:
                advantageousAttributeId = (int)Attribute.Gold;
                disadvantageAttributeId = (int)Attribute.Water;
                break;
            case (int)Attribute.Soil:
                advantageousAttributeId = (int)Attribute.Water;
                disadvantageAttributeId = (int)Attribute.Tree;
                break;
            case (int)Attribute.Gold:
                advantageousAttributeId = (int)Attribute.Tree;
                disadvantageAttributeId = (int)Attribute.Fire;
                break;
            default:
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
        var validityOtherAttributeId = -1;  // 有効時の相手コマンドの属性ID

        switch (commandAttributeId)
        {
            case (int)Attribute.Water:
                validityOtherAttributeId = (int)Attribute.Gold;
                break;
            case (int)Attribute.Tree:
                validityOtherAttributeId = (int)Attribute.Water;
                break;
            case (int)Attribute.Fire:
                validityOtherAttributeId = (int)Attribute.Tree;
                break;
            case (int)Attribute.Soil:
                validityOtherAttributeId = (int)Attribute.Fire;
                break;
            case (int)Attribute.Gold:
                validityOtherAttributeId = (int)Attribute.Soil;
                break;
            default:
                break;
        }

        // ダメージアップ
        if (otherCommandAttributeId == validityOtherAttributeId)
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

    /// <summary>
    /// 全てのアイコンアニメーションのフラグをリセットする
    /// </summary>
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
