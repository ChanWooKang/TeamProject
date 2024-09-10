using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx : TSingleton<GameManagerEx>
{    
    public PlayerManager playeManager;
    public PlayerCtrl playerManager;

    // 움직임을 제한할 UI가 켜질 때 마다 값이 증가
    public int UIStateValue = 0;
    

    //
    public bool isOnBuild = false;
    Coroutine OffBuildCoroutine = null;

    //인벤토리 , 움직임 체크 UI , 크래프트 UI 떠졌을때 움직임 제한 및 커서 활성화
    public void ChangeCursorLockForUI(bool isOn)
    {        
        Cursor.lockState = isOn ? CursorLockMode.None : CursorLockMode.Locked;
    }


    //움직임 통제할 UI가 켜질 경우 true
    public void ControlUI(bool isOn, bool isMoveControl = false)
    {
        if (isOn)
        {
            //활성화 할때
            if (isMoveControl)
            {    
                UIStateValue++;                                
            }
            
        }
        else
        {
            //비활성화 할때
            if (isMoveControl)
            {
                UIStateValue = --UIStateValue <= 0 ? 0 : UIStateValue;                
            }                
        }

        ChangeCursorLockForUI(isOn);
    }

    public bool CheckIsMoveAble()
    {
        if (UIStateValue > 0)
            return false;

        return true;
    }

    public void OffBuildAction()
    {
        if (OffBuildCoroutine != null)
            StopCoroutine(OffBuildCoroutine);
        OffBuildCoroutine = StartCoroutine(OffCoroutine());
    }

    IEnumerator OffCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        isOnBuild = false;
    }
}
