using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("スクリプト")]
    [SerializeField] protected SelectCharacterData SelectCharacterData;

    [Header("キャラクター")]
    [SerializeField] protected CharacterDataBase CharacterDataBase;
    protected Character SelectCharacter;


    protected virtual void Start()
    {
        
    }
}
