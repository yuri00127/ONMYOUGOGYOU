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
    [SerializeField] private AudioClip _differSE;   // ‰A—zŒİªSE

    // IconAnimation
    [SerializeField] public Animator _playerRestrictionAnim;
    [SerializeField] public Animator _playerDifferAnim;
    [SerializeField] public Animator _aiRestrictionAnim;
    [SerializeField] public Animator _aiDifferAnim;
    private const string _restrictionParamName = "IsRestriction";
    private const string _differParamName = "IsDiffer";


    private void Awake()
    {
        _seAudio = GameObject.Find(_seManagerObjName).GetComponent<AudioSource>();
        _bgmAudio = GameObject.Find(_bgmManagerObjName).GetComponent<AudioSource>();
    }

    /// <summary>
    /// y‰A—z§–ñzƒRƒ}ƒ“ƒh‚Ì‰A—z‚ª‘S‚Ä“¯‚¶‚È‚çA‚Ç‚ê‚©‚ğˆá‚¤‚à‚Ì‚É•ÏX‚·‚é
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

        // ƒAƒCƒRƒ“ƒAƒjƒ[ƒVƒ‡ƒ“
        if (isPlayer)
        {
            _playerRestrictionAnim.SetBool(_restrictionParamName, true);
        }

        if (!isPlayer)
        {
            _aiRestrictionAnim.SetBool(_restrictionParamName, true);
        }

        // ƒ‰ƒ“ƒ_ƒ€‚ÈƒRƒ}ƒ“ƒh‚Ì‹C‚ğ1‚Â•ÏX‚·‚é
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
    /// y‰A—zŒİªz‚¨Œİ‚¢‚ÌƒRƒ}ƒ“ƒh‚Ì‰A—z‚ª“¯‚¶ê‡AUŒ‚‚Í–³Œø‚Æ‚È‚é
    /// </summary>
    /// <param name="playerYinYang"></param>
    /// <param name="aiYinYang"></param>
    /// <returns></returns>
    public bool Differ(bool playerYinYang, bool aiYinYang)
    {
        // UŒ‚‚ª’Ê‚é
        if (playerYinYang != aiYinYang)
        {
            return true;
        }

        // UŒ‚‚ª’Ê‚ç‚È‚¢
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
