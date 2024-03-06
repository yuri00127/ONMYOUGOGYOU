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
    [SerializeField] private AudioClip _differSE;   // ‰A—zŒİªSE


    // ‰A—zŒİª
    public bool Differ(bool playerYinYang, bool aiYinYang)
    {
        // UŒ‚‚ª’Ê‚é
        if (playerYinYang != aiYinYang) 
        {
            return true;
        }

        // UŒ‚‚ª’Ê‚ç‚È‚¢
        _seAudio.PlayOneShot(_differSE);
        return false;
    }

    // ‰A—z§–ñ
    public void Restriction(bool[] yinYangCommandList)
    {
        int yinCommand = 0;
        int yangCommand = 0;

        // ‘I‘ğ‚³‚ê‚½ƒRƒ}ƒ“ƒh‚Ì‹C‚ª‘S‚Ä“¯ˆê‚É‚È‚ç‚È‚¢‚æ‚¤‚É‚·‚é
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
            // 5‚Â–Ú‚ÌƒRƒ}ƒ“ƒh‚Ì‹C‚ğ‹­§“I‚Éˆá‚¤‚à‚Ì‚É‚·‚é
        }
    }
}
