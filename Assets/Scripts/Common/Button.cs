using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 
public class Button : MonoBehaviour
{
    // �V�[��
    public const string TitleScene = "Title";
    public const string CharacterSelectScene = "CharacterSelect";
    public const string BattleScene = "Battle";

    [Header("�A�j���[�V����")]
    protected Animator Anim;

    // input
    public bool CanInput = true;
    private const float _inputWait = 0.1f;

    // Audio
    private const string _seObjName = "SEManager";
    protected AudioSource Audio;
    [SerializeField] protected AudioClip SubmitSE;  // �{�^����������SE


    public virtual void Start()
    {
        // Animator�R���|�[�l���g�擾
        Anim = this.GetComponent<Animator>();

        // AudioSource�R���|�[�l���g�擾
        Audio = GameObject.Find(_seObjName).GetComponent<AudioSource>();
    }

    // �t�H�[�J�X���̏���
    public virtual void Select()
    {
        
    }

    // �t�H�[�J�X���O�ꂽ���̏���
    public virtual void Deselect()
    {
        
    }

    // �I���������̏���
    public virtual void Submit()
    {

    }

    // �}�E�X�I�[�o�[�������̏���
    public virtual void PointerEnter(GameObject gameObject)
    {
        if (EventSystem.current.currentSelectedGameObject != gameObject)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }

    // �}�E�X�A�E�g�������̏���
    public virtual void PointerExit()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

}
