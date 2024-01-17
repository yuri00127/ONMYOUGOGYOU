using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandManager : MonoBehaviour
{
    // �X�N���v�g���擾����I�u�W�F�N�g
    private const string _battleManagerObjName = "BattleManager";

    // �X�N���v�g
    public BattleManager _battleManager { get; protected set; }
    public Character SelectCharacter { get; protected set; }

    // �L�����N�^�[�̃R�}���h
    protected GameObject[] SelectCommandObjArray = new GameObject[3];   // �I�������R�}���h�̕\��Object
    protected Image[] SelectCommandImageArray = new Image[3];           // �\��Object��Image�R���|�[�l���g
    protected const string MindObjName = "Mind";                        // �I�������A�z��\������Object�̖��O
    protected Image[] MindImageArray = new Image[3];                    // �A�z�\��Object��Image�R���|�[�l���g

    // �\��
    protected GameObject SelectCommandObj;                              // �\���̈��Object
    [SerializeField] private Sprite[] _yinYangSprites = new Sprite[2];  // �A�z��Sprite

    // �I���R�}���h
    public List<int> CommandIdList { get; private set; } = new List<int>();  // �I�����ꂽ������ID���X�g
    public List<bool> IsYinList { get; private set; } = new List<bool>();    // �I�����ꂽ�C���A���ǂ����̃��X�g
    

    protected virtual void Awake()
    {
        // �X�N���v�g�擾
        _battleManager = GameObject.Find(_battleManagerObjName).GetComponent<BattleManager>();

        // �I�������R�}���h�̕\���̈���擾
        int commandIndex = 0;
        for (int i = 0; i < SelectCommandObjArray.Length; i++)
        {
            // Object�擾
            commandIndex++;
            SelectCommandObjArray[i] = SelectCommandObj.transform.Find(commandIndex.ToString()).gameObject;

            // Image�R���|�[�l���g�擾
            SelectCommandImageArray[i] = SelectCommandObjArray[i].GetComponent<Image>();

            // �A�z�\��Object��Image�R���|�[�l���g�擾
            MindImageArray[i] = SelectCommandObjArray[i].transform.Find(MindObjName).GetComponent<Image>();
        }
    }

    protected virtual void Update()
    {

    }

    /// <summary>
    /// �I�����ꂽ�R�}���h�̉摜��\���̈�ɃZ�b�g����
    /// </summary>
    /// <param name="selectingCommandSequence">�\������ʒu</param>
    public virtual void SelectCommand(int selectingCommandSequence)
    {
        // �I�������R�}���h�̉摜���Z�b�g
        int spriteIndex = CommandIdList[selectingCommandSequence];
        SelectCommandImageArray[selectingCommandSequence].sprite = SelectCharacter.SelectCommandSprites[spriteIndex];
    }

    /// <summary>
    /// �I�����ꂽ�C�̉摜��\���̈�ɃZ�b�g����
    /// </summary>
    /// <param name="selectingCommandSequence">�\������ʒu</param>
    protected virtual void SelectMind(int selectingCommandSequence)
    {
        // �z���I������Ă�����
        if (IsYinList[selectingCommandSequence])
        {
            // �A�̉摜���Z�b�g
            MindImageArray[selectingCommandSequence].sprite = _yinYangSprites[0];
            return;
        }

        // �z�̉摜���Z�b�g
        MindImageArray[selectingCommandSequence].sprite = _yinYangSprites[1];
    }
    
}
