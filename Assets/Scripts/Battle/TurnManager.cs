using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [Header("�X�N���v�g")]
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

    // �R�}���h�o�g�����s��
    public void Battle()
    {
        BattleResult();
    }

    // �R�}���h�o�g���̌��ʂ��擾
    private void BattleResult()
    {

    }
}
