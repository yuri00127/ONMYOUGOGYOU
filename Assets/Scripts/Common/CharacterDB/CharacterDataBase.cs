using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 作成されたデータを管理
[CreateAssetMenu]
[SerializeField]
public class CharacterDataBase : MonoBehaviour
{
    public List<Character> CharacterList = new List<Character>();
}
