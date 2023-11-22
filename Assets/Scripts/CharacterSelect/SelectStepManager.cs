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

    // 選択ステップ（自キャラ、敵キャラ、敵AIレベル）
    //public int NowSelectStep = 0;
    public int NowSelectStep { get; private set; } = 0;

    [Header("デフォルトボタン")]
    [SerializeField] private GameObject _characterDefaultButton;
    [SerializeField] private GameObject _AILevelDefaultButton;

    [SerializeField] private GameObject _AiLevelObj;

    private const string _savePlayerCharacterId = "PlayerCharacter";
    private const string _saveAICharacterId = "AICharacter";
    private const string _saveAILevel = "AILevel";

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


    private void Start()
    {
        _stepGuideImg = _stepGuideObj.GetComponent<Image>();
        _cancelGuideImg = _cancelGuideObj.GetComponent<Image>();
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
        PlayerPrefs.SetInt(_savePlayerCharacterId, PlayerCharacterId);

        NowSelectStep++;
        AICharacterSelect();
    }

    // AIの使用キャラクターを選択して、AIレベル選択へ
    public void NextAILevelSelect(int AICharacterId)
    {
        PlayerPrefs.SetInt(_saveAICharacterId, AICharacterId);

        NowSelectStep++;
        AILevelSelect();
    }

    // AIの強さのレベルを選択して、バトル画面へ遷移
    public void NextBattleScene(int level)
    {
        PlayerPrefs.SetInt(_saveAILevel, level);

        Debug.Log("PLChara:" + PlayerPrefs.GetInt(_savePlayerCharacterId));
        Debug.Log("AIChara:" + PlayerPrefs.GetInt(_saveAICharacterId));
        Debug.Log("AILevel:" + PlayerPrefs.GetInt(_saveAILevel));

        StartCoroutine(_loadNextScene.LoadScene(_battleScene));
    }

    // 選択ステップを一つ前に戻す
    public void BackStep()
    {
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
