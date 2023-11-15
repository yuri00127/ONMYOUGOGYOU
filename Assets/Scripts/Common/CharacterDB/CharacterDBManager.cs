using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// データの管理（追加・削除など）を行う
public class CharacterDBManager : MonoBehaviour
{
    [SerializeField] private CharacterDataBase _characterDataBase;

    public void AddCharacterData(Character character)
    {
        _characterDataBase.CharacterList.Add(character);
    }

    public void DeleteCharacterData()
    {
        
    }
}
