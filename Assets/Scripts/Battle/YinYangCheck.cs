using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YinYangCheck : MonoBehaviour
{
    [Header("Audio")]
    private const string _seManagerObjName = "SEManager";
    private const string _bgmManagerObjName = "BGMManager";
    private AudioSource _seAudio;
    private AudioSource _bgmAudio;
    [SerializeField] private AudioClip _differSE;   // �A�z�ݍ�SE


    // �A�z�ݍ�
    public bool Differ(bool playerYinYang, bool aiYinYang)
    {
        // �U�����ʂ�
        if (playerYinYang != aiYinYang) 
        {
            return true;
        }

        // �U�����ʂ�Ȃ�
        _seAudio.PlayOneShot(_differSE);
        return false;
    }

    // �A�z����
    public void Restriction(bool[] yinYangCommandList)
    {
        int yinCommand = 0;
        int yangCommand = 0;

        // �I�����ꂽ�R�}���h�̋C���S�ē���ɂȂ�Ȃ��悤�ɂ���
        for (int i = 0; i < yinYangCommandList.Length; i++)
        {
            if (yinYangCommandList[i])
            {
                yangCommand++;
                continue;
            }

            yinCommand++;
        }

        if (yinCommand == 0 || yangCommand == 0) 
        {
            // 5�ڂ̃R�}���h�̋C�������I�ɈႤ���̂ɂ���
        }
    }
}
