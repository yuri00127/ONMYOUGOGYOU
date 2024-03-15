using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OperatingGuideChange : MonoBehaviour
{
    private GameObject _view;     // 表示画面のオブジェクト

    [Header("PC操作")]
    [SerializeField] private Sprite[] _changePCButtonSprites = new Sprite[2];
    private const string _changePCButtonObjName = "ChangeKeyBoardAndMouseButton";
    private Image _changePCButtonImg;

    [Header("コントローラー操作")]
    [SerializeField] private Sprite[] _changeControllerButtonSprites = new Sprite[2];
    private const string _changeControllerButtonObjName = "ChangeControllerButton";
    private Image _changeControllerButtonImg;

    [Header("操作説明画像")]
    [SerializeField] private Sprite[] _operatingGuideSprites = new Sprite[4];
    private const string _operatingGuideObjName = "ControllerGuide";
    private Image _operatingGuideImg;
    public bool _isPC { get; private set; } = true;            // 操作をPCに切り替えた直後である
    public bool _isController { get; private set; } = false;   // 操作をコントローラーに切り替えた直後である

    [Header("Audio")]
    [SerializeField] private AudioClip ControllerChangeSE;  // 操作説明画像切り替え時のSE


    /// <summary>
    /// 表示の準備
    /// </summary>
    /// <param name="view">表示する領域のオブジェクト</param>
    public void SetUp(GameObject view)
    {
        // 操作方法切り替えボタンを取得
        _operatingGuideImg = view.transform.Find(_operatingGuideObjName).GetComponent<Image>();
        _changePCButtonImg = view.transform.Find(_changePCButtonObjName).GetComponent<Image>();
        _changeControllerButtonImg = view.transform.Find(_changeControllerButtonObjName).GetComponent<Image>();
    }

    /// <summary>
    /// 操作説明をPC版に切り替える
    /// </summary>
    /// <param name="audio">SEManagerのAudio</param>
    /// <param name="sceneNum">Unityでのシーン番号</param>
    public void PCButton(AudioSource audio, int sceneNum)
    {
        audio.PlayOneShot(ControllerChangeSE);

        // 操作説明画像を変更
        _operatingGuideImg.sprite = _operatingGuideSprites[sceneNum];

        // 切り替えボタンの画像を変更
        _changeControllerButtonImg.sprite = _changeControllerButtonSprites[0];
        _changePCButtonImg.sprite = _changePCButtonSprites[1];

        _isPC = true;
        _isController = false;
    }

    /// <summary>
    /// 操作説明をコントローラー版に切り替える
    /// </summary>
    /// <param name="audio">SEManagerのAudio</param>
    /// <param name="sceneNum">Unityでのシーン番号</param>
    public void ControllerButton(AudioSource audio, int sceneNum)
    {
        audio.PlayOneShot(ControllerChangeSE);

        // 操作説明画像を変更
        _operatingGuideImg.sprite = _operatingGuideSprites[sceneNum + 1];

        // 切り替えボタンの画像を変更
        _changePCButtonImg.sprite = _changePCButtonSprites[0];
        _changeControllerButtonImg.sprite = _changeControllerButtonSprites[1];

        _isController = true;
        _isPC = false;
    }
}
