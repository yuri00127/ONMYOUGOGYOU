using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacter : Button
{
    // �X�N���v�g
    [SerializeField] private CharacterDataBase _characterDataBase;
    [SerializeField] private SelectStepManager _selectStepManager;

    private string _characterIdFormat = "Character{0}";
    private int _selectCharacterId;

    // �L�����N�^�[�r���[
    [SerializeField] private Image _playerCharacterViewImage;
    [SerializeField] private Image _aiCharacterViewImage;
    [SerializeField] private Sprite _defaultCharacterSprite;

    private const int _aiLevelSelectStep = 2;
    private const int _playerCharacterSelectStep = 0;
    private const int _aiCharacterSelectStep = 1;


    public override void PointerEnter(GameObject gameObject)
    {
        if (_selectStepManager.NowSelectStep != _aiLevelSelectStep)
        {
            base.PointerEnter(gameObject);
        }

        // �v���C���[�L�����N�^�[�I����
        if (_selectStepManager.NowSelectStep == _playerCharacterSelectStep)
        {
            _playerCharacterViewImage.sprite = gameObject.GetComponent<Image>().sprite;
            return;
        }

        // AI�L�����N�^�[�I����
        if (_selectStepManager.NowSelectStep == _aiCharacterSelectStep)
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
        if (_selectStepManager.NowSelectStep != _aiLevelSelectStep)
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
        if (_selectStepManager.NowSelectStep == _playerCharacterSelectStep)
        {
            _selectStepManager.NextAICharacterSelect(_selectCharacterId);
            _playerCharacterViewImage.sprite = gameObject.GetComponent<Image>().sprite;
            _aiCharacterViewImage.sprite = _defaultCharacterSprite;
            return;
        }

        // AI�L�����N�^�[�̑I��
        if (_selectStepManager.NowSelectStep == _aiCharacterSelectStep)
        {
            _selectStepManager.NextAILevelSelect(_selectCharacterId);
            _aiCharacterViewImage.sprite = gameObject.GetComponent<Image>().sprite;
            return;
        }
    }
}
