using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleButton : Button
{
    [SerializeField] private LoadNextScene _loadNextScene;
    private const string _characterSelectSceneName = "CharacterSelect";
    private bool _isLoading = false;


    public override void Start()
    {
        base.Start();
    }

    public override void PointerEnter(GameObject gameObject)
    {
        base.PointerEnter(gameObject);
    }

    /// <summary>
    /// �L�����N�^�[�I����ʂ֑J��
    /// </summary>
    public override void Submit()
    {
        // ���񉟉����̂ݏ�������
        if (!_isLoading)
        {
            Audio.PlayOneShot(SubmitSE);
            StartCoroutine(_loadNextScene.LoadScene(_characterSelectSceneName));

            _isLoading = true;
        }
    }

}
