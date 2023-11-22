using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadNextScene : MonoBehaviour
{
    [SerializeField] private GameObject _fadeObj;
    private Image _fadeImg;
    private bool _isFade = false;

    private const float _loadWaitTime = 1.0f;
    private const float _fadeWaitTime = 1.5f;

    private void Update()
    {
        if (_isFade)
        {
            _fadeImg.color += new Color(0, 0, 0, Time.deltaTime);
        }
    }

    // �w�肳�ꂽ���O�̃V�[���ɑJ�ڂ���
    public IEnumerator LoadScene(string sceneName)
    {
        _fadeObj.SetActive(true);
        _fadeImg = _fadeObj.GetComponent<Image>();

        yield return new WaitForSeconds(_loadWaitTime);

        // ���ʉ�

        _isFade = true;
        yield return new WaitForSeconds(_fadeWaitTime);

        SceneManager.LoadScene(sceneName);

    }
}
