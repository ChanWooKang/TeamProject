using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill : MonoBehaviour
{
    public float Damage;
    public int skillID;
    protected SkillInfo Info;


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

    public virtual void DestoryObject() { gameObject.DestroyAPS(); }
    
}
