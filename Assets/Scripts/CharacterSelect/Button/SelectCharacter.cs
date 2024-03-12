using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacter : Button
{
    [SerializeField] private CharacterDataBase _characterDataBase;
    [SerializeField] private SelectStepManager _selectStepManager;

    private string _characterIdFormat = "Character{0}";
    private int _selectCharacterId;

    // �L�����N�^�[�r���[
    private const string _playerCharacterViewObjName = "PlayerCharacterBox";
    private Image _playerCharacterViewImage;
    private const string _aiCharacterViewObjName = "AICharacterBox";
    private Image _aiCharacterViewImage;
    [SerializeField] private Sprite _defaultCharacterSprite;


    private void Awake()
    {
        _playerCharacterViewImage = GameObject.Find(_playerCharacterViewObjName).GetComponent<Image>();
        _aiCharacterViewImage = GameObject.Find(_aiCharacterViewObjName).GetComponent<Image>();
    }

    public override void PointerEnter(GameObject gameObject)
    {
        if (_selectStepManager.NowSelectStep != 2)
        {
            base.PointerEnter(gameObject);
        }

        // �v���C���[�L�����N�^�[�I����
        if (_selectStepManager.NowSelectStep == 0)
        {
            _playerCharacterViewImage.sprite = gameObject.GetComponent<Image>().sprite;
            return;
        }

        // AI�L�����N�^�[�I����
        if (_selectStepManager.NowSelectStep == 1)
        {
            _aiCharacterViewImage.sprite = gameObject.GetComponent<Image>().sprite;
            return;
        }
    }

    /// <summary>
    /// �L�����N�^�[��I��
    /// </summary>
    /// <param name="gameObject">�L�����N�^�[�̃I�u�W�F�N�g</param>
    public void Submit(GameObject gameObject)
    {
        if (_selectStepManager.NowSelectStep != 2)
        {
            // �I�����ꂽ�L�����N�^�[��ID���擾
            for (int i = 1; i <= _characterDataBase.CharacterList.Count; i++)
            {
                if (gameObject.name == string.Format(_characterIdFormat, i))
                {
                    _selectCharacterId = i;
                    break;
                }
            }
        }

        // �v���C���[�L�����N�^�[�̑I��
        if (_selectStepManager.NowSelectStep == 0)
        {
            _selectStepManager.NextAICharacterSelect(_selectCharacterId);
            _playerCharacterViewImage.sprite = gameObject.GetComponent<Image>().sprite;
            _aiCharacterViewImage.sprite = _defaultCharacterSprite;
            return;
        }

        // AI�L�����N�^�[�̑I��
        if (_selectStepManager.NowSelectStep == 1)
        {
            _selectStepManager.NextAILevelSelect(_selectCharacterId);
            _aiCharacterViewImage.sprite = gameObject.GetComponent<Image>().sprite;
            return;
        }
    }
}
