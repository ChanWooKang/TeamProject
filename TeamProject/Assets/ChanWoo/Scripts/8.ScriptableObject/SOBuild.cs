using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

[CreateAssetMenu(fileName = "BuildObject", menuName = "Scriptable/BuildObject")]
public class SOBuild : ScriptableObject
{
    //���๰ ���̵�
    public int buildID;
    //���๰ �̸�
    public string buildName;
    //���� �Ϸ� ��ġ
    public float maxBuildValue;
    //�ʿ� ������ ����Ʈ
    public List<RequiredItem> NeedItems;

    //UI ��Ʈ
    public Sprite icon;
    public string krName;
    [Multiline]
    public string desc;
}
