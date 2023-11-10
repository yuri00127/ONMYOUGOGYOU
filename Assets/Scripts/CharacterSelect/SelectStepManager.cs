using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectStepManager : MonoBehaviour
{
    // �I��i�K�i���L�����N�^�[�A�G�L�����N�^�[�A�GAI���x���j
    public int NowSelectStep { get; private set; } = 0;

    [Header("�f�t�H���g�{�^��")]
    [SerializeField] private GameObject _AICharacterDefaultButton;
    [SerializeField] private GameObject _AILevelDefaultButton;

    // �v���C���[�̎g�p�L�����N�^�[��I�����āA�G�L�����I����
    public void PlayerCharacterSelect(GameObject playerCharacter)
    {
        EventSystem.current.SetSelectedGameObject(_AICharacterDefaultButton);
        NowSelectStep++;
    }

    // AI�̎g�p�L�����N�^�[��I�����āAAI���x���I����
    public void AICharacterSelect(GameObject AICharacter)
    {
        EventSystem.current.SetSelectedGameObject(_AILevelDefaultButton);
        NowSelectStep++;
    }

    // AI�̋����̃��x����I�����āA�o�g����ʂ֑J��
    public void AILevelSelect(GameObject level)
    {

    }
}
