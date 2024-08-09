using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChanWooDefineDatas;

public class ChanWooTalkManager : MonoBehaviour
{
    static ChanWooTalkManager _uniqueInstance;
    public static ChanWooTalkManager Inst { get { return _uniqueInstance; } }
    public ChanWooUI_Talk talkUI;
    // NPC ID���� Ű�� �ν� string[] ��ȭ ����
    Dictionary<int, string[]> talkData;
    //�а� �ִ� talkIndex�� ����
    [SerializeField]int talkCounter;
    //��ȭ�� �Ϸ� �Ǿ����� üũ
    public bool isTalkEnd;

    public int TalkCounter { get { return talkCounter; } set { talkCounter = value; } }

    void Awake()
    {
        _uniqueInstance = this;
        LoadTalkData();
    }

    void Start()
    {
        talkUI.Init();
    }

    //DB ���� �� ȣ�� ( �׽�Ʈ �������� Awake���� �ٷ� ���� )
    public void LoadTalkData()
    {
        talkData = new Dictionary<int, string[]>();
        talkCounter = 0;
        isTalkEnd = false;
        GenerateData();
    }

    //�׽�Ʈ ���� ��ȭ ����
    void GenerateData()
    {
        talkData.Add(1000, new string[] {"Ʃ�丮�� �׽�Ʈ ��ȭ �����Դϴ�." });
        talkData.Add(1001, new string[] { "�������� �޾����� ����� ���� �� ����"});
    }

    //Ű������ ��ġ�ϴ� ��ȭ ���� �޾� ����
    string GetTalk(int id, int talkIndex)
    {
        if (talkIndex >= talkData[id].Length)
            return null;

        return talkData[id][talkIndex];
    }

    //GetTalk�Լ��� �ҷ����� talkIndex���� �����ؼ� ������
    //GetTalk�� ���������� UI�� ǥ��
    public bool OnTalk(int id, string speaker = null)
    {
        string data = GetTalk(id, talkCounter);
        //data�� null�� ��� ��ȭ�� �� �̻� ���� ���� ����
        if (string.IsNullOrEmpty(data))
        {
            talkCounter = 0;
            return false;
        }

        //UI data�� ǥ��
        talkUI.SetText(speaker, data);

        talkCounter++;
        return true;
    }

    //�������� ��ȭ üũ
    public void ShowText(GameObject scanObject,int id,string speakerName, eInteractiveType type = eInteractiveType.NPC)
    {

        //��ȭ�� �ҷ����µ� �Ϸ� ������� NPCType�� ���� �ൿ
        if (OnTalk(id, speakerName) == false)
        {
            //��ȭ�� ���� ������ �ѹ� �� ���� ���� �� UI����
            if (isTalkEnd)
            {
                //UI Off
                talkUI.SetOnOff(false);
                isTalkEnd = false;
                return;
            }
            switch (type)
            {
                case eInteractiveType.NPC:
                    if (scanObject.TryGetComponent(out ChanWooBaseNPC npc))
                    {                        
                        //NPC �ൿ ����
                        npc.ActiveAction();
                    }
                    else
                    {
                        Debug.Log("���� : NPC���۳�Ʈ�� �����ϴ� ");
                    }
                    break;
            }            
        }
        else
        {
            //��ȭ �� ���� ���̹Ƿ� UI On            
            talkUI.SetOnOff(true);
            isTalkEnd = false;
        }
        
    }
}
