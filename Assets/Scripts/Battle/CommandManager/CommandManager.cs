using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandManager : MonoBehaviour
{
    public Character SelectCharacter { get; protected set; }

    // �L�����N�^�[�̃R�}���h
    [SerializeField] protected GameObject[] SelectCommandAttributeObjArray = new GameObject[3];  // �I������������\������Object
    protected Image[] SelectCommandAttributeImageArray = new Image[3];                           // �����\��Object��Image
    [SerializeField] protected GameObject[] SelectCommandMindObjArray = new GameObject[3];       // �I�������A�z��\������Object
    protected Image[] SelectCommandMindImageArray = new Image[3];                                // �A�z�\��Object��Image

    // �\��
    [SerializeField] protected GameObject SelectCommandObj;                              // �\���̈��Object
    [SerializeField] private Sprite[] _yinYangSprites = new Sprite[2];  // �A�z��Sprite

    // �I���R�}���h
    public List<int> CommandIdList { get; private set; } = new List<int>();  // �I�����ꂽ������ID���X�g
    public List<bool> IsYinList { get; private set; } = new List<bool>();    // �I�����ꂽ�C���A���ǂ����̃��X�g
    

    protected virtual void Awake()
    {
        // �I�������R�}���h�̕\���̈���擾
        int commandIndex = 0;
        for (int i = 0; i < SelectCommandAttributeObjArray.Length; i++)
        {
            // Image�R���|�[�l���g�擾
            SelectCommandAttributeImageArray[i] = SelectCommandAttributeObjArray[i].GetComponent<Image>();

            // �A�z�\��Object��Image�R���|�[�l���g�擾
            SelectCommandMindImageArray[i] = SelectCommandMindObjArray[i].GetComponent<Image>();

            commandIndex++;
        }
    }

    protected virtual void Update() { }

    /// <summary>
    /// �I�����ꂽ�����̉摜��\���̈�ɃZ�b�g����
    /// </summary>
    /// <param name="selectingCommandSequence">�\������ʒu</param>
    public virtual void SelectCommand(int selectingCommandSequence)
    {
        // �I�����������̉摜���Z�b�g
        int spriteIndex = CommandIdList[selectingCommandSequence];
        SelectCommandAttributeImageArray[selectingCommandSequence].sprite = SelectCharacter.SelectCommandSprites[spriteIndex];
    }

    /// <summary>
    /// �I�����ꂽ�C�̉摜��\���̈�ɃZ�b�g����
    /// </summary>
    /// <param name="selectingCommandSequence">�\������ʒu</param>
    public virtual void SelectMind(int selectingCommandSequence)
    {
        // �A�̉摜���Z�b�g
        if (IsYinList[selectingCommandSequence])
        {
            SelectCommandMindImageArray[selectingCommandSequence].sprite = _yinYangSprites[0];
            return;
        }

        // �z�̉摜���Z�b�g
        SelectCommandMindImageArray[selectingCommandSequence].sprite = _yinYangSprites[1];
    }
    
}
