using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [Header("スクリプト")]
    [SerializeField] private CommandManager _commandManager;
    [SerializeField] private AICharacterManager _AICharacterManager;

    private int _nowTurn = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // コマンドバトルを行う
    public void Battle()
    {
        BattleResult();
    }

    // コマンドバトルの結果を取得
    private void BattleResult()
    {

    }
}
