using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    // �X�N���v�g���擾����I�u�W�F�N�g
    private const string _selectCharacterData = "SelectCharacterData";

    // �X�N���v�g
    protected SelectCharacterData SelectCharacterData;

    [Header("�L�����N�^�[")]
    [SerializeField] protected CharacterDataBase CharacterDataBase;     // �L�����N�^�[�̃f�[�^�x�[�XObject
    public Character SelectCharacter { get; protected set; }              // �I�����ꂽ�L�����N�^�[


    protected virtual void Awake()
    {
        // �X�N���v�g�̎擾
        SelectCharacterData = GameObject.Find(_selectCharacterData).GetComponent<SelectCharacterData>();
    }

    protected virtual void Start()
    {
        
    }
}
