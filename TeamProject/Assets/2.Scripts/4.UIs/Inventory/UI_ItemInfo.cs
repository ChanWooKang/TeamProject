using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DefineDatas;

public class UI_ItemInfo : UI_Base
{    
    public static UI_ItemInfo _info { get { return TSingleton<UI_ItemInfo>._inst; } }
    enum GameObjects
    {
        MainFrame,                
        EffectParent,                
        AmountParent,        
        LevelParent        
    }

    enum Images
    {
        Icon,
    }

    enum Texts
    {
        ItemName,
        Effect,
        Desc,
        Amount,
        Weight,
        Level
    }

    GameObject _main;
    GameObject _effectParent;
    GameObject _amountParent;
    GameObject _levelParent;
    Image _icon;
    Text _itemName;
    Text _effect;
    Text _desc;
    Text _amount;
    Text _weight;
    Text _level;

    
    RectTransform _rect;
    public Camera UICamera;
    const string _format = "{0:#,###}";

    public override void Init()
    {        
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));
        _main = GetObject((int)GameObjects.MainFrame);
        _effectParent = GetObject((int)GameObjects.EffectParent);
        _amountParent = GetObject((int)GameObjects.AmountParent);
        _levelParent = GetObject((int)GameObjects.LevelParent);
        _icon = GetImage((int)Images.Icon);
        _itemName = GetText((int)Texts.ItemName);
        _effect = GetText((int)Texts.Effect);
        _desc = GetText((int)Texts.Desc);
        _amount = GetText((int)Texts.Amount);
        _weight = GetText((int)Texts.Weight);
        _level = GetText((int)Texts.Level);
        _rect = GetComponent<RectTransform>();
    }

    string SetTextEffect(eItemType type)
    {
        string data = null;
        switch (type)
        {
            case eItemType.Equipment:
                break;
            case eItemType.Weapon:                
                
                break;
        }

        return data;
    }

    public void SetInformation(BaseItem item, Vector3 pos, eItemType type, int cnt, PointerEventData eventData)
    {
        //������ ����
        _icon.sprite = InventoryManager._inst.GetItemSprite(item.Index);

        //������ �̸� ����
        _itemName.text = item.NameKr;

        //������ ȿ�� ���� (ȿ�� ���� ��� ��Ȱ��ȭ) ( ���, ���� ���ؼ� Ȱ��ȭ)
        //���� �߰� ( ��� , ����� �Ѱ��̹Ƿ� ���� )
        switch (type)
        {
            case eItemType.Equipment:
            case eItemType.Weapon:
                //ȿ��
                _effectParent.SetActive(false);
                _effect.text = null;
                //����
                _amountParent.SetActive(false);
                _amount.text = "";
                //����
                _level.text = item.Level.ToString();
                _levelParent.SetActive(true);
                break;
            default:
                //ȿ��
                _effectParent.SetActive(false);
                _effect.text = null;
                //����
                _amount.text = cnt.ToString();
                _amountParent.SetActive(true);
                //����
                _level.text = "";
                _levelParent.SetActive(false);
                break;
        }

        //������ ���� �Է�
        _desc.text = item.Desc;

        //����        
        int weight = Mathf.FloorToInt(item.Weight * cnt);
        _weight.text = string.Format(_format, weight);                
        

        transform.position = pos;
        _main.SetActive(true);
        SetPivot(eventData);
        
    }

    void SetPivot(PointerEventData eventData)
    {
        //Vector3 targetScreenPos = UICamera.WorldToScreenPoint(transform.position);
        Vector2 targetScreenPos = eventData.position;        
        // ȭ�� ���̺��� ���� ��� Pivot �� ����
        if(targetScreenPos.y + _rect.sizeDelta.y > Screen.height)
        {
            
            if(targetScreenPos.x + _rect.sizeDelta.x > Screen.width)
            {
                //���������� ȭ�� ������ ����
                _rect.pivot = Vector2.one;
            }
            else
            {

                _rect.pivot = Vector2.up;
            }
            
        }
        else 
        {            
            if (targetScreenPos.x + _rect.sizeDelta.x > Screen.width)
            {
                //���������� ȭ�� ������ ����
                _rect.pivot = Vector2.right;
            }
            else
            {
                _rect.pivot = Vector2.zero;
            }
        }
       
    }

    public void OffInformation()
    {
        _main.SetActive(false);
        transform.position = Vector3.zero;
    }
}
