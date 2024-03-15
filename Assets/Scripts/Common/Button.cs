using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 
public class Button : MonoBehaviour
{
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

    /// <summary>
    /// �t�H�[�J�X���̏���
    /// </summary>
    public virtual void Select()
    {
        
    }

    /// <summary>
    /// �t�H�[�J�X���O�ꂽ���̏���
    /// </summary>
    public virtual void Deselect()
    {
        
    }

    /// <summary>
    /// �I���������̏���
    /// </summary>
    public virtual void Submit()
    {

    }

    /// <summary>
    /// �}�E�X�I�[�o�[�������̏���
    /// </summary>
    /// <param name="gameObject">�}�E�X�I�[�o�[�����I�u�W�F�N�g</param>
    public virtual void PointerEnter(GameObject gameObject)
    {
        if (EventSystem.current.currentSelectedGameObject != gameObject)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }

    /// <summary>
    /// �}�E�X�A�E�g�������̏���
    /// </summary>
    public virtual void PointerExit()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

}
