using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

[CreateAssetMenu(fileName = "BuildObject", menuName = "Scriptable/BuildObject")]
public class SOBuild : ScriptableObject
{
    //건축물 아이디
    public int buildID;
    //건축물 이름
    public string buildName;
    //건축 완료 수치
    public float maxBuildValue;
    //필요 아이템 리스트
    public List<RequiredItem> NeedItems;

    //UI 파트
    public Sprite icon;
    public string krName;
    [Multiline]
    public string desc;
}
