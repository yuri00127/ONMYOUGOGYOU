using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �f�[�^�̊Ǘ��i�ǉ��E�폜�Ȃǁj���s��
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
