using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DefineDatas;

public class UI_Information : UI_Base
{
    enum Images
    {
        HP_Fill,
        EXP_Fill
    }
    
    enum Texts
    {
        HP_Text
    }
    PlayerStat _stat;
    Text _hpText;
    Image _hpFill;
    Image _expFill;

    string _format;
    bool isSetting = false;

    public void Init(PlayerStat stat)
    {
        _stat = stat;
        Init();
    }

    public override void Init()
    {
        _format = "{0:N0} / <color=grey>{1:N0}</color>";
        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));
        _hpFill = GetImage((int)Images.HP_Fill);
        _expFill = GetImage((int)Images.EXP_Fill);
        _hpText = GetText((int)Texts.HP_Text);        
        isSetting = true;
    }

    public void OnUpdate()
    {
        if (isSetting)
        {
            SetUI();
        }
    }

    public void SetUI()
    {        
        //Fill EXP         
        float rateEXP = _stat.ConvertEXP / _stat.ConvertTotalEXP;
        _expFill.fillAmount = rateEXP;

        float hp = _stat.HP;
        float maxHP = _stat.MaxHP;
        _hpText.text = string.Format(_format, hp, maxHP);
        _hpFill.fillAmount = hp / maxHP;

    }
}
