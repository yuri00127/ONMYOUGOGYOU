using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// キャラクターのデータ構造
[CreateAssetMenu]
[SerializeField]
public class Character : MonoBehaviour
{
    public int Id;
    public string Name;
    public int TypeId;
    public string[] CommandNames = new string[5];
    public Sprite[] CommandSprites = new Sprite[5];
}
