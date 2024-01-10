using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCloseButton : Button
{
    public override void Submit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
