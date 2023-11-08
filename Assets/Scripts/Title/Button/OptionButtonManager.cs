using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionButtonManager : Button
{
    [Header("�I�v�V�����r���[")]
    [SerializeField] private GameObject _optionView;
    [SerializeField] private GameObject _defaultForcus;

    [Header("�A�C�R��")]
    [SerializeField] private Sprite[] _sourceImage = new Sprite[2];

    private const string _assignmentButton = "Menu";


    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetAxisRaw(_assignmentButton) > 0) { Select(); }
    }

    public override void PointerEnter(GameObject gameObject)
    {
       
    }

    // �I�v�V�����r���[���J��
    public override void Select()
    {
        _optionView.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_defaultForcus);
    }
}
