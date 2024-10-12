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
        //아이콘 세팅
        _icon.sprite = InventoryManager._inst.GetItemSprite(item.Index);

        //아이템 이름 설정
        _itemName.text = item.NameKr;

        //아이템 효과 설정 (효과 없을 경우 비활성화) ( 장비, 무기 한해서 활성화)
        //개수 추가 ( 장비 , 무기는 한개이므로 제외 )
        switch (type)
        {
            case eItemType.Equipment:
            case eItemType.Weapon:
                //효과
                _effectParent.SetActive(false);
                _effect.text = null;
                //개수
                _amountParent.SetActive(false);
                _amount.text = "";
                //레벨
                _level.text = item.Level.ToString();
                _levelParent.SetActive(true);
                break;
            default:
                //효과
                _effectParent.SetActive(false);
                _effect.text = null;
                //개수
                _amount.text = cnt.ToString();
                _amountParent.SetActive(true);
                //레벨
                _level.text = "";
                _levelParent.SetActive(false);
                break;
        }

        //아이템 설명 입력
        _desc.text = item.Desc;

        //무게        
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
        // 화면 높이보다 높을 경우 Pivot 값 조정
        if(targetScreenPos.y + _rect.sizeDelta.y > Screen.height)
        {
            
            if(targetScreenPos.x + _rect.sizeDelta.x > Screen.width)
            {
                //오른쪽으로 화면 밖으로 나감
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
                //오른쪽으로 화면 밖으로 나감
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
