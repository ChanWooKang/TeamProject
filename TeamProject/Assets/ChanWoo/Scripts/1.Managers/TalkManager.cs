using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue
{    
    public int npcId;    
    public string krName;    
    public string[] contexts;
}

public class TalkManager : MonoBehaviour
{
    #region [ Singleton ]
    static TalkManager _uniqueInstance;
    public static TalkManager _talk { get { return _uniqueInstance; } }
    #endregion [ Singleton ]

    #region [ Component ]
    public UI_Talk talkUI;
    #endregion [ Component ]

    #region [ �������� ]
    Dictionary<int, Dialogue> talkData;
    public string fileName;
    public bool isEndTalk;
    int talkCounter;
    #endregion [ �������� ]

    void Awake()
    {
        _uniqueInstance = this;        
    }

    void Start()
    {
        LoadTalkData();
        talkUI.Init();
    }    

    public void ResetData()
    {
        isEndTalk = false;
        talkUI.SetOnOff(false);
        talkCounter = 0;
    }

    //CSV���� �ε� �� ȣ��
    public void LoadTalkData()
    {
        talkData = new Dictionary<int, Dialogue>();
        Dialogue[] dialogues = Parse(fileName);

        for (int i = 0; i < dialogues.Length; i++)
        {
            talkData.Add(dialogues[i].npcId, dialogues[i]);
        }
    }

    //CSV���� ��ȯ
    public Dialogue[] Parse(string csvFileName)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();
        TextAsset csvData = Resources.Load<TextAsset>(csvFileName);

        string[] data = csvData.text.Split(new char[] {'\n'});
        for (int i = 1; i < data.Length;)
        {
            string[] row = data[i].Split(new char[] { ',' });
            Dialogue dialogue = new Dialogue();
            dialogue.npcId = int.Parse(row[0]);
            dialogue.krName = row[1];            
            List<string> contextList = new List<string>();
            do
            {
                contextList.Add(row[2]);                
                if (++i < data.Length)
                    row = data[i].Split(new char[] { ',' });
                else
                    break;

            } while (string.IsNullOrEmpty(row[0].ToString()));
            
            dialogue.contexts = contextList.ToArray();
            dialogueList.Add(dialogue);
        }

        return dialogueList.ToArray();
    }

    //Object ID������ ��ġ�ϴ� ��ȭ ȣ��   
    //Index => ��ȭ Index��
    string GetTalk(int id, int talkIndex)
    {
        if (talkData.ContainsKey(id) == false)
            return null;

        if (talkIndex == talkData[id].contexts.Length)
            return null;
        return talkData[id].contexts[talkIndex];
    }

    public bool OnTalk(int objID, string speaker)
    {
        string talkContext = GetTalk(objID, talkCounter);

        if (string.IsNullOrEmpty(talkContext))
        {
            talkCounter = 0;
            return false;
        }

        //UI Text Change
        talkUI.SetText(speaker, talkContext);

        talkCounter++;
        return true;
    }

    public void ShowText(GameObject scanObj, int id, string speaker)
    {
        if(OnTalk(id,speaker) == false)
        {
            if (isEndTalk)
            {
                //UI Off
                talkUI.SetOnOff(false);

                isEndTalk = false;
                return;
            }

            if (scanObj.TryGetComponent(out ObjectData objData))
            {
                //NPC�϶� ��ȭ �� �ൿ
                if (objData.isNPC)
                {
                    if (scanObj.TryGetComponent(out BaseNPC npc))
                    {
                        npc.ActiveAction();
                    }                    
                }                
            }            
            isEndTalk = true;
        }
        else
        {
            //UI On
            talkUI.SetOnOff(true);
            
        }
    }

}
