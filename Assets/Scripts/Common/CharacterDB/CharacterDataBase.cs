using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �쐬���ꂽ�f�[�^���Ǘ�
[CreateAssetMenu]
[SerializeField]
public class CharacterDataBase : ScriptableObject
{
    public List<Character> CharacterList = new List<Character>();
}
