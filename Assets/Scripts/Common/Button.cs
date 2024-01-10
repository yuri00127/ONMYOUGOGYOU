using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 
public class Button : MonoBehaviour
{
    // シーン
    public const string TitleScene = "Title";
    public const string CharacterSelectScene = "CharacterSelect";
    public const string BattleScene = "Battle";

    [Header("アニメーション")]
    protected Animator Anim;

    // input
    public bool CanInput = true;
    private const float _inputWait = 0.1f;

    // Audio
    private const string _seObjName = "SEManager";
    protected AudioSource Audio;
    [SerializeField] protected AudioClip SubmitSE;  // ボタン押下時のSE


    public virtual void Start()
    {
        // Animatorコンポーネント取得
        Anim = this.GetComponent<Animator>();

        // AudioSourceコンポーネント取得
        Audio = GameObject.Find(_seObjName).GetComponent<AudioSource>();
    }

    // フォーカス時の処理
    public virtual void Select()
    {
        
    }

    // フォーカスが外れた時の処理
    public virtual void Deselect()
    {
        
    }

    // 選択した時の処理
    public virtual void Submit()
    {

    }

    // マウスオーバーした時の処理
    public virtual void PointerEnter(GameObject gameObject)
    {
        if (EventSystem.current.currentSelectedGameObject != gameObject)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }

    // マウスアウトした時の処理
    public virtual void PointerExit()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

}
