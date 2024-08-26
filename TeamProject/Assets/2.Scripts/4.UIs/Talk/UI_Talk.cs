using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Talk : MonoBehaviour
{
    //UI Parent 오브젝트
    public GameObject mainObject;
    //대화 상대 NPC 이름
    public Text speakerText;
    // 대화 내용
    public Text dialogueText;

    public void Init()
    {
        SetText(null, null);
        SetOnOff(false);
    }

    public void SetText(string speaker, string dialogue)
    {
        speakerText.text = speaker;
        dialogueText.text = dialogue;
    }

    public void SetOnOff(bool isActive)
    {
        if (mainObject.activeSelf != isActive)
            mainObject.SetActive(isActive);
    }
}
