using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YinYangCheck : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource _seAudio;
    [SerializeField] private AudioSource _bgmAudio;
    [SerializeField] private AudioClip _differSE;   // 陰陽互根SE

    // IconAnimation
    [SerializeField] GameObject _playerBattleIconObj;
    [SerializeField] GameObject _aiBattleIconObj;
    [SerializeField] private Animator _playerRestrictionAnim;
    [SerializeField] private Animator _playerDifferAnim;
    [SerializeField] private Animator _aiRestrictionAnim;
    [SerializeField] private Animator _aiDifferAnim;
    private const string _restrictionParamName = "IsRestriction";
    private const string _differParamName = "IsDiffer";


    /// <summary>
    /// 【陰陽制約】コマンドの気が全て同じなら、どれかを違うものに変更する
    /// </summary>
    /// <param name="yinYangCommandList">気のboolリスト</param>
    /// <param name="commandManager">対応するキャラクターのCommandManager</param>
    public void Restriction(ref List<bool> yinYangCommandList, CommandManager commandManager, bool isPlayer)
    {
        var yinCommand = 0;
        var yangCommand = 0;

        for (var i = 0; i < yinYangCommandList.Count; i++)
        {
            if (yinYangCommandList[i])
            {
                yangCommand++;
                continue;
            }

            yinCommand++;
        }

        if (yinCommand != yinYangCommandList.Count && yangCommand != yinYangCommandList.Count)
        {
            return;
        }

        // アイコンアニメーション
        if (isPlayer)
        {
            _playerRestrictionAnim.SetBool(_restrictionParamName, true);
        }

        if (!isPlayer)
        {
            _aiRestrictionAnim.SetBool(_restrictionParamName, true);
        }

        // ランダムなコマンドの気を1つ変更する
        int randomCommandIndex = Random.Range(0, yinYangCommandList.Count);

        if (yinCommand == yinYangCommandList.Count) 
        {
            yinYangCommandList[randomCommandIndex] = true;
            commandManager.SelectMind(randomCommandIndex);
            return;
        }

        if (yangCommand == yinYangCommandList.Count)
        {
            yinYangCommandList[randomCommandIndex] = false;
            commandManager.SelectMind(randomCommandIndex);
        }
    }

    /// <summary>
    /// 【陰陽互根】お互いのコマンドの陰陽が同じ場合、攻撃は無効となる
    /// </summary>
    /// <param name="playerYinYang">プレイヤーの気</param>
    /// <param name="aiYinYang">敵の気</param>
    /// <returns></returns>
    public bool Differ(bool playerYinYang, bool aiYinYang)
    {
        // 攻撃が通る
        if (playerYinYang != aiYinYang)
        {
            return true;
        }

        // 攻撃が通らない
        _seAudio.PlayOneShot(_differSE);
        _playerDifferAnim.SetBool(_differParamName, true);
        _aiDifferAnim.SetBool(_differParamName, true);

        return false;
    }

    /// <summary>
    /// 陰陽互換のアニメーションフラグをリセット
    /// </summary>
    public void AnimParametersReset()
    {
        _playerDifferAnim.SetBool(_differParamName, false);
        _aiDifferAnim.SetBool(_differParamName, false);
    }

    /// <summary>
    /// 陰陽制約のアニメーションフラグをリセット
    /// </summary>
    public void RestrictionAnimParametersReset()
    {
        _playerRestrictionAnim.SetBool(_restrictionParamName, false);
        _aiRestrictionAnim.SetBool(_restrictionParamName, false);
    }
}
