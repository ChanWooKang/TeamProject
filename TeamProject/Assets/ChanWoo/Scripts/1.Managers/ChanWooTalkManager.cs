using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChanWooDefineDatas;

public class ChanWooTalkManager : MonoBehaviour
{
    static ChanWooTalkManager _uniqueInstance;
    public static ChanWooTalkManager Inst { get { return _uniqueInstance; } }
    public ChanWooUI_Talk talkUI;
    // NPC ID값을 키로 인식 string[] 대화 내용
    Dictionary<int, string[]> talkData;
    //읽고 있는 talkIndex값 저장
    [SerializeField]int talkCounter;
    //대화가 완료 되었는지 체크
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

    //DB 연결 후 호출 ( 테스트 버전에선 Awake에서 바로 실행 )
    public void LoadTalkData()
    {
        talkData = new Dictionary<int, string[]>();
        talkCounter = 0;
        isTalkEnd = false;
        GenerateData();
    }

    //테스트 버전 대화 생성
    void GenerateData()
    {
        talkData.Add(1000, new string[] {"튜토리얼 테스트 대화 내용입니다." });
        talkData.Add(1001, new string[] { "아이템을 받았으면 저기로 가라 더 없다"});
    }

    //키값으로 일치하는 대화 내용 받아 오기
    string GetTalk(int id, int talkIndex)
    {
        if (talkIndex >= talkData[id].Length)
            return null;

        return talkData[id][talkIndex];
    }

    //GetTalk함수를 불러오며 talkIndex값을 증가해서 가져옴
    //GetTalk로 가져왔으면 UI로 표시
    public bool OnTalk(int id, string speaker = null)
    {
        string data = GetTalk(id, talkCounter);
        //data가 null일 경우 대화가 더 이상 존재 하지 않음
        if (string.IsNullOrEmpty(data))
        {
            talkCounter = 0;
            return false;
        }

        //UI data값 표시
        talkUI.SetText(speaker, data);

        talkCounter++;
        return true;
    }

    //직접적인 대화 체크
    public void ShowText(GameObject scanObject,int id,string speakerName, eInteractiveType type = eInteractiveType.NPC)
    {

        //대화를 불러오는데 완료 됬을경우 NPCType에 따라 행동
        if (OnTalk(id, speakerName) == false)
        {
            //대화가 전부 끝나고 한번 더 실행 했을 때 UI끄기
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
                        //NPC 행동 진행
                        npc.ActiveAction();
                    }
                    else
                    {
                        Debug.Log("오류 : NPC컴퍼넌트가 없습니다 ");
                    }
                    break;
            }            
        }
        else
        {
            //대화 가 진행 중이므로 UI On            
            talkUI.SetOnOff(true);
            isTalkEnd = false;
        }
        
    }
}
