using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �L�����N�^�[�̃f�[�^�\��
[CreateAssetMenu]
[SerializeField]
public class Character : ScriptableObject
{
    public int Id;                                          // ID
    public string Name;                                     // ���O
    public int AttributeId;                                 // ������ID
    public Sprite CharacterImage;                           // �摜
    public Sprite[] CommandSprites = new Sprite[5];         // �R�}���h�̉摜
    public Sprite[] SelectCommandSprites = new Sprite[5];   // �I����Ԃ̃R�}���h�̉摜
}
