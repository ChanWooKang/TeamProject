using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class BossStateInit : TSingleton<BossStateInit>, IFSMState<BossCtrl>
{
    public void Enter(BossCtrl m)
    {
        m._move._offsetPos = m.transform.position;
        m.SettingMonsterStatByLevel();
        m.InitData();
        m._move.BaseNavSetting();
        m._anim.SleepAction();
    }

    public void Execute(BossCtrl m)
    {
        
    }

    public void Exit(BossCtrl m)
    {
        
    }
}
