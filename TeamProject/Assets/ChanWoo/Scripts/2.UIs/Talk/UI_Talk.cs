using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Talk : MonoBehaviour
{
    //UI Parent ������Ʈ
    public GameObject mainObject;
    //��ȭ ��� NPC �̸�
    public Text speakerText;
    // ��ȭ ����
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
