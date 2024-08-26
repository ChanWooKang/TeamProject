using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DefineDatas;

public class UI_ItemInfo : UI_Base
{
    static UI_ItemInfo _uniqueInstance;
    public static UI_ItemInfo _info { get { return _uniqueInstance; } }
    enum GameObjects
    {
        MainFrame,
        ItemName,
        Icon,
        EffectParent,
        Effect,
        Desc,
        AmountParent,
        Amount,        
        Weight,
        PriceParent,
        Price
    }

    GameObject _main;
    GameObject _effectParent;
    GameObject _amountParent;
    GameObject _priceParent;
    Image _icon;
    Text _itemName;
    Text _effect;
    Text _desc;
    Text _amount;
    Text _weight;
    Text _price;

    
    RectTransform _rect;
    public Camera UICamera;
    const string _format = "{0:#,###}";


    private void Awake()
    {
        _uniqueInstance = this;
    }

    public override void Init()
    {        
        Bind<GameObject>(typeof(GameObjects));
        _main = GetObject((int)GameObjects.MainFrame);
        _effectParent = GetObject((int)GameObjects.EffectParent);
        _amountParent = GetObject((int)GameObjects.AmountParent);
        _priceParent = GetObject((int)GameObjects.PriceParent);
        _icon = GetObject((int)GameObjects.Icon).GetComponent<Image>();
        _itemName = GetObject((int)GameObjects.ItemName).GetComponent<Text>();
        _effect = GetObject((int)GameObjects.Effect).GetComponent<Text>();
        _desc = GetObject((int)GameObjects.Desc).GetComponent<Text>();
        _amount = GetObject((int)GameObjects.Amount).GetComponent<Text>();
        _weight = GetObject((int)GameObjects.Weight).GetComponent<Text>();
        _price = GetObject((int)GameObjects.Price).GetComponent<Text>();
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
                //ȿ�� ���� �߰� �� �f�󼣶�
                _effectParent.SetActive(false);
                _effect.text = null;

                _amountParent.SetActive(false);
                _amount.text = "";                
                break;
            case eItemType.Weapon:
                //ȿ�� ���� �߰� �� �f�󼣶�
                _effectParent.SetActive(false);
                _effect.text = null;

                _amountParent.SetActive(false);
                _amount.text = "";
                break;
            default:
                //ȿ��
                _effectParent.SetActive(false);
                _effect.text = null;
                //����
                _amount.text = cnt.ToString();
                _amountParent.SetActive(true);
                break;
        }

        //������ ���� �Է�
        _desc.text = item.Desc;

        //����        
        int weight = Mathf.FloorToInt(item.Weight * cnt);
        _weight.text = string.Format(_format, weight);

        //���� �ҰŸ� �߰� ( �����Ϳ��� ���� )
        //�ϴ� ������ ��Ȱ��
        _price.text = "";
        _priceParent.SetActive(false);

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
