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

    // �L�����N�^�[�I����ʂ֑J��
    public override void Submit()
    {
        StartCoroutine(LoadScene());
    }

    // �V�[���J�ڂ̃A�j���[�V����
    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(_loadWaitTime);

        SceneManager.LoadScene(CharacterSelectScene);

    }
}
