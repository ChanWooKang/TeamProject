using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx : TSingleton<GameManagerEx>
{        
    [Header("Object Script For Connect Other Scripts")]
    public PlayerCtrl playerManager;

    //UI = Update 통제
    [Header("UIs For Update Controll")]
    public UI_StatInfo _statInfo;
    public UI_Information _information;

    // 움직임을 제한할 UI가 켜질 때 마다 값이 증가
    public int UIStateValue = 0;


    bool isSetting = false;
    public bool isOnBuild = false;
    Coroutine OffBuildCoroutine = null;    

    private void Update()
    {
        if(isSetting)
            UIUpdate();

        if (Input.GetKeyDown(KeyCode.H))
        {
            InventoryManager._inst.ChangeItemLevel(200, 0);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            InventoryManager._inst.ChangeItemLevel(200, 1);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            InventoryManager._inst.ChangeItemLevel(200, 2);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            InventoryManager._inst.ChangeItemLevel(200, 3);
        }        
    }    

    public void GameMangerStart()
    {
        UIInit();
        isSetting = true;
    }

    public void UIInit()
    {
        if (_statInfo != null)
            _statInfo.Init(playerManager._stat);
        if (_information != null)
            _information.Init(playerManager._stat);        
    }

    void UIUpdate()
    {
        if (_statInfo != null)
            _statInfo.OnUpdate();
        if (_information != null)
            _information.OnUpdate();
    }

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
