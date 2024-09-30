using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DefineDatas;

public class UI_Stat : UI_Base
{
    enum GameObjects
    {
        PlusButton
    }

    enum Texts
    {
        StatValue
    }

    UI_StatInfo _statInfo;
    GameObject _plusButton;
    Text _statValue;

    [SerializeField] eStatType _myType;
    string _format;

    public override void Init()
    {
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        _statValue = GetText((int)Texts.StatValue);
        _plusButton = GetObject((int)GameObjects.PlusButton);
        BindEvent(_plusButton, (PointerEventData data) => { if (data.button == PointerEventData.InputButton.Left) PlusButtonDown(); }, MouseEvent.Click);
        _format = "{0:N0}";
    }

    public void SetStatInfo(UI_StatInfo info)
    {
        _statInfo = info;
    }

    public void SetUI(bool isOn)
    {
        float statValue = _statInfo.GiveData(_myType);
        _statValue.text = string.Format(_format,statValue);
        OnOffPlusButton(isOn);
    }

    void OnOffPlusButton(bool isOn)
    {
        _plusButton.SetActive(isOn);
    }

    public void PlusButtonDown()
    {
        _statInfo.PlusStat(_myType);
    }
}
