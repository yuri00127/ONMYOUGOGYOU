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
    private int _maxHp = 100;

    private bool _isFirstAnimation = false;    // �ŏ��̃A�j���[�V�������s��ꂽ��
    private int _nowRound = 1;                 // ���݂̃��E���h
    private bool _isFinish = false;            // ���s�����肵�Ă��邩

    private void Awake()
    {
        // �X�N���v�g�擾
        _playerCommandManager = GameObject.Find(_playerCommandManagerObjName).GetComponent<PlayerCommandManager>();
        _aiCommandManager = GameObject.Find(_aiCommandManagerObjName).GetComponent<AICommandManager>();
        _roundCounter = GameObject.Find(_roundCounterObjName).GetComponent<RoundCounter>();

        // HP��Slider�擾
        _playerHpSlider = GameObject.Find(_playerHpObjName).GetComponent<Slider>();
        _aiHpSlider = GameObject.Find(_auHpObjName).GetComponent<Slider>();

        // HP�̏����ݒ�
        _playerHpSlider.value = 0;
        _aiHpSlider.value = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        // �J�n���A�j���[�V����
        StartCoroutine(StartAnimation());
        
    }

    private void Update()
    {
        // �J�n���A�j���[�V����
        if (!_isFirstAnimation)
        {
            _playerHpSlider.value = Mathf.Lerp(_playerHpSlider.value, _maxHp, 0.04f);
            _aiHpSlider.value = Mathf.Lerp(_aiHpSlider.value, _maxHp, 0.04f);
        }
    }

    /// <summary>
    /// �J�n���̃A�j���[�V����
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(1f);

        // HP�o�[�𖞃^���ɂ���
        _isFirstAnimation = true;   // �A�j���[�V����
        _playerHpSlider.value = _maxHp;
        _aiHpSlider.value = _maxHp;

        // ���E���h��\������

    }

    // �R�}���h�o�g�����s��
    public void Battle()
    {
        // �o�g�����ʎ擾
        BattleResult();

        // ���݂���HP���c���Ă���΁A���̃��E���h��
        if (!_isFinish)
        {
            StartCoroutine(NextRound());
        }

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
        StartCoroutine(DamageResult(damageResult));

    }

    /// <summary>
    /// �R�}���h�����̑�������
    /// [n,0] ��������̌���(1:�L���A-1:�s���A0:�ǂ���ł��Ȃ�)
    /// [n,1] ��a�̗L��(0:���A1:�����A-1:�G)
    /// </summary>
    /// <returns>���茋�ʂ̔z��</returns>
    private int[,] AttributeCheck()
    {
        int[,] result = new int[3,2];
        int playerAdvantageous = 0;
        int playerDisadvantage = 0;

        // �R�}���h�̏��Ԃ��ƂɌ��ʂ�z��Ɋi�[
        for (int i = 0; i < result.GetLength(0); i++)
        {
            int playerCommandAttributeId = _playerCommandManager.CommandIdList[i] + 1;
                
            // �v���C���[�̃R�}���h�����̗L���E�s�����擾
            var attributeCompatibility = AttributeCompativilityCheck(playerCommandAttributeId);
            playerAdvantageous = attributeCompatibility.playerAdvantageous;
            playerDisadvantage = attributeCompatibility.playerDisadvantage;

            // �G�R�}���h�̑������擾
            int aiCommandAttributeId = _aiCommandManager.CommandIdList[i] + 1;

            // ��a�̃`�F�b�N
            result[i, 1] = 0;

            // �L�����N�^�[�ƃR�}���h�̑�������v���Ă��邩�`�F�b�N
            if (IsHarmony(_playerCommandManager.SelectCharacter.AttributeId, playerCommandAttributeId))
            {
                result[i, 1] = 1;
            }
            
            // �G�L�����N�^�[�ƓG�R�}���h�̑�������v���Ă��邩�`�F�b�N
            if (IsHarmony(_aiCommandManager.SelectCharacter.AttributeId, aiCommandAttributeId))
            {
                result[i, 1] = -1;
            }

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
    private (int playerAdvantageous, int playerDisadvantage) AttributeCompativilityCheck(int playerCommandAttributeId)
    {
        switch (playerCommandAttributeId)
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
    /// <param name="attributeResult">[n,0]�����A[n,1]��a</param>
    /// <returns>�R�}���h���Ƃ̃_���[�W�ʂ̔z��</returns>
    private int[,] DamageCheck(int[,] attributeResult)
    {
        int[,] result = new int[3,2];               // �m�肵���_���[�W�̔z��
        int[] damaged = new int[] { 15, 5, 10 };    // �L���A�s���A�ʏ�̃_���[�W��
        bool isContradict = false;                  // �U���ł����������̗L��

        // �R�}���h�̏��Ԃ��ƂɌ��ʂ�z��Ɋi�[
        for (int i = 0; i < result.GetLength(0); i++)
        {
            // �U���̑ł��������������Ă��邩���`�F�b�N
            isContradict = IsContradict(_playerCommandManager.IsYinList[i], _aiCommandManager.IsYinList[i]);

            // �v���C���[���L���Ȏ�
            if (attributeResult[i,0] == 1)
            {
                // �G�ւ̃_���[�W
                result[i, 0] = damaged[0];

                // �����ւ̃_���[�W
                result[i, 1] = damaged[1];

                // �ł�����������
                if (isContradict)
                {
                    result[i, 1] = 0;
                }

                continue;
            }

            // �v���C���[���s���Ȏ�
            if (attributeResult[i,0] == -1)
            {
                // �G�ւ̃_���[�W
                result[i, 0] = damaged[1];

                // �����ւ̃_���[�W
                result[i,1] = damaged[0];

                continue;
            }

            // �������Ȃ��Ƃ�
            if (attributeResult[i,0] == 0)
            {
                // �G�ւ̃_���[�W
                result[i, 0] = damaged[2];

                // �����ւ̃_���[�W
                result[i, 1] = damaged[2];

                // �ł�����������
                if (isContradict)
                {
                    result[i, 1] = 0;
                }

                continue;
            }
        }

        // ��a�̏���(�_���[�W��1.5�{�ɂ���)
        for (int i = 0; i < result.GetLength(0); i++)
        {
            // ���R�}���h
            if (attributeResult[i, 1] == 1)
            {
                result[i, 0] = (int)(result[i, 0] * 1.5);
            }

            // ����R�}���h
            if (attributeResult[i, 1] == -1)
            {
                result[i, 1] = (int)(result[i, 1] * 1.5);
            }

            //Debug.Log(string.Format("{0}�Ԗڂ̃R�}���h ", i + 1) + "����_���[�W:" + result[i, 0]);
            //Debug.Log(string.Format("{0}�Ԗڂ̃R�}���h ", i + 1) + "���_���[�W:" + result[i, 1]);
        }
        
        return result;
    }

    /// <summary>
    /// �L�����N�^�[�̑����ƃR�}���h�̑�������v���Ă��邩
    /// </summary>
    /// <param name="characterAttributeId">�L�����N�^�[�̑���</param>
    /// <param name="commandAttributeId">�R�}���h�̑���</param>
    /// <returns></returns>
    private bool IsHarmony(int characterAttributeId, int commandAttributeId)
    { 
        if (characterAttributeId == commandAttributeId) { return true; }

        return false;
    }

    /// <summary>
    /// �U���̑ł��������������邩
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

    /// <summary>
    /// �_���[�W���Q�[�W�ɔ��f����
    /// </summary>
    /// <param name="damageResult"></param>
    /// <returns></returns>
    private IEnumerator DamageResult(int[,] damageResult)
    {
        var wait = new WaitForSeconds(1f);

        // �R�}���h�̏��ԂɃ_���[�W���m��
        for (int i = 0; i < damageResult.GetLength(0); i++)
        {
            // �G�ւ̃_���[�W���m��
            _aiHpSlider.value -= damageResult[i, 0];

            // ����
            if (_aiHpSlider.value <= 0)
            {
                _isFinish = true;
                StartCoroutine(BattleFinish(true));
                break;
            }
            // �����ւ̃_���[�W���m��
            _playerHpSlider.value -= damageResult[i, 1];

            // �s�k
            if (_playerHpSlider.value <= 0)
            {
                _isFinish = true;
                StartCoroutine(BattleFinish(false));
                break;
            }

            yield return wait;
        }

    }

    private IEnumerator NextRound()
    {
        yield return new WaitForSeconds(3f);

        // �R�}���h������
        _playerCommandManager.SelectingCommandSequence = 0;
        _playerCommandManager.IsAllSelect = false;
        _playerCommandManager.CommandReset();
        _playerCommandManager.CommandIdList.Clear();
        _playerCommandManager.IsYinList.Clear();
        _aiCommandManager.CommandIdList.Clear();
        _aiCommandManager.IsYinList.Clear();
        _aiCommandManager.SetAICommand();

        // ���E���h�\�����X�V
        _nowRound++;
        _roundCounter.CountUp(_nowRound);

        yield return new WaitForSeconds(1f);
    }

    private IEnumerator BattleFinish(bool playerWin)
    {
        Debug.Log("�o�g���I��");

        if (playerWin) { Debug.Log("�v���C���[�̏���"); }
        if (!playerWin) { Debug.Log("�G�̏���"); }

        yield return new WaitForSeconds(1f);

        // ���s�A�j���[�V����
    }
}
