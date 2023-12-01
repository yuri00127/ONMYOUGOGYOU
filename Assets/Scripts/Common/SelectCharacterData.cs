using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacterData : MonoBehaviour
{
    public string SavePlayerCharacterId { get; private set;} = "PlayerCharacter";
    public string SaveAICharacterId { get; private set; } = "AICharacter";
    public string SaveAILevel { get; private set;} = "AILevel";

}
