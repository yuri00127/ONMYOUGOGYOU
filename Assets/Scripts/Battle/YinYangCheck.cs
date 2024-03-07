using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YinYangCheck : MonoBehaviour
{
    [Header("Audio")]
    private const string _seManagerObjName = "SEManager";
    private const string _bgmManagerObjName = "BGMManager";
    private AudioSource _seAudio;
    private AudioSource _bgmAudio;
    [SerializeField] private AudioClip _differSE;   // 陰陽互根SE

    // IconAnimation
    [SerializeField] GameObject _playerBattleIconObj;
    [SerializeField] GameObject _aiBattleIconObj;
    private const string _restrictionIconObjName = "YinYangRestriction";
    private const string _differIconObjName = "YinYangDiffer";
    private Animator _playerRestrictionAnim;
    private Animator _playerDifferAnim;
    private Animator _aiRestrictionAnim;
    private Animator _aiDifferAnim;
    private const string _restrictionParamName = "IsRestriction";
    private const string _differParamName = "IsDiffer";


    private void Awake()
    {
        _seAudio = GameObject.Find(_seManagerObjName).GetComponent<AudioSource>();
        _bgmAudio = GameObject.Find(_bgmManagerObjName).GetComponent<AudioSource>();

        _playerRestrictionAnim = _playerBattleIconObj?.transform.Find(_restrictionIconObjName).gameObject?.GetComponent<Animator>();
        _playerDifferAnim = _playerBattleIconObj?.transform.Find(_differIconObjName).gameObject?.GetComponent<Animator>();
        _aiRestrictionAnim = _aiBattleIconObj?.transform.Find(_restrictionIconObjName).gameObject?.GetComponent<Animator>();
        _aiDifferAnim = _aiBattleIconObj?.transform.Find(_differIconObjName).gameObject?.GetComponent<Animator>() ;
    }

    /// <summary>
    /// 【陰陽制約】コマンドの陰陽が全て同じなら、どれかを違うものに変更する
    /// </summary>
    /// <param name="yinYangCommandList"></param>
    /// <param name="commandManager"></param>
    public void Restriction(ref List<bool> yinYangCommandList, CommandManager commandManager, bool isPlayer)
    {
        int yinCommand = 0;
        int yangCommand = 0;

        for (int i = 0; i < yinYangCommandList.Count; i++)
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
    /// <param name="playerYinYang"></param>
    /// <param name="aiYinYang"></param>
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

    public void AnimParametersReset()
    {
        _playerDifferAnim.SetBool(_differParamName, false);
        _aiDifferAnim.SetBool(_differParamName, false);
    }

    public void RestrictionAnimParametersReset()
    {
        _playerRestrictionAnim.SetBool(_restrictionParamName, false);
        _aiRestrictionAnim.SetBool(_restrictionParamName, false);
    }
}
