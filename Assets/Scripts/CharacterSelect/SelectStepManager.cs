using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectStepManager : MonoBehaviour
{
    [Header("�X�N���v�g")]
    [SerializeField] private LoadNextScene _loadNextScene;
    [SerializeField] private SelectCharacterData _selectCharacterData;

    public int NowSelectStep { get; private set; } = 0;     // �I���X�e�b�v�i���L�����A�G�L�����A�GAI���x���j

    [Header("�f�t�H���g�{�^��")]
    [SerializeField] private GameObject _characterDefaultButton;
    [SerializeField] private GameObject _AILevelDefaultButton;

    [SerializeField] private GameObject _AiLevelObj;

    [Header("�X�e�b�v�K�C�h")]
    [SerializeField] private GameObject _stepGuideObj;
    private Image _stepGuideImg;
    [SerializeField] private Sprite[] _stepGuides = new Sprite[3];

    [Header("Cancel�K�C�h")]
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
    /// �v���C���[�L�����N�^�[�̑I���X�e�b�v�J�n
    /// </summary>
    private void PlayerCharacterSelect()
    {
        StepGuideSet();
        EventSystem.current.SetSelectedGameObject(_characterDefaultButton);
    }

    /// <summary>
    /// �G�L�����N�^�[�̑I���X�e�b�v�J�n
    /// </summary>
    private void AICharacterSelect()
    {
        StepGuideSet();
        EventSystem.current.SetSelectedGameObject(_characterDefaultButton);

    }

    /// <summary>
    /// �GAI���x���̑I���X�e�b�v�J�n
    /// </summary>
    private void AILevelSelect()
    {
        _AiLevelObj.SetActive(true);
        StepGuideSet();
        EventSystem.current.SetSelectedGameObject(_AILevelDefaultButton);
    }


    /// <summary>
    /// �v���C���[�̎g�p�L�����N�^�[���m�肷��
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
    /// AI�̎g�p�L�����N�^�[���m�肷��
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
    /// AI�̋����̃��x�����m�肷��
    /// </summary>
    /// <param name="level"></param>
    public void NextBattleScene(int level)
    {
        _seAudio.PlayOneShot(_submitFinishSE);
        PlayerPrefs.SetInt(_selectCharacterData.SaveAILevel, level);

        StartCoroutine(_loadNextScene.LoadScene(_battleScene));
    }

    /// <summary>
    /// �I���X�e�b�v����O�ɖ߂�
    /// </summary>
    public void BackStep()
    {
        _seAudio.PlayOneShot(_cancelSE);
        NowSelectStep--;

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

    /// <summary>
    /// �X�e�b�v���ƂɃK�C�h�摜��ύX����
    /// </summary>
    private void StepGuideSet()
    {
        _stepGuideImg.sprite = _stepGuides[NowSelectStep];
        _cancelGuideImg.sprite = _cancelGuides[NowSelectStep];
    }

}
