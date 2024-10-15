using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx : TSingleton<GameManagerEx>
{        
    [Header("Object Script For Connect Other Scripts")]
    public PlayerCtrl playerManager;

    //UI = Update ����
    [Header("UIs For Update Controll")]
    public UI_StatInfo _statInfo;
    public UI_Information _information;

    // �������� ������ UI�� ���� �� ���� ���� ����
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

    //�κ��丮 , ������ üũ UI , ũ����Ʈ UI �������� ������ ���� �� Ŀ�� Ȱ��ȭ
    public void ChangeCursorLockForUI(bool isOn)
    {        
        Cursor.lockState = isOn ? CursorLockMode.None : CursorLockMode.Locked;
    }


    //������ ������ UI�� ���� ��� true
    public void ControlUI(bool isOn, bool isMoveControl = false)
    {
        if (isOn)
        {
            //Ȱ��ȭ �Ҷ�
            if (isMoveControl)
            {    
                UIStateValue++;                                
            }
            
        }
        else
        {
            //��Ȱ��ȭ �Ҷ�
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
