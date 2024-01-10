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

    // 選択ステップ（自キャラ、敵キャラ、敵AIレベル）
    //public int NowSelectStep = 0;
    public int NowSelectStep { get; private set; } = 0;

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
    private const string _seManagerObjName = "SEManager";
    private AudioSource _audio;
    [SerializeField] private AudioClip _submitSE;
    [SerializeField] private AudioClip _submitFinishSE;
    [SerializeField] private AudioClip _cancelSE;


    private void Start()
    {
        _stepGuideImg = _stepGuideObj.GetComponent<Image>();
        _cancelGuideImg = _cancelGuideObj.GetComponent<Image>();

        _audio = GameObject.Find(_seManagerObjName).GetComponent<AudioSource>();
    }

    // プレイヤーキャラクターの選択ステップ
    private void PlayerCharacterSelect()
    {
        StepGuideSet();
        EventSystem.current.SetSelectedGameObject(_characterDefaultButton);
    }

    // 敵キャラクターの選択ステップ
    private void AICharacterSelect()
    {
        StepGuideSet();
        EventSystem.current.SetSelectedGameObject(_characterDefaultButton);
    }

    // 敵AIレベルの選択ステップ
    private void AILevelSelect()
    {
        _AiLevelObj.SetActive(true);
        StepGuideSet();
        EventSystem.current.SetSelectedGameObject(_AILevelDefaultButton);
    }


    // プレイヤーの使用キャラクターを選択して、敵キャラ選択へ
    public void NextAICharacterSelect(int PlayerCharacterId)
    {
        _audio.PlayOneShot(_submitSE);
        PlayerPrefs.SetInt(_selectCharacterData.SavePlayerCharacterId, PlayerCharacterId);

        NowSelectStep++;
        AICharacterSelect();
    }

    // AIの使用キャラクターを選択して、AIレベル選択へ
    public void NextAILevelSelect(int AICharacterId)
    {
        _audio.PlayOneShot(_submitSE);
        PlayerPrefs.SetInt(_selectCharacterData.SaveAICharacterId, AICharacterId);

        NowSelectStep++;
        AILevelSelect();
    }

    // AIの強さのレベルを選択して、バトル画面へ遷移
    public void NextBattleScene(int level)
    {
        _audio.PlayOneShot(_submitFinishSE);
        PlayerPrefs.SetInt(_selectCharacterData.SaveAILevel, level);

        StartCoroutine(_loadNextScene.LoadScene(_battleScene));
    }

    // 選択ステップを一つ前に戻す
    public void BackStep()
    {
        _audio.PlayOneShot(_cancelSE);

        NowSelectStep--;
        Debug.Log("back:" + NowSelectStep);

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

    // ステップごとにガイド画像を変更する
    private void StepGuideSet()
    {
        _stepGuideImg.sprite = _stepGuides[NowSelectStep];
        _cancelGuideImg.sprite = _cancelGuides[NowSelectStep];
    }

}
