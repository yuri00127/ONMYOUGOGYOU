using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �܍s�V�X�e���̔�����s��
/// </summary>
public class WuXingCheck : MonoBehaviour
{
    [Header("�A�C�R��")]
    [SerializeField] private GameObject _playerRivalryIcon;
    [SerializeField] private GameObject _aiRivalryIcon;
    [SerializeField] private Sprite[] _rivalryIconSprites = new Sprite[2];

    // �_���[�W�v�Z
    private int _damageMagnification = 2;             // �����̔{��
    private int _damageMagnificationIsReinforce = 3;  // ��a���̑����{��
    private int _damageUpValue = 5;                 �@// �����ɂ��_���[�WUP��
    private int _damageUpValueIsReinforce = 8;        // ��a���̑����_���[�WUP��

    [Header("Audio")]
    [SerializeField] private AudioSource _seAudio;
    [SerializeField] private AudioSource _bgmAudio;
    [SerializeField] private AudioClip _reinforceSE;            // ��aSE
    [SerializeField] private AudioClip _attackSE;               // �U��SE
    [SerializeField] private AudioClip _advantageousAttackSE;   // �L���U��SE
    [SerializeField] private AudioClip _disadbantageAttackSE;   // �s���U��SE

    // IconAnimation
    [SerializeField] GameObject _playerBattleIconObj;
    [SerializeField] GameObject _aiBattleIconObj;
    [SerializeField] private Animator _playerReinforceAnim;
    [SerializeField] private Animator _playerRivalryAnim;
    [SerializeField] private Animator _playerAmplificationAnim;
    [SerializeField] private Animator _aiReinforceAnim;
    [SerializeField] private Animator _aiRivalryAnim;
    [SerializeField] private Animator _aiAmplificationAnim;
    private const string _reinforceParamName = "IsReinforce";
    private const string _rivalryAdParamName = "IsRivalry_Advantageous";
    private const string _rivalryDisadParamName = "IsRivalry_Disadvantage";
    private const string _amplificationParamName = "IsAmplification";

    private enum Attribute
    {
        Water = 1,
        Tree = 2,
        Fire = 3,
        Soil = 4,
        Gold = 5
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
        var advantageousAttributeId = -1;   // �v���C���[���L���ȑ���ID
        var disadvantageAttributeId = -1;   // �v���C���[���s���ȑ���ID

        int playerDamaged;  // �v���C���[���^����_���[�W
        int aiDamaged;      // �v���C���[���󂯂�_���[�W

        switch (playerCommandAttributeId)
        {
            case (int)Attribute.Water:
                advantageousAttributeId = (int)Attribute.Fire;
                disadvantageAttributeId = (int)Attribute.Soil;
                break;
            case (int)Attribute.Tree:
                advantageousAttributeId = (int)Attribute.Soil;
                disadvantageAttributeId = (int)Attribute.Gold;
                break;
            case (int)Attribute.Fire:
                advantageousAttributeId = (int)Attribute.Gold;
                disadvantageAttributeId = (int)Attribute.Water;
                break;
            case (int)Attribute.Soil:
                advantageousAttributeId = (int)Attribute.Water;
                disadvantageAttributeId = (int)Attribute.Tree;
                break;
            case (int)Attribute.Gold:
                advantageousAttributeId = (int)Attribute.Tree;
                disadvantageAttributeId = (int)Attribute.Fire;
                break;
            default:
                break;
        }

        // �v���C���[���^����_���[�W�A�b�v
        if (aiCommandAttributeId == advantageousAttributeId)
        {
            _seAudio.PlayOneShot(_advantageousAttackSE);
            _playerRivalryAnim.SetBool(_rivalryAdParamName, true);
            _aiRivalryAnim.SetBool(_rivalryDisadParamName, true);

            playerDamaged = (!isPlayerReinforce) ? playerDamageBase * _damageMagnification :  playerDamageBase * _damageMagnificationIsReinforce;
            aiDamaged = (!isAiReinforce) ? aiDamageBase / _damageMagnification : aiDamageBase / _damageMagnificationIsReinforce;

            return (playerDamaged, aiDamaged);
        }

        // �v���C���[���^����_���[�W�_�E��
        if (aiCommandAttributeId == disadvantageAttributeId)
        {
            _seAudio.PlayOneShot(_disadbantageAttackSE);
            _playerRivalryAnim.SetBool(_rivalryDisadParamName, true);
            _aiRivalryAnim.SetBool(_rivalryAdParamName, true);

            playerDamaged = (!isPlayerReinforce) ? playerDamageBase / _damageMagnification : playerDamageBase / _damageMagnificationIsReinforce;
            aiDamaged = (!isAiReinforce) ? aiDamageBase * _damageMagnification : aiDamageBase * _damageMagnificationIsReinforce;

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
        var validityOtherAttributeId = -1;  // �L�����̑���R�}���h�̑���ID

        switch (commandAttributeId)
        {
            case (int)Attribute.Water:
                validityOtherAttributeId = (int)Attribute.Gold;
                break;
            case (int)Attribute.Tree:
                validityOtherAttributeId = (int)Attribute.Water;
                break;
            case (int)Attribute.Fire:
                validityOtherAttributeId = (int)Attribute.Tree;
                break;
            case (int)Attribute.Soil:
                validityOtherAttributeId = (int)Attribute.Fire;
                break;
            case (int)Attribute.Gold:
                validityOtherAttributeId = (int)Attribute.Soil;
                break;
            default:
                break;
        }

        // �_���[�W�A�b�v
        if (otherCommandAttributeId == validityOtherAttributeId)
        {
            if (isPlayer)
            {
                _playerAmplificationAnim.SetBool(_amplificationParamName, true);
            }

            if (!isPlayer)
            {
                _aiAmplificationAnim.SetBool(_amplificationParamName, true);
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
                _playerReinforceAnim.SetBool(_reinforceParamName, true);
            }

            if (!isPlayer) 
            {
                _aiReinforceAnim.SetBool(_reinforceParamName, true);
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// �S�ẴA�C�R���A�j���[�V�����̃t���O�����Z�b�g����
    /// </summary>
    public void AnimParametersReset()
    {
        _playerReinforceAnim.SetBool(_reinforceParamName, false);
        _aiReinforceAnim.SetBool(_reinforceParamName, false);
        _playerRivalryAnim.SetBool(_rivalryAdParamName, false);
        _aiRivalryAnim.SetBool(_rivalryDisadParamName, false);
        _playerRivalryAnim.SetBool(_rivalryDisadParamName, false);
        _aiRivalryAnim.SetBool(_rivalryAdParamName, false);
        _playerAmplificationAnim.SetBool(_amplificationParamName, false);
        _aiAmplificationAnim.SetBool(_amplificationParamName, false);
    }
}
