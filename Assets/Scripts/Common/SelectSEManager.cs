using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectSEManager : MonoBehaviour
{
    // Audio
    private AudioSource _audio;
    [SerializeField] private AudioClip SelectSE;    // ボタン切り替え時のSE

    [SerializeField] private GameObject _beforeObj;  // 直前にフォーカスしたGameObject

    // Start is called before the first frame update
    void Start()
    {
        _audio = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // 直前とは違うGameObjectをフォーカスしたとき、SEを鳴らす
        if (!ReferenceEquals(EventSystem.current.currentSelectedGameObject, _beforeObj))
        {
            _audio.PlayOneShot(SelectSE);

            GameObject newObj = EventSystem.current.currentSelectedGameObject;
            _beforeObj = newObj;
        }
    }
}
