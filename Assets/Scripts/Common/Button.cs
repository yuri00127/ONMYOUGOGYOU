using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 
public class Button : MonoBehaviour
{
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

    /// <summary>
    /// フォーカス時の処理
    /// </summary>
    public virtual void Select()
    {
        
    }

    /// <summary>
    /// フォーカスが外れた時の処理
    /// </summary>
    public virtual void Deselect()
    {
        
    }

    /// <summary>
    /// 選択した時の処理
    /// </summary>
    public virtual void Submit()
    {

    }

    /// <summary>
    /// マウスオーバーした時の処理
    /// </summary>
    /// <param name="gameObject">マウスオーバーしたオブジェクト</param>
    public virtual void PointerEnter(GameObject gameObject)
    {
        if (EventSystem.current.currentSelectedGameObject != gameObject)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }

    /// <summary>
    /// マウスアウトした時の処理
    /// </summary>
    public virtual void PointerExit()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

}
