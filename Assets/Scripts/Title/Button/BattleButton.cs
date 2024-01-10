using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleButton : Button
{
    [SerializeField] private LoadNextScene _loadNextScene;
    private const string _nextScene = "CharacterSelect";

    private bool _isLoading = false;

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
        // 初回押下時のみ処理する
        if (!_isLoading)
        {
            Audio.PlayOneShot(SubmitSE);
            StartCoroutine(_loadNextScene.LoadScene(_nextScene));

            _isLoading = true;
        }
    }

}
