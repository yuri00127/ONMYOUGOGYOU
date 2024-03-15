using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleResult : MonoBehaviour
{
    [Header("���s����")]
    [SerializeField] private GameObject _resultCanvas;
    [SerializeField] private Image _resultImage;
    [SerializeField] private GameObject _showCharacterImageObj;         // �\������L�����N�^�[��Image�����I�u�W�F�N�g
    private Image _showCharacterImage;
    [SerializeField] private GameObject _playerCharacterObj;            // �v���C���[�L�����N�^�[�̃I�u�W�F�N�g
    [SerializeField] private GameObject _resultDefaultButtonObj;
    [SerializeField] private Sprite[] _resultSprites = new Sprite[2];   // ���s�摜
    private const string _buttonParentObjName = "Button";               // �{�^�����q�Ƃ��Ď��e�I�u�W�F�N�g�̖��O

    [Header("Audio")]
    [SerializeField] private AudioSource _bgmAudio;
    [SerializeField] private AudioClip _battleFinishJingle;    // �o�g���I�����̃W���O��


    public IEnumerator CoBattleFinish(bool playerWin)
    {
        _bgmAudio.clip = _battleFinishJingle;
        _bgmAudio.Play();
        _bgmAudio.loop = false;

        // ���s���ʂ̕\��
        _resultCanvas.SetActive(true);
        _showCharacterImage = _showCharacterImageObj.GetComponent<Image>();

        // �v���C���[����
        if (playerWin)
        {
            _resultImage.sprite = _resultSprites[0];
        }

        // �v���C���[�s�k
        if (!playerWin)
        {
            _resultImage.sprite = _resultSprites[1];
        }

        // �A�j���[�V����
        _showCharacterImage.sprite = _playerCharacterObj.GetComponent<Image>().sprite;
        _showCharacterImageObj.GetComponent<Animator>().SetTrigger("First");

        yield return new WaitForSeconds(1f);

        _resultCanvas.transform.Find(_buttonParentObjName).gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_resultDefaultButtonObj);
    }
}
