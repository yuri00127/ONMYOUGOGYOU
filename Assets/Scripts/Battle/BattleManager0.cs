using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleManager0 : MonoBehaviour
{

    // �X�N���v�g���擾����I�u�W�F�N�g
    private const string _playerCommandManagerObjName = "PlayerCommandManager";
    private const string _aiCommandManagerObjName = "AICommandManager";
    private const string _roundCounterObjName = "RoundCounter";

    // �X�N���v�g
    private PlayerCommandManager _playerCommandManager;
    private AICommandManager _aiCommandManager;
    private RoundCounter _roundCounter;
    private WuXingCheck _wuXingCheck;
    private YinYangCheck _yinYangCheck;
    

    // HP
    private const string _playerHpObjName = "PlayerCharacterHP";
    private const string _auHpObjName = "AICharacterHP";
    private Slider _playerHpSlider;
    private Slider _aiHpSlider;
    private int _maxHp = 100;
    private bool _isFirstAnimation = false;    // �ŏ��̃A�j���[�V�������s��ꂽ��
    private int _nowRound = 1;                 // ���݂̃��E���h
    private bool _isFinish = false;            // ���s�����肵�Ă��邩

    // �_���[�W�v�Z
    private int _playerDamageBase = 5;      // �v���C���[�̊�b�_���[�W��
    private int _aiDamageBase = 15;         // �G�̊�b�_���[�W��
    private int _damageMagnification = 2;   // �L��(*)�A�s��(/)���̔{��
    private int _harmonyDamageUpValue = 3;  // ��a�̃_���[�WUP��
    private int _damageUpValue = 5;         // �����ɂ��_���[�WUP��

    [Header("�_���[�W�\��")]
    [SerializeField] private Sprite[] _damageIcon = new Sprite[5];  // �����A��a�A�����L���A�����s���A�ł�����
    private Sprite[,] _playerDamageSprite = new Sprite[3, 4];       // �v���C���[�̃_���[�W�\��
    private Sprite[,] _aiDamageSprite = new Sprite[3, 3];           // �G�̃_���[�W�\��
    [SerializeField] private GameObject[] _playerDamageIconObj;     // �摜��K�p����Object(�v���C���[)
    [SerializeField] private GameObject[] _aiDamageIconObj;         // �摜��K�p����Object(�G)

    [Header("���s����")]
    [SerializeField] private GameObject _resultCanvas;
    private const string _resultImageObjName = "ResultImage";
    private Image _resultImage;
    private const string _characterImageObjName = "CharacterImage";
    private GameObject _characterImageObj;
    private Image _characterImage;
    private const string _resultDefaultButtonObjName = "BackCharacterSelectButton";
    private GameObject _resultDefaultButtonObj;
    [SerializeField] private Sprite[] _resultSprites = new Sprite[2];   // ���s�摜

    [Header("Audio")]
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

    [Header("Animation")]
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
        _wuXingCheck = this.GetComponent<WuXingCheck>();
        _yinYangCheck = this.GetComponent <YinYangCheck>();

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
        StartCoroutine(DamageResult(attributeResult, damageResult));

    }

    /// <summary>
    /// �R�}���h�����̑�������
    /// [n,0] ��������̌���(1:�L���A-1:�s���A0:�ǂ���ł��Ȃ�)
    /// [n,1] �v���C���[�̔�a����(0:���A1:�L)
    /// [n,2] �G�̔�a����(0:���A1:�L)
    /// [n,3] �_���[�W��������(0:���A1:�L)
    /// </summary>
    /// <returns>���茋�ʂ̔z��</returns>
    private int[,] AttributeCheck()
    {
        int[,] result = new int[3,4];   // ���茋�ʂ̔z��
        int beforeAiCommandId = -1;      // ���O�̓G�̃R�}���h

        // �R�}���h�̏��Ԃ��ƂɌ��ʂ�z��Ɋi�[
        for (int i = 0; i < result.GetLength(0); i++)
        {
            int playerCommandAttributeId = _playerCommandManager.CommandIdList[i] + 1;
                
            // �v���C���[�̃R�}���h�����̗L���E�s�����擾
            var attributeCompatibility = AttributeCompativilityCheck(playerCommandAttributeId);
            int playerAdvantageous = attributeCompatibility.playerAdvantageous;
            int playerDisadvantage = attributeCompatibility.playerDisadvantage;
            int playerDamageUp = attributeCompatibility.playerDamageUp;

            // �G�R�}���h�̑������擾
            int aiCommandAttributeId = _aiCommandManager.CommandIdList[i] + 1;

            // ��a�̃`�F�b�N
            result[i, 1] = 0;
            result[i, 2] = 0;

            if (IsHarmony(_playerCommandManager.SelectCharacter.AttributeId, playerCommandAttributeId))
            {
                result[i, 1] = 1;
            }
            
            if (IsHarmony(_aiCommandManager.SelectCharacter.AttributeId, aiCommandAttributeId))
            {
                result[i, 2] = 1;
            }

            // �_���[�W��������
            result[i, 3] = 0;

            if (i >= 1 && beforeAiCommandId == playerDamageUp)
            {
                result[i, 3] = 1;
            }

            // �G�̃R�}���h���L�^
            beforeAiCommandId = aiCommandAttributeId;

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
    private (int playerAdvantageous, int playerDisadvantage, int playerDamageUp) AttributeCompativilityCheck(int playerCommandAttributeId)
    {
        switch (playerCommandAttributeId)
        {
            // ��
            case 1:
                return (3, 4, 5);
            // ��
            case 2:
                return (4, 5, 1);
            // ��
            case 3:
                return (5, 1, 2);
            // �y
            case 4:
                return (1, 2, 3);
            // ��
            case 5:
                return (2, 3, 4);
            // �f�t�H���g(��΂ɒʂ�Ȃ�)
            default:
                Debug.Log("�����~�X");
                return (1, 1, 1);
        }
    }

    /// <summary>
    /// �_���[�W�̗ʂ��擾
    /// [n,0] �G�ւ̃_���[�W��
    /// [n,1] �v���C���[�ւ̃_���[�W��
    /// [n,2] �ł������̗L��(0:�Ȃ�,1:����)
    /// </summary>
    /// <param name="attributeResult">[n,0]�����A[n,1]�v���C���[��a�A[n,2]�G��a�A[n,3]����</param>
    /// <returns>�R�}���h���Ƃ̃_���[�W�ʂ̔z��</returns>
    private int[,] DamageCheck(int[,] attributeResult)
    {
        int[,] result = new int[3,3];            // �m�肵���_���[�W�̔z��

        // �R�}���h�̏��Ԃ��ƂɌ��ʂ�z��Ɋi�[
        for (int i = 0; i < result.GetLength(0); i++)
        {
            int sumPlayerDamageBase = _playerDamageBase;   // �v���C���[���^�����b�_���[�W�̍��v
            int sumPlayerDamage = 0;                       // �v���C���[���^����_���[�W�̍��v
            int sumAiDamageBase = _aiDamageBase;           // �G���^�����b�_���[�W�̍��v
            int sumAiDamage = 0;                           // �G���^����_���[�W�̍��v
            List<Sprite> playerDamageIcon = new List<Sprite>();    // �v���C���[�̃_���[�W�v�f
            List<Sprite> aiDamageIcon = new List<Sprite>();        // �G�̃_���[�W�v�f

            // ���O�̑��������ɂ��_���[�WUP
            if (attributeResult[i, 3] == 1)
            {
                sumPlayerDamageBase += _damageUpValue;
                playerDamageIcon.Add(_damageIcon[0]);
            }

            // ��a�����ɂ��_���[�WUP
            if (attributeResult[i, 1] == 1)
            {
                sumPlayerDamageBase += _harmonyDamageUpValue;
                playerDamageIcon.Add(_damageIcon[1]);
            }

            if (attributeResult[i, 2] == 1)
            {
                sumAiDamageBase += _harmonyDamageUpValue;
                aiDamageIcon.Add(_damageIcon[1]);
            }

            // �v���C���[���L���Ȏ�
            if (attributeResult[i,0] == 1)
            {
                sumPlayerDamage = sumPlayerDamageBase * _damageMagnification;
                sumAiDamage = sumAiDamageBase / _damageMagnification;
                playerDamageIcon.Add(_damageIcon[2]);
                aiDamageIcon.Add(_damageIcon[3]);
            }

            // �v���C���[���s���Ȏ�
            if (attributeResult[i,0] == -1)
            {
                sumPlayerDamage = sumPlayerDamageBase / _damageMagnification;
                sumAiDamage = sumAiDamageBase * _damageMagnification;
                playerDamageIcon.Add(_damageIcon[3]);
                aiDamageIcon.Add(_damageIcon[2]);
            }

            // �������Ȃ��Ƃ�
            if (attributeResult[i,0] == 0)
            {
                sumPlayerDamage = sumPlayerDamageBase;
                sumAiDamage = sumAiDamageBase;
            }

            // �U���̑ł��������������Ă��邩���`�F�b�N
            result[i, 2] = 0;

            if (attributeResult[i, 0] != -1 && IsContradict(_playerCommandManager.IsYinList[i], _aiCommandManager.IsYinList[i]))
            {
                result[i, 2] = 1;
                sumAiDamage /= 5;
                playerDamageIcon.Add(_damageIcon[4]);
            }

            // �_���[�W�m��
            result[i, 0] = sumPlayerDamage;
            result[i, 1] = sumAiDamage;

            // �_���[�W�A�C�R���\��
            for (int j = 0; j < playerDamageIcon.Count; j++)
            {
                Debug.Log("playerDamageIcon�̒����F" + playerDamageIcon.Count);
                Debug.Log("_playerDamageSprite�̒����F" + _playerDamageSprite[i, j]);
                _playerDamageSprite[i, j] = playerDamageIcon[j];
            }

            for (int j = 0; j < aiDamageIcon.Count; j++)
            {
                _aiDamageSprite[i, j] = aiDamageIcon[j];
            }
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
    private IEnumerator DamageResult(int[,] attributeResult, int[,] damageResult)
    {
        var wait = new WaitForSeconds(2f);
        yield return new WaitForSeconds(0.5f);

        // �R�}���h�̏��ԂɃ_���[�W���m��
        for (int i = 0; i < damageResult.GetLength(0); i++)
        {
            // �_���[�W�\��
            for (int j = 0; j < _playerDamageSprite.GetLength(0); j++)
            {
                Debug.Log("playerDamageSprite�̒����F" + _playerDamageSprite.Length);
                _playerDamageIconObj[j].GetComponent<Image>().sprite = _playerDamageSprite[i, j];
            }

            for (int j = 0; j < _aiDamageSprite.Length; j++)
            {
                _aiDamageIconObj[j].GetComponent<Image>().sprite = _aiDamageSprite[i, j];
            }


            // �U���A�j���[�V����
            if (attributeResult[i, 0] == 1)
            {
                // �L��
                _seAudio.PlayOneShot(_advantageousAttackSE);
            }

            if (attributeResult[i, 0] == 0)
            {
                // ����
                _seAudio.PlayOneShot(_attackSE);
            }

            if (attributeResult[i, 0] == -1)
            {
                // �s��
                _seAudio.PlayOneShot(_disadbantageAttackSE);
            }

            // �ł������A�j���[�V����
            bool isContradict = false;
            if (damageResult[i, 2] == 1)
            {
                _seAudio.PlayOneShot(_contradictSE);
                isContradict = true;
            }

            // ��a�A�j���[�V����
            bool isPlayerHarmony = false;
            if (attributeResult[i, 1] == 1)
            {
                _seAudio.PlayOneShot(_harmonySE);
                isPlayerHarmony = true;
            }

            bool isAiHarmony = false;
            if (!isContradict && attributeResult[i, 2] == 1)
            {
                _seAudio.PlayOneShot(_harmonySE);
                isAiHarmony = true;
            }

            AttackAnimation(_playerCommandManager.CommandIdList[i] + 1, _aiCommandManager.CommandIdList[i] + 1,
                isContradict, isPlayerHarmony, isAiHarmony);
            
            // �f�o�b�O�p
            Debug.Log("�����F" + attributeResult[i, 0]);
            Debug.Log("�����̔�a�F" + attributeResult[i, 1] + "\n����̔�a�F" + attributeResult[i, 2]);
            Debug.Log("�����̗L���F" + attributeResult[i, 3] + "\n�ł������̗L���F" + damageResult[i, 2]);
            Debug.Log("�G�ւ̃_���[�W�F" + damageResult[i, 0] + "\n�v���C���[�ւ̃_���[�W�F" + damageResult[i, 1]);
            

            // �G�ւ̃_���[�W���m��
            _aiHpSlider.value -= damageResult[i, 0];

            // �v���C���[�ւ̃_���[�W���m��
            _playerHpSlider.value -= damageResult[i, 1];

            yield return wait;

            // �s�k
            if (_playerHpSlider.value <= 0)
            {
                _isFinish = true;
                StartCoroutine(BattleFinish(false));
                break;
            }

            // ����
            if (_aiHpSlider.value <= 0)
            {
                _isFinish = true;
                StartCoroutine(BattleFinish(true));
                break;
            }
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
        _aiCommandManager.ShowAICommand();

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
        _bgmAudio.loop = false;

        // ���s���ʂ̕\��
        _resultCanvas.SetActive(true);
        _resultImage = GameObject.Find(_resultImageObjName).GetComponent<Image>();
        _characterImageObj = GameObject.Find(_characterImageObjName);
        _characterImage = _characterImageObj.GetComponent<Image>();
        
        // �v���C���[����
        if (playerWin)
        {
            _resultImage.sprite = _resultSprites[0];
        }

        // �v���C���[�s�k
        if (!playerWin)
        {
            _resultImage.sprite = _resultSprites[1];
        }

        _characterImage.sprite = GameObject.Find("PlayerCharacter").GetComponent<Image>().sprite;
        _characterImageObj.GetComponent<Animator>().SetTrigger("First");

        yield return new WaitForSeconds(1f);

        _resultCanvas.transform.Find("Button").gameObject.SetActive(true);
        _resultDefaultButtonObj = GameObject.Find(_resultDefaultButtonObjName);
        EventSystem.current.SetSelectedGameObject(_resultDefaultButtonObj);

    }
    
}
