using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class PetStateInit : TSingleton<PetStateInit>, IFSMState<PetController>
{
    public void Enter(PetController m)
    {
        m.Movement._offsetPos = m.transform.position;
        //������ �ڵ� ���� �� �� ����
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
