using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleButton : Button
{
    [SerializeField] private LoadNextScene _loadNextScene;
    private const string _nextScene = "CharacterSelect";

    public override void Start()
    {
        base.Start();
    }

    public override void PointerEnter(GameObject gameObject)
    {
        base.PointerEnter(gameObject);
    }

    // �L�����N�^�[�I����ʂ֑J��
    public override void Submit()
    {
        StartCoroutine(_loadNextScene.LoadScene(_nextScene));
    }

}
