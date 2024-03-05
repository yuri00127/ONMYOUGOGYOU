using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YinYangCheck : MonoBehaviour
{
    // 陰陽互根
    public int Differ(int damage, bool playerYinYang, bool aiYinYang)
    {
        // 攻撃が通る
        if (playerYinYang != aiYinYang) 
        {
            return damage;
        }

        // 攻撃が通らない
        return 0;
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
