using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �܍s�V�X�e���̔�����s��
/// </summary>
public class WuXingCheck : MonoBehaviour
{
    // ����
    public void Rivalry(int playerCommandAttributeId, int aiCommandAttributeId)
    {
        int advantageousAttributeId = -1;
        int disadvantageAttributeId = -1;

        switch (playerCommandAttributeId)
        {
            // ��
            case 1:
                advantageousAttributeId = 3;
                disadvantageAttributeId = 4;
                break;
            // ��
            case 2:
                advantageousAttributeId = 4;
                disadvantageAttributeId = 5;
                break;
            // ��
            case 3:
                advantageousAttributeId = 5;
                disadvantageAttributeId = 1;
                break;
            // �y
            case 4:
                advantageousAttributeId = 1;
                disadvantageAttributeId = 2;
                break;
            // ��
            case 5:
                advantageousAttributeId = 2;
                disadvantageAttributeId = 3;
                break;
            // �f�t�H���g(��΂ɒʂ�Ȃ�)
            default:
                Debug.Log("�����~�X");
                break;
        }

        if (aiCommandAttributeId == advantageousAttributeId)
        {
            // �_���[�W�A�b�v
            return;
        }

        if (aiCommandAttributeId == disadvantageAttributeId)
        {
            // �_���[�W�_�E��
            return;
        }

        // ��b�_���[�W���̂܂ܕԂ�
        return;
    }

    // ����
    public int Amplification(int playerCommandAttributeId, int aiCommandAttributeId)
    {
        int otherAttributeId = -1;

        switch (playerCommandAttributeId)
        {
            // ��
            case 1:
                otherAttributeId = 5;
                break;
            // ��
            case 2:
                otherAttributeId = 1;
                break;
            // ��
            case 3:
                otherAttributeId = 2;
                break;
            // �y
            case 4:
                otherAttributeId = 3;
                break;
            // ��
            case 5:
                otherAttributeId = 4;
                break;
            // �f�t�H���g(��΂ɒʂ�Ȃ�)
            default:
                Debug.Log("�����~�X");
                break;
        }

        // �_���[�W�A�b�v
        if (otherAttributeId == aiCommandAttributeId)
        {
            // ��b�_���[�W��ǉ����ĕԂ�
        }

        // ��b�_���[�W�����̂܂ܕԂ�
        return 0;
    }
}
