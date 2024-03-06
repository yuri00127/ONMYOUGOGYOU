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
    [SerializeField] private AudioClip _differSE;   // 陰陽互根SE


    // 陰陽互根
    public bool Differ(bool playerYinYang, bool aiYinYang)
    {
        // 攻撃が通る
        if (playerYinYang != aiYinYang) 
        {
            return true;
        }

        // 攻撃が通らない
        _seAudio.PlayOneShot(_differSE);
        return false;
    }

    // 陰陽制約
    public void Restriction(bool[] yinYangCommandList)
    {
        int yinCommand = 0;
        int yangCommand = 0;

        // 選択されたコマンドの気が全て同一にならないようにする
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
            // 5つ目のコマンドの気を強制的に違うものにする
        }
    }
}
