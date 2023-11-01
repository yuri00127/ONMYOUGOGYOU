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
    private Animator _anim;


    public virtual void Start()
    {
        // Animatorコンポーネント取得
        _anim = this.GetComponent<Animator>();
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
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    // マウスアウトした時の処理
    public virtual void PointerExit()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

}
