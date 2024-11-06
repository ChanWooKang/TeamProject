using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class SkillInfo
{
    protected int _index;
    protected string _nameKr;
    protected string _nameEn;
    protected float _damageTimes;
    protected eAttackType _type;
    protected string _desc;
    protected int _rate;

    #region [ Property ] 
    public int Index { get { return _index; } }
    public string NameKr { get { return _nameKr; } }
    public string NameEn { get { return _nameEn; } }
    public float DamageTimes { get { return _damageTimes; } }
    public eAttackType Type { get { return _type; } }
    public string Desc { get { return _desc; } }
    public int Rate { get { return _rate; } }
    #endregion [ Property ]    

    public SkillInfo
        (int index, string nameKr, string nameEn,
            float damageTimes,eAttackType type, string desc, int rate)
    {
        _index = index;
        _nameKr = nameKr;
        _nameEn = nameEn;
        _damageTimes = damageTimes;
        _type = type;
        _desc = desc;
        _rate = rate;
    }
}
