using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("�X�N���v�g")]
    [SerializeField] protected SelectCharacterData SelectCharacterData;

    [Header("�L�����N�^�[")]
    [SerializeField] protected CharacterDataBase CharacterDataBase;
    protected Character SelectCharacter;


    protected virtual void Start()
    {
        
    }
}
