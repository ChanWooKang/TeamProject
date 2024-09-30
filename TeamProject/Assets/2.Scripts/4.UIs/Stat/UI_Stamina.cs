using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DefineDatas;

public class UI_Stamina : UI_Base
{    
    enum Images
    {
        Stamina
    }

    public GameObject _main;
    Image _stamina;
    public PlayerStat _stat;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if(_stat.Stamina < _stat.MaxStamina)
        {
            if (_main.activeSelf == false)
                _main.SetActive(true);
            FillAmountByStamina();
        }
        else
        {
            if (_main.activeSelf == true)
                _main.SetActive(false);
        }
    }

    public override void Init()
    {
        Bind<Image>(typeof(Images));
        _stamina = GetImage((int)Images.Stamina);
    }

    public void FillAmountByStamina()
    {
        _stamina.fillAmount = _stat.Stamina / _stat.MaxStamina;
    }
}
