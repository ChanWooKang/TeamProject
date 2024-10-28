using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill : MonoBehaviour
{
    public float Damage;
    public int skillID;
    protected SkillInfo Info;
    public MonsterController MonCtrl;
    public BossCtrl BossCtrl;
    public PetController PetCtrl;

    protected void SetInformation()
    {
        if (Managers._data.Dict_Skill.ContainsKey(skillID))
        {
            Info = Managers._data.Dict_Skill[skillID];
        }
        else
        {
            Info = null;
        }
    }
    public void SetMon(MonsterController mCtrl)
    {
        MonCtrl = mCtrl;
    }
    public void SetBoss(BossCtrl bCtrl)
    {
        BossCtrl = bCtrl;
    }
    public void SetPet(PetController pCtrl)
    {
        PetCtrl = pCtrl;
    }
    
    public virtual void DestoryObject() { gameObject.DestroyAPS(); }
    
}
