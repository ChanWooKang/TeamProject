using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class BaseAnimCtrl : MonoBehaviour
{
    public int nextSkill = 0;
    //기본 공격 or  스킬 선택 비중
    public float[] AttackWeights;        
    //기본 공격 개수 및 버프
    public float[] _meleeWeightProbs;

    protected int _animIDGetHit;
    protected int _animIDAttack;    
    protected int _animIDAttackPattern;

    protected void InitAnimData()
    {
        _animIDGetHit = Animator.StringToHash("GetHit");
        _animIDAttack = Animator.StringToHash("Attack");
        _animIDAttackPattern = Animator.StringToHash("Pattern");
    }

    public eAttackType ChooseAttackType(int index)
    {
        eAttackType attackType = eAttackType.None;
        nextSkill = 0;
        float randValue = Random.Range(0f, 1f);
        int number = 0;
        for(int i = 0; i < AttackWeights.Length; i++)
        {
            if(randValue <= AttackWeights[i])
            {
                number = i;
                break;
            }
        }

        if (number > 0)
        {
            //스킬 사용
            //MonsterController index값 소환            
            attackType = ChooseSkillType(index);
        }
        else
        {
            //기본 공격
            attackType = eAttackType.MeleeAttack;
        }

        return attackType;
    }

    eAttackType ChooseSkillType(int index)
    {
        eAttackType skillType = eAttackType.MeleeAttack;
        if(Managers._data.Dict_MonsterSkill.ContainsKey(index))
        {
            List<SkillInfo> skills = Managers._data.Dict_MonsterSkill[index];
            if(skills.Count > 0)
            {
                int maxWeight = 0;
                foreach (var skill in skills)
                {
                    maxWeight += skill.Rate;
                }

                int pivot = Random.Range(0, maxWeight);
                int cumulativeWeight = 0;
                foreach (var skill in skills)
                {
                    cumulativeWeight += skill.Rate;
                    if(pivot <= cumulativeWeight)
                    {
                        skillType = skill.Type;
                        nextSkill = skill.Index;
                        break;
                    }
                }
            }                        
        }

        return skillType;
    }    

    public int PickBaseAttackPattern()
    {
        int index = 0;
        float[] probs = _meleeWeightProbs;
        float randValue = Random.Range(0.0f, 1.0f);
        for (int i = 0; i < probs.Length; i++)
        {
            if (randValue <= probs[i])
            {
                index = i;
                break;
            }
        }
        return index;
    }
}
