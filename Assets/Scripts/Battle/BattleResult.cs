using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleResult : MonoBehaviour
{
    [Header("勝敗結果")]
    [SerializeField] private GameObject _resultCanvas;
    [SerializeField] private Image _resultImage;
    [SerializeField] private GameObject _showCharacterImageObj;         // 表示するキャラクターのImageを持つオブジェクト
    private Image _showCharacterImage;
    [SerializeField] private GameObject _playerCharacterObj;            // プレイヤーキャラクターのオブジェクト
    [SerializeField] private GameObject _resultDefaultButtonObj;
    [SerializeField] private Sprite[] _resultSprites = new Sprite[2];   // 勝敗画像
    private const string _buttonParentObjName = "Button";               // ボタンを子として持つ親オブジェクトの名前

    [Header("Audio")]
    [SerializeField] private AudioSource _bgmAudio;
    [SerializeField] private AudioClip _battleFinishJingle;    // バトル終了時のジングル


    public IEnumerator CoBattleFinish(bool playerWin)
    {
        _bgmAudio.clip = _battleFinishJingle;
        _bgmAudio.Play();
        _bgmAudio.loop = false;

        // 勝敗結果の表示
        _resultCanvas.SetActive(true);
        _showCharacterImage = _showCharacterImageObj.GetComponent<Image>();

        // プレイヤー勝利
        if (playerWin)
        {
            _resultImage.sprite = _resultSprites[0];
        }

        // プレイヤー敗北
        if (!playerWin)
        {
            _resultImage.sprite = _resultSprites[1];
        }

        // アニメーション
        _showCharacterImage.sprite = _playerCharacterObj.GetComponent<Image>().sprite;
        _showCharacterImageObj.GetComponent<Animator>().SetTrigger("First");

        yield return new WaitForSeconds(1f);

        _resultCanvas.transform.Find(_buttonParentObjName).gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_resultDefaultButtonObj);
    }
}
