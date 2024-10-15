using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue
{
    public int index;
    public string npcName;
    public string[] contexts;
}

public class TalkManager : TSingleton<TalkManager>
{
    public UI_Talk talkUI;

    Dictionary<int, Dialogue> _talkData;
    public string _fileName;
    public bool isTalking;
    int _talkCounter;
    

    private void Start()
    {
        InitData();
    }

    void InitData()
    {
        isTalking = false;
        _talkCounter = 0;
        LoadTalkData();        
    }

    #region [ Load Data ]
    void LoadTalkData()
    {
        AddData(Parse(_fileName));
    }

    void AddData(Dialogue[] datas)
    {
        _talkData = new Dictionary<int, Dialogue>();
        for (int i = 0; i < datas.Length; i++)
            _talkData.Add(datas[i].index, datas[i]);
    }

    Dialogue[] Parse(string fileName)
    {
        List<Dialogue> talkList = new List<Dialogue>();
        TextAsset fileData = Resources.Load<TextAsset>($"CSV/{fileName}");

        string[] datas = fileData.text.Split(new char[] { '\n' });
        for (int i = 1; i < datas.Length;)
        {
            string[] row = datas[i].Split(new char[] { ',' });            
            Dialogue dialogue = new Dialogue();
            dialogue.index = int.Parse(row[0]);
            dialogue.npcName = row[1];
            List<string> contextList = new List<string>();
            do
            {
                contextList.Add(row[2]);
                if (++i < datas.Length)
                    row = datas[i].Split(new char[] { ',' });
                else
                    break;
            } while (string.IsNullOrEmpty(row[0].ToString()));
            dialogue.contexts = contextList.ToArray();
            talkList.Add(dialogue);
        }

        return talkList.ToArray();
    }
    #endregion [ Load Data ]

    string GetTalk(int index, int talkIndex)
    {
        //해당하는 인덱스가 존재 하지 않을 경우
        if (!_talkData.ContainsKey(index))
            return null;

        //대화 내용을 전부 불러온 이후 일 경우
        if (talkIndex >= _talkData[index].contexts.Length)
            return null;

        return _talkData[index].contexts[talkIndex];
    }

    public bool OnTalk(int index)
    {
        string context = GetTalk(index, _talkCounter);

        //해당하는 데이터가 불러오지 않은 경우
        if (string.IsNullOrEmpty(context))
        {
            _talkCounter = 0;
            return false;
        }

        string speaker = _talkData[index].npcName;
        //UI Text 변경
        talkUI.SetText(speaker, context);
        _talkCounter++;
        return true;
    }

    //직접적으로 텍스트 호출
    //public void ShowText(GameObject scanObject, int index)
    //{
    //    isTalking = OnTalk(index);
    //    BaseNPC npc = null;
    //    bool isNPC = false;

    //    if (scanObject.TryGetComponent(out ObjectData data))
    //    {
    //        isNPC = data.isNPC && scanObject.TryGetComponent(out npc);            
    //    }        

    //    if (isTalking)
    //    {
    //        talkUI.SetOnOff(true);
    //        if (isNPC)
    //            npc.Talking();
    //    }
    //    else
    //    {
    //        if (UICoroutine != null)
    //            StopCoroutine(UICoroutine);
    //        UICoroutine = StartCoroutine(OffUI(0.5f));

    //        if (isNPC)
    //            npc.ActiveAction();            
    //    }
    //}

    public void ShowText(GameObject scanObject, int index)
    {
        bool isTalk = OnTalk(index);
        talkUI.SetOnOff(isTalk);

        if (scanObject.TryGetComponent(out ObjectData data))
        {
            if(data.isNPC && scanObject.TryGetComponent(out BaseNPC npc))
            {
                if (isTalk)
                    npc.Talking();
                else
                    npc.ActiveAction();
            }
        }        
    }

    public void TalkingEnd()
    {
        talkUI.SetOnOff(false);        
    }

    IEnumerator OffUI(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        talkUI.SetOnOff(false);
    }
}