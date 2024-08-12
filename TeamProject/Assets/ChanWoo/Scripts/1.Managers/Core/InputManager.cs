using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DefineDatas;

public class InputManager
{
    public Action KeyAction = null;
    public Action<MouseEvent> LeftMouseAction = null;
    public Action<MouseEvent> RightMouseAction = null;
    bool isLPress = false;
    bool isRPress = false;
    float LPressTime = 0;    
    float RPressTime = 0;
    const float tick = 0.25f;

    public void OnUpdate()
    {
        if (Input.anyKey && KeyAction != null)
            KeyAction.Invoke();

        //UI�� ���콺 �����Ͱ� ���� �� UIŬ�� �� ���콺 (��Ŭ��, ��Ŭ��)�۾� �Ұ�
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        
        //��Ŭ�� ����
        if (LeftMouseAction != null)
        {
            if (Input.GetMouseButton(0))
            {
                if (!isLPress)
                {
                    LeftMouseAction.Invoke(MouseEvent.PointerDown);
                    LPressTime = Time.time;
                }
                LeftMouseAction.Invoke(MouseEvent.Press);
                isLPress = true;
            }
            else
            {
                if (isLPress)
                {
                    if (Time.time > LPressTime + tick)
                        LeftMouseAction.Invoke(MouseEvent.Click);
                    LeftMouseAction.Invoke(MouseEvent.PointerUp);
                }
                isLPress = false;
                LPressTime = 0;
            }
        }

        // ��Ŭ�� ����

        if (RightMouseAction != null)
        {
            if (Input.GetMouseButton(1))
            {
                if (!isRPress)
                {
                    RightMouseAction.Invoke(MouseEvent.PointerDown);
                    RPressTime = Time.time;
                }
                RightMouseAction.Invoke(MouseEvent.Press);
                isRPress = true;
            }
            else
            {
                if (isRPress)
                {
                    if (Time.time > RPressTime + tick)
                        RightMouseAction.Invoke(MouseEvent.Click);
                    RightMouseAction.Invoke(MouseEvent.PointerUp);
                }
                isRPress = false;
                RPressTime = 0;
            }
        }
    }

    public void Clear()
    {
        LeftMouseAction = null;
        RightMouseAction = null;
        KeyAction = null;
    }

}
