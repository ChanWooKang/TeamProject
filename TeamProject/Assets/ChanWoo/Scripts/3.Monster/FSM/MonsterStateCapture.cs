using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class MonsterStateCapture : TSingleton<MonsterStateCapture>, IFSMState<MonsterController>
{
    Transform targetTransform;
    float cntTime;
    public void Enter(MonsterController m)
    {
        targetTransform = m._captureModel;
        cntTime = 0;
        m.ChangeModelByCapture(true);
        m.JumpNavSetting();
        m.MakeJump();                
    }

    public void Execute(MonsterController m)
    {
        
        cntTime += Time.deltaTime;
        float value = (1 - cntTime) <= 0 ? 0 : 1 - cntTime;
        targetTransform.localScale = Vector3.one * (value);
        if (cntTime > 2.0f)
        {
            targetTransform.gameObject.SetActive(false);            
        }
        
    }

    public void Exit(MonsterController m)
    {
        m.ChangeModelByCapture(false);        
    }
}
