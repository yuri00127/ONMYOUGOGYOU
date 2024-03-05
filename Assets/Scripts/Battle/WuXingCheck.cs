using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 五行システムの判定を行う
/// </summary>
public class WuXingCheck : MonoBehaviour
{
    // 相剋
    public void Rivalry(int playerCommandAttributeId, int aiCommandAttributeId)
    {
        int advantageousAttributeId = -1;
        int disadvantageAttributeId = -1;

        switch (playerCommandAttributeId)
        {
            // 水
            case 1:
                advantageousAttributeId = 3;
                disadvantageAttributeId = 4;
                break;
            // 木
            case 2:
                advantageousAttributeId = 4;
                disadvantageAttributeId = 5;
                break;
            // 火
            case 3:
                advantageousAttributeId = 5;
                disadvantageAttributeId = 1;
                break;
            // 土
            case 4:
                advantageousAttributeId = 1;
                disadvantageAttributeId = 2;
                break;
            // 金
            case 5:
                advantageousAttributeId = 2;
                disadvantageAttributeId = 3;
                break;
            // デフォルト(絶対に通らない)
            default:
                Debug.Log("処理ミス");
                break;
        }

        if (aiCommandAttributeId == advantageousAttributeId)
        {
            // ダメージアップ
            return;
        }

        if (aiCommandAttributeId == disadvantageAttributeId)
        {
            // ダメージダウン
            return;
        }

        // 基礎ダメージそのまま返す
        return;
    }

    // 相生
    public int Amplification(int playerCommandAttributeId, int aiCommandAttributeId)
    {
        int otherAttributeId = -1;

        switch (playerCommandAttributeId)
        {
            // 水
            case 1:
                otherAttributeId = 5;
                break;
            // 木
            case 2:
                otherAttributeId = 1;
                break;
            // 火
            case 3:
                otherAttributeId = 2;
                break;
            // 土
            case 4:
                otherAttributeId = 3;
                break;
            // 金
            case 5:
                otherAttributeId = 4;
                break;
            // デフォルト(絶対に通らない)
            default:
                Debug.Log("処理ミス");
                break;
        }

        // ダメージアップ
        if (otherAttributeId == aiCommandAttributeId)
        {
            // 基礎ダメージを追加して返す
        }

        // 基礎ダメージをそのまま返す
        return 0;
    }
}
