using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class PetStateInit : TSingleton<PetStateInit>, IFSMState<PetController>
{
    public void Enter(PetController m)
    {
        m.Movement._offsetPos = m.transform.position;
        //레벨은 자동 생성 할 때 저장
        m.InitPetHud();
        m.BaseNavSetting();
        m.ChangeState(PetStateIdle._inst);
    }

    public void Execute(PetController m)
    {

    }

    public void Exit(PetController m)
    {

    }
}
