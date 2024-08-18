using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class BaseItem
{
    protected int _index;
    protected eItemType _type;
    protected string _nameEn;
    protected string _desc;
    protected string _spriteName;
    protected string _nameKr;
    protected float _weight;

    //최대 스택 ( 엑셀 데이터 추가 요청 )
    protected int _maxStack;

    

    public int Index { get { return _index; } }
    public eItemType Type { get { return _type; } set { _type = value; } }
    public string NameEn { get { return _nameEn; } }
    public string Desc { get { return _desc; } }
    public string SpriteName { get { return _spriteName; } }
    public string NameKr { get { return _nameKr; } }
    public float Weight { get { return _weight; } }

    public int MaxStack { get { return _maxStack; } }

    public virtual void Init(int index, string nameEn, string desc, string spriteName, string nameKr, float weight)
    {
        _index = index;
        _nameEn = nameEn;
        _desc = desc;
        _spriteName = spriteName;
        _nameKr = nameKr;
        _weight = weight;
        _maxStack = 999;
    }

    
}
