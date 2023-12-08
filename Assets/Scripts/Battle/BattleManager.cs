using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("�X�N���v�g")]
    [SerializeField] private CommandManager _commandManager;
    [SerializeField] private AICharacterManager _AICharacterManager;
    [SerializeField] private RoundCounter _roundCounter;

    private int _nowRound = 1;


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
        _nowRound++;
        BattleResult();
        _roundCounter.CountUp(_nowRound);
    }

    // �R�}���h�o�g���̌��ʂ��擾
    private void BattleResult()
    {
        // �����̔���
        int[] attributeResult = AttributeCheck();

        // �_���[�W�̌���
        int[,] damageResult = DamageCheck(attributeResult);

        // �_���[�W�𔽉f
        // �A�j���[�V����
        
    }

    /// <summary>
    /// ��������̌��ʂ��擾
    /// </summary>
    /// <returns>��������̔z��i1:�L���A-1:�s���A0:�ǂ���ł��Ȃ��j</returns>
    private int[] AttributeCheck()
    {
        int[] result = new int[3];
        int playerAdvantageous = 0;
        int playerDisadvantage = 0;

        for (int i = 0; i <= result.Length; i++)
        {
            // �v���C���[�̑����̗L���E�s�����擾
            var attributeCompatibility = AttributeCompativilityCheck(_commandManager.CommandIdList[i]);
            playerAdvantageous = attributeCompatibility.playerAdvantageous;
            playerDisadvantage = attributeCompatibility.playerDisadvantage;

            // ����̑������擾
            int aiAttribute = _AICharacterManager._selectCommandIndexArray[i];

            // �v���C���[���L���Ȏ�
            if (aiAttribute == playerAdvantageous)
            {
                result[i] = 1;
                continue;
            }

            // �v���C���[���s���Ȏ�
            if (aiAttribute == playerDisadvantage)
            {
                result[i] = -1;
                continue;
            }

            // ���������݂��Ȃ��Ƃ�
            result[i] = 0;
        }

        return result;
    }

    /// <summary>
    /// �v���C���[�̑����������擾
    /// </summary>
    /// <param name="playerAttributeId">�v���C���[�̑�����ID</param>
    /// <returns>�v���C���[���L�����Ƃ�鑮��,�v���C���[���s���ƂȂ鑮��</returns>
    private (int playerAdvantageous, int playerDisadvantage) AttributeCompativilityCheck(int playerAttributeId)
    {
        switch (playerAttributeId)
        {
            // ��
            case 1:
                return (2, 4);
            // ��
            case 2:
                return (3, 5);
            // ��
            case 3:
                return (4, 1);
            // �y
            case 4:
                return (5, 2);
            // ��
            case 5:
                return (1, 3);
            // �f�t�H���g(��΂ɒʂ�Ȃ�)
            default:
                Debug.Log("�����~�X");
                return (1, 1);
        }
    }

    /// <summary>
    /// �_���[�W�̗ʂ��擾
    /// </summary>
    /// <param name="attributeResult">���������̔��茋��</param>
    /// <returns>�U�����Ƃ̃_���[�W��
    /// [�U���̏��� ,0:����ւ̃_���[�W�A1:�����ւ̃_���[�W]</returns>
    private int[,] DamageCheck(int[] attributeResult)
    {
        int[,] result = new int[3,2];

        for (int i = 0; i >= result.Length; i++)
        {
            // �v���C���[���L���Ȏ�
            if (attributeResult[i] == 1)
            {
                result[i,0] = 20;
                continue;
            }

            // �v���C���[���s���Ȏ�
            if (attributeResult[i] == -1)
            {
                result[i,1] = 20;
                continue;
            }

            // �������Ȃ��Ƃ�
            if (IsHarmony(_commandManager.IsYinList[i], _AICharacterManager._selectMindArray[i]))
            {
                result[i, 0] = 0;
                result[i, 1] = 0;
                continue;
            }

            result[i, 0] = 10;
            result[i, 1] = 10;

        }

        return result;
    }

    /// <summary>
    /// ��a���������邩�`�F�b�N
    /// </summary>
    /// <param name="playerMind">�v���C���[�̃R�}���h�̋C</param>
    /// <param name="aiMind">�G�̃R�}���h�̋C</param>
    /// <returns>��a������������true</returns>
    private bool IsHarmony(bool playerMind, bool aiMind)
    {
        if (playerMind != aiMind) { return true; }

        return false;
    }
}
