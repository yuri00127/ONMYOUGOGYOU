using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �܍s�V�X�e���̔�����s��
/// </summary>
public class WuXingCheck : MonoBehaviour
{
    [Header("Icon")]
    [SerializeField] private GameObject _playerRivalryIcon;
    [SerializeField] private GameObject _aiRivalryIcon;
    [SerializeField] private Sprite[] _rivalryIconSprites = new Sprite[2];

    // �_���[�W�v�Z
    private int _damageMagnification = 2;             // �����̔{��
    private int _damageMagnificationIsReinforce = 3;  // ��a���̑����{��
    private int _damageUpValue = 5;                 �@// �����ɂ��_���[�WUP��
    private int _damageUpValueIsReinforce = 8;        // ��a���̑����_���[�WUP��

    [Header("Audio")]
    private const string _seManagerObjName = "SEManager";
    private const string _bgmManagerObjName = "BGMManager";
    private AudioSource _seAudio;
    private AudioSource _bgmAudio;
    [SerializeField] private AudioClip _reinforceSE;            // ��aSE
    [SerializeField] private AudioClip _attackSE;               // �U��SE
    [SerializeField] private AudioClip _advantageousAttackSE;   // �L���U��SE
    [SerializeField] private AudioClip _disadbantageAttackSE;   // �s���U��SE

    // IconAnimation
    [SerializeField] public Animator _playerAnim;
    [SerializeField] public Animator _aiAnim;
    private const string _reinforceParamName = "IsReinforce";
    private const string _rivalryAdParamName = "IsRivalry_Advantageous";
    private const string _rivalryDisadParamName = "IsRivalry_Disadvantage";
    private const string _AmplificationParamName = "IsAmplification";


    private void Awake()
    {
        _seAudio = GameObject.Find(_seManagerObjName).GetComponent<AudioSource>();
        _bgmAudio = GameObject.Find(_bgmManagerObjName).GetComponent<AudioSource>();
    }

    /// <summary>
    /// �y�����z���������ɂ���ă_���[�W�{����ω�
    /// </summary>
    /// <param name="playerDamageBase">�v���C���[���^���鏈���O�_���[�W</param>
    /// <param name="aiDamageBase">AI���^���鏈���O�_���[�W</param>
    /// <param name="playerCommandAttributeId">�v���C���[�̃R�}���h�̑���ID</param>
    /// <param name="aiCommandAttributeId">AI�̃R�}���h�̑���ID</param>
    /// <returns>�v���C���[���^���鏈����_���[�W�AAI���^���鏈����_���[�W</returns>
    public (int playerDamaged, int aiDamaged) Rivalry (int playerDamageBase, int aiDamageBase,
        int playerCommandAttributeId, int aiCommandAttributeId, bool isPlayerReinforce, bool isAiReinforce)
    {
        int advantageousAttributeId = -1;   // �v���C���[���L���ȑ���ID
        int disadvantageAttributeId = -1;   // �v���C���[���s���ȑ���ID

        int playerDamaged = 0;
        int aiDamaged = 0;

        switch (playerCommandAttributeId)
        {
            // ��
            case 1:
                advantageousAttributeId = 3;
                disadvantageAttributeId = 4;
                break;
            // ��
            case 2:
                advantageousAttributeId = 4;
                disadvantageAttributeId = 5;
                break;
            // ��
            case 3:
                advantageousAttributeId = 5;
                disadvantageAttributeId = 1;
                break;
            // �y
            case 4:
                advantageousAttributeId = 1;
                disadvantageAttributeId = 2;
                break;
            // ��
            case 5:
                advantageousAttributeId = 2;
                disadvantageAttributeId = 3;
                break;
            // �f�t�H���g(��΂ɒʂ�Ȃ�)
            default:
                Debug.Log("�����~�X");
                break;
        }

        // �v���C���[���^����_���[�W�A�b�v
        if (aiCommandAttributeId == advantageousAttributeId)
        {
            _seAudio.PlayOneShot(_advantageousAttackSE);
            _playerAnim.SetBool(_rivalryAdParamName, true);
            _aiAnim.SetBool(_rivalryDisadParamName, true);

            playerDamaged = playerDamageBase * _damageMagnification;
            aiDamaged = aiDamageBase / _damageMagnification;

            if (isPlayerReinforce)
            {
                playerDamaged = playerDamageBase * _damageMagnificationIsReinforce;
            }

            if (isAiReinforce)
            {
                aiDamaged = aiDamaged / _damageMagnificationIsReinforce;
            }

            return (playerDamaged, aiDamaged);
        }

        // �v���C���[���^����_���[�W�_�E��
        if (aiCommandAttributeId == disadvantageAttributeId)
        {
            _seAudio.PlayOneShot(_disadbantageAttackSE);
            _playerAnim.SetBool(_rivalryDisadParamName, true);
            _aiAnim.SetBool(_rivalryAdParamName, true);

            playerDamaged = playerDamageBase / _damageMagnification;
            aiDamaged = aiDamageBase * _damageMagnification;

            if (isPlayerReinforce)
            {
                playerDamaged = playerDamageBase / _damageMagnificationIsReinforce;
            }

            if (isAiReinforce)
            {
                aiDamaged = aiDamaged * _damageMagnificationIsReinforce;
            }

            return (playerDamaged, aiDamaged);
        }

        // ��b�_���[�W���̂܂ܕԂ�
        _seAudio.PlayOneShot(_attackSE);
        return (playerDamageBase, aiDamageBase);
    }

    /// <summary>
    /// �y�����z���������ɂ���ă_���[�W�𑝕�
    /// </summary>
    /// <param name="damageBase">�����O�_���[�W</param>
    /// <param name="commandAttributeId">���R�}���h�̑���ID</param>
    /// <param name="otherCommandAttributeId">����R�}���h�̑���ID</param>
    /// <returns>������_���[�W</returns>
    public int Amplification(int damageBase, int commandAttributeId, int otherCommandAttributeId, bool isReinforce, bool isPlayer)
    {
        int otherAttributeId = -1;  // ����̃R�}���h�̑���ID

        switch (commandAttributeId)
        {
            // ��
            case 1:
                otherAttributeId = 5;
                break;
            // ��
            case 2:
                otherAttributeId = 1;
                break;
            // ��
            case 3:
                otherAttributeId = 2;
                break;
            // �y
            case 4:
                otherAttributeId = 3;
                break;
            // ��
            case 5:
                otherAttributeId = 4;
                break;
            // �f�t�H���g(��΂ɒʂ�Ȃ�)
            default:
                Debug.Log("�����~�X");
                break;
        }

        // �_���[�W�A�b�v
        if (otherCommandAttributeId == otherAttributeId)
        {
            if (isPlayer)
            {
                _playerAnim.SetBool(_AmplificationParamName, true);
            }

            if (!isPlayer)
            {
                _aiAnim.SetBool(_AmplificationParamName, true);
            }

            // ��a������
            if (isReinforce)
            {
                return damageBase + _damageUpValueIsReinforce;
            }

            return damageBase + _damageUpValue;
        }

        // ��b�_���[�W�����̂܂ܕԂ�
        return damageBase;
    }

    /// <summary>
    /// �y��a�z�L�����N�^�[�ƃR�}���h�̑���������������
    /// </summary>
    /// <param name="commandAttributeId">�R�}���h�̑���ID</param>
    /// <param name="character">�L�����N�^�[</param>
    /// <returns>true/false</returns>
    public bool Reinforce(int commandAttributeId, Character character, bool isPlayer)
    {
        // ����
        if (commandAttributeId == character.AttributeId)
        {
            _seAudio.PlayOneShot(_reinforceSE);

            if (isPlayer) 
            { 
                _playerAnim.SetBool(_reinforceParamName, true);
            }

            if (!isPlayer) 
            {
                _aiAnim.SetBool(_reinforceParamName, true);
            }

            return true;
        }

        return false;
    }

    public void AnimParametersReset()
    {
        _playerAnim.SetBool(_reinforceParamName, false);
        _aiAnim.SetBool(_reinforceParamName, false);
        _playerAnim.SetBool(_rivalryAdParamName, false);
        _aiAnim.SetBool(_rivalryDisadParamName, false);
        _playerAnim.SetBool(_rivalryDisadParamName, false);
        _aiAnim.SetBool(_rivalryAdParamName, false);
        _playerAnim.SetBool(_AmplificationParamName, false);
        _aiAnim.SetBool(_AmplificationParamName, false);
    }
}
