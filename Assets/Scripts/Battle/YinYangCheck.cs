using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YinYangCheck : MonoBehaviour
{
    // �A�z�ݍ�
    public int Differ(int damage, bool playerYinYang, bool aiYinYang)
    {
        // �U�����ʂ�
        if (playerYinYang != aiYinYang) 
        {
            return damage;
        }

        // �U�����ʂ�Ȃ�
        return 0;
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
