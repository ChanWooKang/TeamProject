using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable/Item")]
public class SOItems : ScriptableObject
{
    //������ ���̵�
    public int itemID;
    //������ �̸�
    public string itemName;
    //������ Ÿ�� ( ���, �Һ�, ��Ÿ ��)
    public eItemType itemType;
    //������ ���� �ִ�ġ ( ��� �������� 1)
    public int maxStack;
    //������ ����
    public int price;
    //������ �ŷ� ���� ����
    public bool isTradeItem;
    //������ ����
    public float itemWeight;

    //���� ������ ��� 
    //�ʿ� ������
    public List<RequiredItem> NeedItems;

    //UI ������
    public Sprite icon;
    //UI ������ �̸� �ѱ� �ۼ�
    public string krName;
    //UI ������ ����
    [Multiline]
    public string desc;
}


