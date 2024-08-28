using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class MonsterStateInit : TSingleton<MonsterStateInit>, IFSMState<MonsterController>
{
    public void Enter(MonsterController m)
    {
        m._movement._offsetPos = m.transform.position;
        //������ �ڵ� ���� �� �� ����
        m.SettingMonsterStatByLevel();
        m.InitData();
        m.SetTarget();
        m.BaseNavSetting();
        m.ChangeState(MonsterStateIdle._inst);
    }

    public void Execute(MonsterController m)
    {

    }

    public void Exit(MonsterController m)
    {

    }
}
