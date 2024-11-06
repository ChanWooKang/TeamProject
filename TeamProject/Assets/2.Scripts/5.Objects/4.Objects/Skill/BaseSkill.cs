using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class BaseSkill : MonoBehaviour
{
    public float Damage;
    public int skillID;
    protected SkillInfo Info;
    public MonsterController MonCtrl;
    public BossCtrl BossCtrl;
    public PetController PetCtrl;
    public eSkillSubject _subject = eSkillSubject.None;

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
        _subject = eSkillSubject.Monster;
        MonCtrl = mCtrl;
    }
    public void SetBoss(BossCtrl bCtrl)
    {
        _subject = eSkillSubject.Boss;
        BossCtrl = bCtrl;
    }
    public void SetPet(PetController pCtrl)
    {
        _subject = eSkillSubject.Pet;
        PetCtrl = pCtrl;
    }
    
    public virtual void DestoryObject() { gameObject.DestroyAPS(); }
    
}
