using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YinYangCheck : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource _seAudio;
    [SerializeField] private AudioSource _bgmAudio;
    [SerializeField] private AudioClip _differSE;   // �A�z�ݍ�SE

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
    /// �y�A�z����z�R�}���h�̋C���S�ē����Ȃ�A�ǂꂩ���Ⴄ���̂ɕύX����
    /// </summary>
    /// <param name="yinYangCommandList">�C��bool���X�g</param>
    /// <param name="commandManager">�Ή�����L�����N�^�[��CommandManager</param>
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

        // �A�C�R���A�j���[�V����
        if (isPlayer)
        {
            _playerRestrictionAnim.SetBool(_restrictionParamName, true);
        }

        if (!isPlayer)
        {
            _aiRestrictionAnim.SetBool(_restrictionParamName, true);
        }

        // �����_���ȃR�}���h�̋C��1�ύX����
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
    /// �y�A�z�ݍ��z���݂��̃R�}���h�̉A�z�������ꍇ�A�U���͖����ƂȂ�
    /// </summary>
    /// <param name="playerYinYang">�v���C���[�̋C</param>
    /// <param name="aiYinYang">�G�̋C</param>
    /// <returns></returns>
    public bool Differ(bool playerYinYang, bool aiYinYang)
    {
        // �U�����ʂ�
        if (playerYinYang != aiYinYang)
        {
            return true;
        }

        // �U�����ʂ�Ȃ�
        _seAudio.PlayOneShot(_differSE);
        _playerDifferAnim.SetBool(_differParamName, true);
        _aiDifferAnim.SetBool(_differParamName, true);

        return false;
    }

    /// <summary>
    /// �A�z�݊��̃A�j���[�V�����t���O�����Z�b�g
    /// </summary>
    public void AnimParametersReset()
    {
        _playerDifferAnim.SetBool(_differParamName, false);
        _aiDifferAnim.SetBool(_differParamName, false);
    }

    /// <summary>
    /// �A�z����̃A�j���[�V�����t���O�����Z�b�g
    /// </summary>
    public void RestrictionAnimParametersReset()
    {
        _playerRestrictionAnim.SetBool(_restrictionParamName, false);
        _aiRestrictionAnim.SetBool(_restrictionParamName, false);
    }
}
