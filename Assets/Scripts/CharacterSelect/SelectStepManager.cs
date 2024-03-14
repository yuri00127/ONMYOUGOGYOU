using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectStepManager : MonoBehaviour
{
    [Header("スクリプト")]
    [SerializeField] private LoadNextScene _loadNextScene;
    [SerializeField] private SelectCharacterData _selectCharacterData;

    public int NowSelectStep { get; private set; } = 0;     // 選択ステップ（自キャラ、敵キャラ、敵AIレベル）

    [Header("デフォルトボタン")]
    [SerializeField] private GameObject _characterDefaultButton;
    [SerializeField] private GameObject _AILevelDefaultButton;

    [SerializeField] private GameObject _AiLevelObj;

    [Header("ステップガイド")]
    [SerializeField] private GameObject _stepGuideObj;
    private Image _stepGuideImg;
    [SerializeField] private Sprite[] _stepGuides = new Sprite[3];

    [Header("Cancelガイド")]
    [SerializeField] private GameObject _cancelGuideObj;
    private Image _cancelGuideImg;
    [SerializeField] private Sprite[] _cancelGuides = new Sprite[3];
    
    private const string _titleScene = "Title";
    private const string _battleScene = "Battle";

    // Audio
    [SerializeField] private AudioSource _seAudio;
    [SerializeField] private AudioClip _submitSE;
    [SerializeField] private AudioClip _submitFinishSE;
    [SerializeField] private AudioClip _cancelSE;


    private void Start()
    {
        _stepGuideImg = _stepGuideObj.GetComponent<Image>();
        _cancelGuideImg = _cancelGuideObj.GetComponent<Image>();
    }

    /// <summary>
    /// プレイヤーキャラクターの選択ステップ開始
    /// </summary>
    private void PlayerCharacterSelect()
    {
        StepGuideSet();
        EventSystem.current.SetSelectedGameObject(_characterDefaultButton);
    }

    /// <summary>
    /// 敵キャラクターの選択ステップ開始
    /// </summary>
    private void AICharacterSelect()
    {
        StepGuideSet();
        EventSystem.current.SetSelectedGameObject(_characterDefaultButton);

    }

    /// <summary>
    /// 敵AIレベルの選択ステップ開始
    /// </summary>
    private void AILevelSelect()
    {
        _AiLevelObj.SetActive(true);
        StepGuideSet();
        EventSystem.current.SetSelectedGameObject(_AILevelDefaultButton);
    }


    /// <summary>
    /// プレイヤーの使用キャラクターを確定する
    /// </summary>
    /// <param name="PlayerCharacterId"></param>
    public void NextAICharacterSelect(int PlayerCharacterId)
    {
        _seAudio.PlayOneShot(_submitSE);
        PlayerPrefs.SetInt(_selectCharacterData.SavePlayerCharacterId, PlayerCharacterId);

        NowSelectStep++;
        AICharacterSelect();
    }

    /// <summary>
    /// AIの使用キャラクターを確定する
    /// </summary>
    /// <param name="AICharacterId"></param>
    public void NextAILevelSelect(int AICharacterId)
    {
        _seAudio.PlayOneShot(_submitSE);
        PlayerPrefs.SetInt(_selectCharacterData.SaveAICharacterId, AICharacterId);

        NowSelectStep++;
        AILevelSelect();
    }

    /// <summary>
    /// AIの強さのレベルを確定する
    /// </summary>
    /// <param name="level"></param>
    public void NextBattleScene(int level)
    {
        _seAudio.PlayOneShot(_submitFinishSE);
        PlayerPrefs.SetInt(_selectCharacterData.SaveAILevel, level);

        StartCoroutine(_loadNextScene.LoadScene(_battleScene));
    }

    /// <summary>
    /// 選択ステップを一つ前に戻す
    /// </summary>
    public void BackStep()
    {
        _seAudio.PlayOneShot(_cancelSE);
        NowSelectStep--;

        // タイトル画面へ戻る
        if (NowSelectStep == -1)
        {
            SceneManager.LoadScene(_titleScene);
        }

        // プレイヤー選択へ戻る
        if (NowSelectStep == 0)
        {
            PlayerCharacterSelect();
            return;
        }

        // 敵選択へ戻る
        if (NowSelectStep == 1)
        {
            _AiLevelObj.SetActive(false);
            AICharacterSelect();
            return;
        }
    }

    /// <summary>
    /// ステップごとにガイド画像を変更する
    /// </summary>
    private void StepGuideSet()
    {
        _stepGuideImg.sprite = _stepGuides[NowSelectStep];
        _cancelGuideImg.sprite = _cancelGuides[NowSelectStep];
    }

}
