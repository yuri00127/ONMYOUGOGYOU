using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBreakButton : Button
{
    [SerializeField] private LoadNextScene _loadNextScene;
    private const string _nextScene = "CharacterSelect";

    private bool _isLoading = false;

    // �L�����N�^�[�I����ʂ֑J��
    public override void Submit()
    {
        if (!_isLoading)
        {
            Audio.PlayOneShot(SubmitSE);
            StartCoroutine(_loadNextScene.LoadScene(_nextScene));
        }

        _isLoading = true;
    }

    public override void PointerEnter(GameObject gameObject)
    {
        base.PointerEnter(gameObject);
    }
}
