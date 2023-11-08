using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleButton : Button
{

    public override void Start()
    {
        base.Start();
    }


    public override void Select()
    {
        // シーンを次のインデックスにする
    }

    public override void Deselect()
    {
        
    }

    public override void Submit()
    {
        
    }

    public override void PointerEnter(GameObject gameObject)
    {
        base.PointerEnter(gameObject);
    }

    public override void PointerExit()
    {
        base.PointerExit();
    }

}
