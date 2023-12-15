using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    // �X�N���v�g���擾����I�u�W�F�N�g
    private const string _playerCommandManagerObjName = "PlayerCommandManager";
    private const string _aiCommandManagerObjName = "AICommandManager";
    private const string _roundCounterObjName = "RoundCounter";
    

    // �X�N���v�g
    private PlayerCommandManager _playerCommandManager;
    private AICommandManager _aiCommandManager;
    private RoundCounter _roundCounter;

    // HP
    private const string _playerHpObjName = "PlayerCharacterHP";
    private const string _auHpObjName = "AICharacterHP";
    private Slider _playerHpSlider;
    private Slider _aiHpSlider;

    private int _nowRound = 1;  // ���݂̃��E���h

    private void Awake()
    {
        // �X�N���v�g�擾
        _playerCommandManager = GameObject.Find(_playerCommandManagerObjName).GetComponent<PlayerCommandManager>();
        _aiCommandManager = GameObject.Find(_aiCommandManagerObjName).GetComponent<AICommandManager>();
        _roundCounter = GameObject.Find(_roundCounterObjName).GetComponent<RoundCounter>();

        // HP��Slider�擾
        _playerHpSlider = GameObject.Find(_playerHpObjName).GetComponent<Slider>();
        _aiHpSlider = GameObject.Find(_auHpObjName).GetComponent<Slider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // �J�n���̃A�j���[�V����

        
    }

    private IEnumerator StartAnimation()
    {
        // HP�o�[�𖞃^���ɂ���
        while (_playerHpSlider.value != 1)
        {
            _playerHpSlider.value += 0.01f;
            _aiHpSlider.value += 0.01f;
        }

        yield return new WaitForSeconds(0.3f);

        // ���E���h��\������

    }

    // �R�}���h�o�g�����s��
    public void Battle()
    {
        // �o�g�����ʎ擾
        BattleResult();

        // ���̃��E���h��
        _nowRound++;
        _roundCounter.CountUp(_nowRound);
    }

    // �R�}���h�o�g���̌��ʂ��擾
    private void BattleResult()
    {
        // �����̔���
        int[,] attributeResult = AttributeCheck();

        // �_���[�W�̌���
        int[,] damageResult = DamageCheck(attributeResult);

        // �A�j���[�V����

        // �_���[�W�𔽉f

    }

    /// <summary>
    /// �R�}���h�����̑�������
    /// [n,0] ��������̌���(1:�L���A-1:�s���A0:�ǂ���ł��Ȃ�)
    /// [n,1] ��a�̗L��(0:���A1:�L)
    /// [n,2] �G�̔�a�̗L��
    /// </summary>
    /// <returns>���茋�ʂ̔z��</returns>
    private int[,] AttributeCheck()
    {
        int[,] result = new int[3,3];
        int playerAdvantageous = 0;
        int playerDisadvantage = 0;

        for (int i = 0; i <= result.Length; i++)
        {
            int playerCommandAttributeId = _playerCommandManager.CommandIdList[i];
            bool isHarmony = false;
                
            // �v���C���[�̃R�}���h�����̗L���E�s�����擾
            var attributeCompatibility = AttributeCompativilityCheck(playerCommandAttributeId);
            playerAdvantageous = attributeCompatibility.playerAdvantageous;
            playerDisadvantage = attributeCompatibility.playerDisadvantage;

            // �G�R�}���h�̑������擾
            int aiCommandAttributeId = _aiCommandManager.CommandIdList[i];

            // �L�����N�^�[�ƃR�}���h�̑�������v���Ă��邩�`�F�b�N
            if (IsHarmony(_playerCommandManager.SelectCharacter.AttributeId, playerCommandAttributeId))
            {
                result[i, 1] = 1;
            }
            result[i, 1] = 0;

            // �G�L�����N�^�[�ƓG�R�}���h�̑�������v���Ă��邩�`�F�b�N
            if (IsHarmony(_aiCommandManager.SelectCharacter.AttributeId, aiCommandAttributeId))
            {
                result[i, 2] = 1;
            }
            result[i, 2] = 0;

            // �v���C���[���L���Ȏ�
            if (aiCommandAttributeId == playerAdvantageous)
            {
                result[i, 0] = 1;
                continue;
            }

            // �v���C���[���s���Ȏ�
            if (aiCommandAttributeId == playerDisadvantage)
            {
                result[i, 0] = -1;
                continue;
            }

            // ���������݂��Ȃ��Ƃ�
            result[i, 0] = 0;
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
    /// [n,0] �G�ւ̃_���[�W��
    /// [n,1] �����ւ̃_���[�W��
    /// </summary>
    /// <param name="attributeResult">[n,0]�����A[n,1]��a�A[n,2]�G�̔�a</param>
    /// <returns>�R�}���h���Ƃ̃_���[�W�ʂ̔z��</returns>
    private int[,] DamageCheck(int[,] attributeResult)
    {
        int[,] result = new int[3,2];
        bool isContradict = false;
        bool isPlayerHarmony = false;   // �L�����̑����ƃR�}���h�̑�������v���Ă��邩(�v���C���[)
        bool isAiHarmony = false;       // �L�����̑����ƃR�}���h�̑�������v���Ă��邩(�G)

        for (int i = 0; i >= result.Length; i++)
        {
            // �����̈�v���m�F

            // �U���̑ł��������������Ă��邩�`�F�b�N
            if (IsContradict(_playerCommandManager.IsYinList[i], _playerCommandManager.IsYinList[i]))
            {
                isContradict = true;
            }

            // �v���C���[���L���Ȏ�
            if (attributeResult[i,0] == 1)
            {
                result[i, 0] = 20;
                
                // �����ւ̃_���[�W
                if (!isContradict)
                {
                    result[i, 1] = 5;
                }

                continue;
            }

            // �v���C���[���s���Ȏ�
            if (attributeResult[i,0] == -1)
            {
                result[i,1] = 20;
                continue;
            }

            // �������Ȃ��Ƃ�
            if (attributeResult[i,0] == 0)
            {
                result[i, 0] = 10;

                // �����ւ̃_���[�W
                if (!isContradict)
                {
                    result[i, 1] = 10;
                }
                
                continue;
            }

        }

        return result;
    }


    /// <summary>
    /// �L�����N�^�[�̑����ƃR�}���h�̑�������v���Ă��邩�`�F�b�N
    /// </summary>
    /// <param name="characterAttributeId"></param>
    /// <param name="commandAttributeId"></param>
    /// <returns></returns>
    private bool IsHarmony(int characterAttributeId, int commandAttributeId)
    { 
        if (characterAttributeId == commandAttributeId) { return true; }

        return false;
    }

    /// <summary>
    /// �U���̑ł��������������邩�`�F�b�N
    /// ������ƈقȂ�C��I������Ƒł�����������
    /// �������s���̎��͑ł������Ȃ�
    /// </summary>
    /// <param name="playerMind">�v���C���[�̃R�}���h�̋C</param>
    /// <param name="aiMind">�G�̃R�}���h�̋C</param>
    /// <returns>�ł�����������������true</returns>
    private bool IsContradict(bool playerMind, bool aiMind)
    {
        if (playerMind != aiMind) { return true; }

        return false;
    }

}
