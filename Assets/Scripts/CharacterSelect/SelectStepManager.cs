using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectStepManager : MonoBehaviour
{
    // 選択段階（自キャラクター、敵キャラクター、敵AIレベル）
    public int NowSelectStep { get; private set; } = 0;

    [Header("デフォルトボタン")]
    [SerializeField] private GameObject _AICharacterDefaultButton;
    [SerializeField] private GameObject _AILevelDefaultButton;

    // プレイヤーの使用キャラクターを選択して、敵キャラ選択へ
    public void PlayerCharacterSelect(GameObject playerCharacter)
    {
        EventSystem.current.SetSelectedGameObject(_AICharacterDefaultButton);
        NowSelectStep++;
    }

    // AIの使用キャラクターを選択して、AIレベル選択へ
    public void AICharacterSelect(GameObject AICharacter)
    {
        EventSystem.current.SetSelectedGameObject(_AILevelDefaultButton);
        NowSelectStep++;
    }

    // AIの強さのレベルを選択して、バトル画面へ遷移
    public void AILevelSelect(GameObject level)
    {

    }
}
