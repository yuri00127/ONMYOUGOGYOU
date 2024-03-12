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
    private WuXingCheck _wuXingCheck;
    private YinYangCheck _yinYangCheck;
    private BattleResult _battleResult;

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
    private int _aiDamageBase = 8;         // �G�̊�b�_���[�W��

    [Header("Animation")]
    [SerializeField] private ParticleSystem _waterAttackParticle;   // �������U���G�t�F�N�g
    [SerializeField] private ParticleSystem _treeAttackParticle;    // �ؑ����U���G�t�F�N�g
    [SerializeField] private ParticleSystem _fireAttackParticle;    // �Α����U���G�t�F�N�g
    [SerializeField] private ParticleSystem _soilAttackParticle;    // �y�����U���G�t�F�N�g
    [SerializeField] private ParticleSystem _goldAttackParticle;    // �������U���G�t�F�N�g
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
        _yinYangCheck = this.GetComponent<YinYangCheck>();
        _battleResult = this.GetComponent<BattleResult>();

        // �A�j���[�V�����`��ʒu���擾
        _playerPos = GameObject.Find(_playerObjName).transform.position;
        _aiPos = GameObject.Find(_aiObjName).transform.position;

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
    }

    /// <summary>
    /// �I�����ꂽ�R�}���h�ɂ��o�g�����J�n����
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoBattleStart()
    {
        List<int> playerCommandList = _playerCommandManager.CommandIdList;  // �v���C���[�̃R�}���h�̑���ID���X�g
        List<bool> playerIsYinList = _playerCommandManager.IsYinList;       // �v���C���[�̃R�}���h�̉A�z���X�g
        List<int> aiCommandList = _aiCommandManager.CommandIdList;          // AI�̃R�}���h�̑���ID���X�g
        List<bool> aiIsYinList = _aiCommandManager.IsYinList;               // AI�̃R�}���h�̉A�z���X�g

        var isPlayerReinforce = false;
        var isAiReinforce = false;

        var wait = new WaitForSeconds(2.0f);

        // 1���E���h�̏���
        for (var i = 0; i < playerCommandList.Count; i++)
        {
            int playerDamage = _playerDamageBase;
            int aiDamage = _aiDamageBase;

            // �y�A�z����z�R�}���h�̉A�z���S�ē����Ȃ�A�ǂꂩ���Ⴄ���̂ɕύX����
            _yinYangCheck.Restriction(ref playerIsYinList, _playerCommandManager, true);
            _yinYangCheck.Restriction(ref aiIsYinList, _aiCommandManager, false);

            // �y�A�z�ݍ��z���݂��̉A�z�������Ȃ�A�_���[�W�͔������Ȃ�
            if (!_yinYangCheck.Differ(playerIsYinList[i], aiIsYinList[i]))
            {
                yield return wait;
                _yinYangCheck.AnimParametersReset();
                continue;
            }

            // �y��a�z�L�����N�^�[�ƃR�}���h�̑����������Ȃ�A�ȍ~�̌��ʂ𑝕�����
            isPlayerReinforce = _wuXingCheck.Reinforce(playerCommandList[i] + 1, _playerCommandManager.SelectCharacter, true);
            isAiReinforce = _wuXingCheck.Reinforce(aiCommandList[i] + 1, _aiCommandManager.SelectCharacter, false);

            // �y�����z���������ɂ��A�_���[�W�𑝌�
            var rivalryDamage = _wuXingCheck.Rivalry(playerDamage, aiDamage, playerCommandList[i] + 1, aiCommandList[i] + 1, isPlayerReinforce, isAiReinforce);
            playerDamage = rivalryDamage.playerDamaged;
            aiDamage = rivalryDamage.aiDamaged;

            // �y�����z���������ɂ��A�_���[�W�𑝕�
            playerDamage = _wuXingCheck.Amplification(playerDamage, playerCommandList[i] + 1, aiCommandList[i] + 1, isPlayerReinforce, true);
            aiDamage = _wuXingCheck.Amplification(aiDamage, aiCommandList[i] + 1, playerCommandList[i] + 1, isAiReinforce, false);

            // �U���A�j���[�V����
            AttackAnimation(playerCommandList[i] + 1, isPlayerReinforce, _aiPos);
            AttackAnimation(aiCommandList[i] + 1, isAiReinforce, _playerPos);

            // �G�ւ̃_���[�W���m��
            _aiHpSlider.value -= playerDamage;

            // �v���C���[�ւ̃_���[�W���m��
            _playerHpSlider.value -= aiDamage;

            yield return wait;

            _wuXingCheck.AnimParametersReset();

            yield return new WaitForSeconds(1.0f);

            // �s�k
            if (_playerHpSlider.value <= 0)
            {
                _isFinish = true;
                StartCoroutine(_battleResult.CoBattleFinish(false));
                break;
            }

            // ����
            if (_aiHpSlider.value <= 0)
            {
                _isFinish = true;
                StartCoroutine(_battleResult.CoBattleFinish(true));
                break;
            }
        }

        // ���݂���HP���c���Ă���΁A���̃��E���h��
        if (!_isFinish)
        {
            StartCoroutine(NextRound());
        }
    }

    /// <summary>
    /// �U���A�j���[�V����
    /// </summary>
    /// <param name="commandAttributeId">�R�}���h�̑���ID</param>
    /// <param name="isReinforce">��a�̗L��</param>
    private void AttackAnimation(int commandAttributeId, bool isReinforce, Vector3 pos)
    {
        ParticleSystem damageParticle = null;

        switch (commandAttributeId)
        {
            case 1:
                damageParticle = Instantiate(_waterAttackParticle, pos, Quaternion.identity);
                break;
            case 2:
                damageParticle = Instantiate(_treeAttackParticle, pos, Quaternion.identity);
                break;
            case 3:
                damageParticle = Instantiate(_fireAttackParticle, pos, Quaternion.identity);
                break;
            case 4:
                damageParticle = Instantiate(_soilAttackParticle, pos, Quaternion.identity);
                break;
            case 5:
                damageParticle = Instantiate(_goldAttackParticle, pos, Quaternion.identity);
                break;
            default:
                break;
        }

        if (isReinforce)
        {
            damageParticle.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }

        damageParticle.Play();
        Destroy(damageParticle.gameObject, 0.5f);
    }

    private IEnumerator NextRound()
    {
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
        _yinYangCheck.RestrictionAnimParametersReset();

        yield return new WaitForSeconds(1f);
    }
}
