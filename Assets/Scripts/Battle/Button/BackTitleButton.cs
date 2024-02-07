using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTitleButton : Button
{
    [SerializeField] private LoadNextScene _loadNextScene;
    private const string _nextScene = "Title";

    private bool _isLoading = false;

    // キャラクター選択画面へ遷移
    public override void Submit()
    {
        if (!_isLoading)
        {
            StartCoroutine(_loadNextScene.LoadScene(_nextScene));
        }

        _isLoading = true;
    }

    public override void PointerEnter(GameObject gameObject)
    {
        base.PointerEnter(gameObject);
    }
}
