using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx : TSingleton<GameManagerEx>
{

    public bool isOnUI = false;

    //인벤토리 , 움직임 체크 UI , 크래프트 UI 떠졌을때 움직임 제한 및 커서 활성화
    public void ChangeCursorLockForUI(bool isOn)
    {
        isOnUI = isOn;

        Cursor.lockState = isOn ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
