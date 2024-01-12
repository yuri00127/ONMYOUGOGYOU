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

    // Audio
    private const string _seManagerObjName = "SEManager";
    private const string _bgmManagerObjName = "BGMManager";
    private AudioSource _seAudio;
    private AudioSource _bgmAudio;
    [SerializeField] private AudioClip _disadbantageAttackSE;
    [SerializeField] private AudioClip _attackSE;
    [SerializeField] private AudioClip _advantageousAttackSE;
    [SerializeField] private AudioClip _harmonySE;             // ��a��������SE
    [SerializeField] private AudioClip _contradictSE;          // �ł�������������SE
    [SerializeField] private AudioClip _battleFinishJingle;    // �o�g���I�����̃W���O��

    // Animation
    [SerializeField] private ParticleSystem _waterAttackParticle;
    [SerializeField] private ParticleSystem _treeAttackParticle;
    [SerializeField] private ParticleSystem _fireAttackParticle;
    [SerializeField] private ParticleSystem _soilAttackParticle;
    [SerializeField] private ParticleSystem _goldAttackParticle;
    private Vector3 _playerPos;
    private const string _playerObjName = "PlayerCharacter";
    private Vector3 _aiPos;
    private const string _aiObjName = "AICharacter";


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

        _seAudio = GameObject.Find(_seManagerObjName).GetComponent<AudioSource>();
        _bgmAudio = GameObject.Find(_bgmManagerObjName).GetComponent<AudioSource>();

        _playerPos = GameObject.Find(_playerObjName).transform.position;
        _aiPos = GameObject.Find(_aiObjName).transform.position;
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
    }

    /// <summary>
    /// �R�}���h�o�g�����s��
    /// </summary>
    public void Battle()
    {
        // �����̔���
        int[,] attributeResult = AttributeCheck();

        // �_���[�W�̌���
        int[,] damageResult = DamageCheck(attributeResult);

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
    /// [n,2] ��������(-1:�s��,0:����,1:�L��)
    /// [n,3] �ł������̗L��(0:�Ȃ�,1:����)
    /// [n,4] ��a�̗L��(0:�Ȃ�,1:����)
    /// [n,5] �G�̔�a�̗L��(0:�Ȃ�,1:����)
    /// </summary>
    /// <param name="attributeResult">[n,0]�����A[n,1]��a</param>
    /// <returns>�R�}���h���Ƃ̃_���[�W�ʂ̔z��</returns>
    private int[,] DamageCheck(int[,] attributeResult)
    {
        int[,] result = new int[3,6];               // �m�肵���_���[�W�̔z��
        int[] damaged = new int[] { 15, 5, 10 };    // �L���A�s���A�ʏ�̃_���[�W��
        bool isContradict = false;                  // �U���ł����������̗L��

        // �R�}���h�̏��Ԃ��ƂɌ��ʂ�z��Ɋi�[
        for (int i = 0; i < result.GetLength(0); i++)
        {
            // �U���̑ł��������������Ă��邩���`�F�b�N
            isContradict = IsContradict(_playerCommandManager.IsYinList[i], _aiCommandManager.IsYinList[i]);

            result[i, 3] = 0;

            if (isContradict)
            {
                result[i, 3] = 1;
            }

            // �v���C���[���L���Ȏ�
            if (attributeResult[i,0] == 1)
            {   
                result[i, 2] = 1;

                // �_���[�W
                result[i, 0] = damaged[0];
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
                result[i, 2] = -1;

                // �_���[�W
                result[i, 0] = damaged[1];
                result[i,1] = damaged[0];

                continue;
            }

            // �������Ȃ��Ƃ�
            if (attributeResult[i,0] == 0)
            {
                result[i, 2] = 0;

                // �_���[�W
                result[i, 0] = damaged[2];
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
            result[i, 4] = 0;
            result[i, 5] = 0;

            // ���R�}���h
            if (attributeResult[i, 1] == 1)
            {
                result[i, 0] = (int)(result[i, 0] * 1.5);
                result[i, 4] = 1;
            }

            // ����R�}���h
            if (attributeResult[i, 1] == -1)
            {
                result[i, 1] = (int)(result[i, 1] * 1.5);
                result[i, 5] = 1;
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
        var wait = new WaitForSeconds(2f);
        yield return new WaitForSeconds(0.5f);

        // �R�}���h�̏��ԂɃ_���[�W���m��
        for (int i = 0; i < damageResult.GetLength(0); i++)
        {
            // �U���A�j���[�V����
            if (damageResult[i, 2] == 1)
            {
                // �L��
                _seAudio.PlayOneShot(_advantageousAttackSE);
            }

            if (damageResult[i, 2] == 0)
            {
                // ����
                _seAudio.PlayOneShot(_attackSE);
            }

            if (damageResult[i, 2] == -1)
            {
                // �s��
                _seAudio.PlayOneShot(_disadbantageAttackSE);
            }

            // �ł������A�j���[�V����
            bool isContradict = false;
            if (damageResult[i, 3] == 1)
            {
                _seAudio.PlayOneShot(_contradictSE);
                isContradict = true;
            }

            // ��a�A�j���[�V����
            bool isPlayerHarmony = false;
            if (damageResult[i, 4] == 1)
            {
                _seAudio.PlayOneShot(_harmonySE);
                isPlayerHarmony = true;
            }

            bool isAiHarmony = false;
            if (!isContradict && damageResult[i, 5] == 1)
            {
                _seAudio.PlayOneShot(_harmonySE);
                isAiHarmony = true;
            }

            AttackAnimation(_playerCommandManager.CommandIdList[i] + 1, _aiCommandManager.CommandIdList[i] + 1,
                isContradict, isPlayerHarmony, isAiHarmony);

            // �G�ւ̃_���[�W���m��
            _aiHpSlider.value -= damageResult[i, 0];

            // �����ւ̃_���[�W���m��
            _playerHpSlider.value -= damageResult[i, 1];

            yield return wait;

            // ����
            if (_aiHpSlider.value <= 0)
            {
                _isFinish = true;
                StartCoroutine(BattleFinish(true));
                break;
            }

            // �s�k
            if (_playerHpSlider.value <= 0)
            {
                _isFinish = true;
                StartCoroutine(BattleFinish(false));
                break;
            }

            //yield return wait;
        }

        // ���݂���HP���c���Ă���΁A���̃��E���h��
        if (!_isFinish)
        {
            StartCoroutine(NextRound());
        }
    }

    private void AttackAnimation(int playerCommandAttributeId, int aiCommandAttributeId, bool isContradict, bool isPlayerHarmony, bool isAiHarmony)
    {
        ParticleSystem playerDamageParticle = null;
        ParticleSystem aiDamageParticle = null;

        // �v���C���[�ւ̍U��
        switch (playerCommandAttributeId)
        {
            case 1:
                aiDamageParticle = Instantiate(_waterAttackParticle, _aiPos, Quaternion.identity);
                break;
            case 2:
                aiDamageParticle = Instantiate(_treeAttackParticle, _aiPos, Quaternion.identity);
                break;
            case 3:
                aiDamageParticle = Instantiate(_fireAttackParticle, _aiPos, Quaternion.identity);
                break;
            case 4:
                aiDamageParticle = Instantiate(_soilAttackParticle, _aiPos, Quaternion.identity);
                break;
            case 5:
                aiDamageParticle = Instantiate(_goldAttackParticle, _aiPos, Quaternion.identity);
                break;
            default:
                break;
        }

        // ��a
        if (isPlayerHarmony)
        {
            aiDamageParticle.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }

        aiDamageParticle.Play();
        Destroy(aiDamageParticle.gameObject, 0.5f);

        // �v���C���[�ւ̍U��
        if (!isContradict)
        {
            switch (aiCommandAttributeId)
            {
                case 1:
                    playerDamageParticle = Instantiate(_waterAttackParticle, _playerPos, Quaternion.identity);
                    break;
                case 2:
                    playerDamageParticle = Instantiate(_treeAttackParticle, _playerPos, Quaternion.identity);
                    break;
                case 3:
                    playerDamageParticle = Instantiate(_fireAttackParticle, _playerPos, Quaternion.identity);
                    break;
                case 4:
                    playerDamageParticle = Instantiate(_soilAttackParticle, _playerPos, Quaternion.identity);
                    break;
                case 5:
                    playerDamageParticle = Instantiate(_goldAttackParticle, _playerPos, Quaternion.identity);
                    break;
                default:
                    break;
            }

            // ��a
            if (isAiHarmony)
            {
                playerDamageParticle.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }

            playerDamageParticle.Play();
            Destroy(playerDamageParticle.gameObject, 0.5f);
        }
    }

    private IEnumerator NextRound()
    {
        //yield return new WaitForSeconds(3f);

        // �R�}���h������
        _playerCommandManager.SelectingCommandSequence = 0;
        _playerCommandManager.IsAllSelect = false;
        _playerCommandManager.CommandReset();
        _playerCommandManager.CommandIdList.Clear();
        _playerCommandManager.IsYinList.Clear();
        _aiCommandManager.CommandIdList.Clear();
        _aiCommandManager.IsYinList.Clear();
        _aiCommandManager.SetAICommand();

        yield return new WaitForSeconds(0.5f);

        // ���E���h�\�����X�V
        _nowRound++;
        _roundCounter.CountUp(_nowRound);

        yield return new WaitForSeconds(1f);
    }

    private IEnumerator BattleFinish(bool playerWin)
    {
        Debug.Log("�o�g���I��");

        _bgmAudio.clip = _battleFinishJingle;
        _bgmAudio.Play();

        if (playerWin) { Debug.Log("�v���C���[�̏���"); }
        if (!playerWin) { Debug.Log("�G�̏���"); }

        yield return new WaitForSeconds(1f);

        // ���s�A�j���[�V����
    }
}
