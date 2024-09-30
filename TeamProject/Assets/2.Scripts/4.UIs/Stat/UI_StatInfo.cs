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
        BonusStat
    }

    enum Images
    {
        EXP_Fill,
    }
    public PlayerStat _stat;
    GameObject _statParent;
    UI_Stat[] _stats;
    Text _level;
    Text _requireEXP;
    Image _fillEXP;
    Text _bonusStat;    

    string _format;
    public override void Init()
    {
        _stat = GameManagerEx._inst.playerManager._stat;
        
        _format = "{0:N0}";
        Bind<GameObject>(typeof(GameObjects));
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
        _statParent = GetObject((int)GameObjects.Status_Parent);
        _stats = _statParent.GetComponentsInChildren<UI_Stat>();
        _level = GetText((int)Texts.LevelText);
        _requireEXP = GetText((int)Texts.RequiredEXP);
        _bonusStat = GetText((int)Texts.BonusStat);
        _fillEXP = GetImage((int)Images.EXP_Fill);
        SettingStat();
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
        SetUIsByBonusStat();
    }

    void SetUIsByBonusStat()
    {
        int bonusStat = _stat.BonusStat;
        _bonusStat.text = string.Format(_format, bonusStat);
        for (int i = 0; i < _stats.Length; i++)
        {
            _stats[i].SetUI(bonusStat > 0);
        }
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
