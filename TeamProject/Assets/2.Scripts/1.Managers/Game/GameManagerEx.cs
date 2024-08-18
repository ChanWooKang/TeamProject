using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx : TSingleton<GameManagerEx>
{

    public bool isOnUI = false;

    //�κ��丮 , ������ üũ UI , ũ����Ʈ UI �������� ������ ���� �� Ŀ�� Ȱ��ȭ
    public void ChangeCursorLockForUI(bool isOn)
    {
        isOnUI = isOn;

        Cursor.lockState = isOn ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
