using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue
{    
    public int npcId;    
    public string krName;    
    public string[] contexts;
}

public class TalkManager : TSingleton<TalkManager>
{   

    #region [ Component ]
    public UI_Talk talkUI;
    #endregion [ Component ]

    #region [ 전역변수 ]
    Dictionary<int, Dialogue> talkData;
    public string fileName;
    public bool isEndTalk;
    int talkCounter;
    #endregion [ 전역변수 ]


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

    //CSV파일 로드 후 호출
    public void LoadTalkData()
    {
        talkData = new Dictionary<int, Dialogue>();
        Dialogue[] dialogues = Parse(fileName);

        for (int i = 0; i < dialogues.Length; i++)
        {
            Debug.Log(dialogues[i].npcId);
            talkData.Add(dialogues[i].npcId, dialogues[i]);
        }
    }

    //CSV파일 변환
    public Dialogue[] Parse(string csvFileName)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();
        TextAsset csvData = Resources.Load<TextAsset>($"CSV/{csvFileName}");

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

    //Object ID값으로 일치하는 대화 호출   
    //Index => 대화 Index값
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
                //NPC일때 대화 및 행동
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
