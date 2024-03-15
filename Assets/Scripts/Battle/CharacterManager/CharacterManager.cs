using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    // �X�N���v�g
    [SerializeField] protected SelectCharacterData SelectCharacterData;

    [Header("�L�����N�^�[")]
    [SerializeField] protected CharacterDataBase CharacterDataBase;     // �L�����N�^�[�̃f�[�^�x�[�XObject
    public Character SelectCharacter { get; protected set; }            // �I�����ꂽ�L�����N�^�[

    // �L�����N�^�[�̉摜
    protected Image CharacterImage;

    protected virtual void Awake() { }
}
