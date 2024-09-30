using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DefineDatas;

public class UI_StatInfo : UI_Base
{
    enum GameObjects
    {
        Status_Parent
    }

    enum Texts
    {
        LevelText,        
        RequiredEXP,
        BonusTitle,
        BonusStat,
        HP_Text
    }

    enum Images
    {
        EXP_Fill,
        HP_Fill
    }
    public PlayerStat _stat;
    GameObject _statParent;
    UI_Stat[] _stats;
    Text _level;
    Text _requireEXP;
    Text _hp;
    Image _fillEXP;
    Image _fillHP;
    Text _bonusTitle;
    Text _bonusStat;    

    string _format;
    string _unActiveFormat;
    bool _isSetting = false;
    private void Update()
    {
        if (_isSetting && InventoryManager._inst.invenUI.isOnUI)
        {
            SetUI();
            InventoryManager._inst.invenUI.SettingInvenWeight();
        }
    }

    public override void Init()
    {                
        _format = "<color=white>{0:N0}</color>";
        _unActiveFormat = "<color=grey>{0:N0}</color>";
        Bind<GameObject>(typeof(GameObjects));
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
        _statParent = GetObject((int)GameObjects.Status_Parent);
        _stats = _statParent.GetComponentsInChildren<UI_Stat>();
        _level = GetText((int)Texts.LevelText);
        _requireEXP = GetText((int)Texts.RequiredEXP);
        _hp = GetText((int)Texts.HP_Text);
        _bonusTitle = GetText((int)Texts.BonusTitle);
        _bonusStat = GetText((int)Texts.BonusStat);
        _fillEXP = GetImage((int)Images.EXP_Fill);
        _fillHP = GetImage((int)Images.HP_Fill);
        SettingStat();
        _isSetting = true;
    }

    void SettingStat()
    {
        for(int i = 0; i < _stats.Length; i++)
        {
            _stats[i].Init();
            _stats[i].SetStatInfo(this);            
        }
    }

    public void SetUI()
    {
        //Level
        int level = _stat.Level;
        _level.text = string.Format(_format, level);
        //Require EXP
        float requiredEXP = _stat.ConvertRequiredEXP;
        _requireEXP.text = string.Format(_format, requiredEXP);
        //Fill EXP         
        float rateEXP = _stat.ConvertEXP / _stat.ConvertTotalEXP;
        _fillEXP.fillAmount = rateEXP;        
        //BonusStat       
        SetHPUI();
        SetUIsByBonusStat();
    }

    void SetUIsByBonusStat()
    {
        int bonusStat = _stat.BonusStat;
        string title = "스테이터스 포인트";
        if(bonusStat > 0)
        {
            _bonusTitle.text = string.Format(_format, title);
            _bonusStat.text = string.Format(_format, bonusStat);
        }
        else
        {
            _bonusTitle.text = string.Format(_unActiveFormat, title);
            _bonusStat.text = string.Format(_unActiveFormat, bonusStat);
        }
        
        for (int i = 0; i < _stats.Length; i++)
        {
            _stats[i].SetUI(bonusStat > 0);
        }
    }

    void SetHPUI()
    {
        float HP = _stat.HP;
        float MaxHP = _stat.MaxHP;

        string format = "{0:N0} / <color=grey>{1:N0}</color>";
        _hp.text = string.Format(format, HP,MaxHP);
        _fillHP.fillAmount = HP / MaxHP;    
    }

    public float GiveData(eStatType type)
    {
        float data = 0;
        switch (type)
        {
            case eStatType.HP:
                data = _stat.MaxHP;
                break;
            case eStatType.Statmina:
                data = _stat.MaxStamina;
                break;
            case eStatType.Attack:
                data = _stat.Damage;
                break;
            case eStatType.Defense:
                data = _stat.Defense;
                break;
            case eStatType.WorkAblity:
                data = _stat.WorkAbility;
                break;
            case eStatType.CarryWeight:
                data = _stat.CarryWeight;
                break;
        }
        return data;
    }

    public void PlusStat(eStatType type)
    {        
        _stat.AddStat(type);
        SetUIsByBonusStat();
    }
}
