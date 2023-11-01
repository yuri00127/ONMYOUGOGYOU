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
    private Animator _anim;


    public virtual void Start()
    {
        // Animator�R���|�[�l���g�擾
        _anim = this.GetComponent<Animator>();
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
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    // �}�E�X�A�E�g�������̏���
    public virtual void PointerExit()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

}
