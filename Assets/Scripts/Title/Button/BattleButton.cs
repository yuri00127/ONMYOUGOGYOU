using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleButton : Button
{
    private float _loadWaitTime = 1.0f;

    public override void Start()
    {
        base.Start();
    }

    public override void PointerEnter(GameObject gameObject)
    {
        base.PointerEnter(gameObject);
    }

    // キャラクター選択画面へ遷移
    public override void Submit()
    {
        StartCoroutine(LoadScene());
    }

    // シーン遷移のアニメーション
    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(_loadWaitTime);

        SceneManager.LoadScene(CharacterSelectScene);

    }
}
