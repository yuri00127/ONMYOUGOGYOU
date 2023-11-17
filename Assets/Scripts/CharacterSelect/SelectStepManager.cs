using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectStepManager : MonoBehaviour
{
    // �I���X�e�b�v�i���L�����A�G�L�����A�GAI���x���j
    public int NowSelectStep = 0;
    //public int NowSelectStep { get; private set; } = 0;

    [Header("�f�t�H���g�{�^��")]
    [SerializeField] private GameObject _characterDefaultButton;
    [SerializeField] private GameObject _AILevelDefaultButton;

    [SerializeField] private GameObject _AiLevelObj;

    private const string _savePlayerCharacterId = "PlayerCharacter";
    private const string _saveAICharacterId = "AICharacter";
    private const string _saveAILevel = "AILevel";

    [Header("�X�e�b�v�K�C�h")]
    [SerializeField] private GameObject _stepGuideObj;
    private Image _stepGuideImg;
    [SerializeField] private Sprite[] _stepGuides = new Sprite[3];
    

    private const string _titleScene = "Title";
    private const string _battleScene = "Battle";


    private void Start()
    {
        _stepGuideImg = _stepGuideObj.GetComponent<Image>();
    }

    // �v���C���[�L�����N�^�[�̑I���X�e�b�v
    private void PlayerCharacterSelect()
    {
        _stepGuideImg.sprite = _stepGuides[0];
        EventSystem.current.SetSelectedGameObject(_characterDefaultButton);
    }

    // �G�L�����N�^�[�̑I���X�e�b�v
    private void AICharacterSelect()
    {
        _stepGuideImg.sprite = _stepGuides[1];
        EventSystem.current.SetSelectedGameObject(_characterDefaultButton);
    }

    // �GAI���x���̑I���X�e�b�v
    private void AILevelSelect()
    {
        _AiLevelObj.SetActive(true);
        _stepGuideImg.sprite = _stepGuides[2];
        EventSystem.current.SetSelectedGameObject(_AILevelDefaultButton);
    }

    // �v���C���[�̎g�p�L�����N�^�[��I�����āA�G�L�����I����
    public void NextAICharacterSelect(int PlayerCharacterId)
    {
        PlayerPrefs.SetInt(_savePlayerCharacterId, PlayerCharacterId);

        NowSelectStep++;
        AICharacterSelect();
    }

    // AI�̎g�p�L�����N�^�[��I�����āAAI���x���I����
    public void NextAILevelSelect(int AICharacterId)
    {
        PlayerPrefs.SetInt(_saveAICharacterId, AICharacterId);

        NowSelectStep++;
        AILevelSelect();
    }

    // AI�̋����̃��x����I�����āA�o�g����ʂ֑J��
    public void NextBattleScene(int level)
    {
        PlayerPrefs.SetInt(_saveAILevel, level);

        Debug.Log("PLChara:" + PlayerPrefs.GetInt(_savePlayerCharacterId));
        Debug.Log("AIChara:" + PlayerPrefs.GetInt(_saveAICharacterId));
        Debug.Log("AILevel:" + PlayerPrefs.GetInt(_saveAILevel));

        //SceneManager.LoadScene(_battleScene);
    }

    // �I���X�e�b�v����O�ɖ߂�
    public void BackStep()
    {
        NowSelectStep--;
        Debug.Log("back:" + NowSelectStep);

        // �^�C�g����ʂ֖߂�
        if (NowSelectStep == -1)
        {
            SceneManager.LoadScene(_titleScene);
        }

        // �v���C���[�I���֖߂�
        if (NowSelectStep == 0)
        {
            PlayerCharacterSelect();
            return;
        }

        // �G�I���֖߂�
        if (NowSelectStep == 1)
        {
            _AiLevelObj.SetActive(false);
            AICharacterSelect();
            return;
        }
    }


}