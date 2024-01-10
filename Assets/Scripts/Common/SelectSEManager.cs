using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectSEManager : MonoBehaviour
{
    // Audio
    private AudioSource _audio;
    [SerializeField] private AudioClip SelectSE;    // �{�^���؂�ւ�����SE

    [SerializeField] private GameObject _beforeObj;  // ���O�Ƀt�H�[�J�X����GameObject

    // Start is called before the first frame update
    void Start()
    {
        _audio = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // ���O�Ƃ͈ႤGameObject���t�H�[�J�X�����Ƃ��ASE��炷
        if (!ReferenceEquals(EventSystem.current.currentSelectedGameObject, _beforeObj))
        {
            _audio.PlayOneShot(SelectSE);

            GameObject newObj = EventSystem.current.currentSelectedGameObject;
            _beforeObj = newObj;
        }
    }
}
