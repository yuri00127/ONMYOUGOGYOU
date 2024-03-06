using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleResult : MonoBehaviour
{
    [Header("勝敗結果")]
    [SerializeField] private GameObject _resultCanvas;
    private const string _resultImageObjName = "ResultImage";
    private Image _resultImage;
    private const string _characterImageObjName = "CharacterImage";
    private GameObject _characterImageObj;
    private Image _characterImage;
    private const string _resultDefaultButtonObjName = "BackCharacterSelectButton";
    private GameObject _resultDefaultButtonObj;
    [SerializeField] private Sprite[] _resultSprites = new Sprite[2];   // 勝敗画像

    [Header("Audio")]
    private const string _bgmManagerObjName = "BGMManager";
    private AudioSource _bgmAudio;
    [SerializeField] private AudioClip _battleFinishJingle;    // バトル終了時のジングル


    public IEnumerator BattleFinish(bool playerWin)
    {
        Debug.Log("バトル終了");

        _bgmAudio.clip = _battleFinishJingle;
        _bgmAudio.Play();
        _bgmAudio.loop = false;

        // 勝敗結果の表示
        _resultCanvas.SetActive(true);
        _resultImage = GameObject.Find(_resultImageObjName).GetComponent<Image>();
        _characterImageObj = GameObject.Find(_characterImageObjName);
        _characterImage = _characterImageObj.GetComponent<Image>();

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

        _characterImage.sprite = GameObject.Find("PlayerCharacter").GetComponent<Image>().sprite;
        _characterImageObj.GetComponent<Animator>().SetTrigger("First");

        yield return new WaitForSeconds(1f);

        _resultCanvas.transform.Find("Button").gameObject.SetActive(true);
        _resultDefaultButtonObj = GameObject.Find(_resultDefaultButtonObjName);
        EventSystem.current.SetSelectedGameObject(_resultDefaultButtonObj);

    }
}
