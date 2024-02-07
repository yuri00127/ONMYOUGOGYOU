using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    // �X�N���v�g���擾����I�u�W�F�N�g
    private const string _selectCharacterData = "SelectCharacterData";

    // �X�N���v�g
    protected SelectCharacterData SelectCharacterData;

    [Header("�L�����N�^�[")]
    [SerializeField] protected CharacterDataBase CharacterDataBase;     // �L�����N�^�[�̃f�[�^�x�[�XObject
    public Character SelectCharacter { get; protected set; }              // �I�����ꂽ�L�����N�^�[

    // �L�����N�^�[�̉摜
    protected Image CharacterImage;


    protected virtual void Awake()
    {
        // �X�N���v�g�̎擾
        SelectCharacterData = GameObject.Find(_selectCharacterData).GetComponent<SelectCharacterData>();
    }
}
