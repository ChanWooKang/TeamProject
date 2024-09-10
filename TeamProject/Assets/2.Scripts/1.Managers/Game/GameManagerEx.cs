using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx : TSingleton<GameManagerEx>
{    
    public PlayerManager playeManager;
    public PlayerCtrl playerManager;

    // �������� ������ UI�� ���� �� ���� ���� ����
    public int UIStateValue = 0;
    

    //
    public bool isOnBuild = false;
    Coroutine OffBuildCoroutine = null;

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
