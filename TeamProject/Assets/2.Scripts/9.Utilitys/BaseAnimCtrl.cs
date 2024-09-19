using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class BaseAnimCtrl : MonoBehaviour
{
    public int nextSkill = 0;
    //�⺻ ���� or  ��ų ���� ����
    public float[] AttackWeights;        
    //�⺻ ���� ���� �� ����
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
            //��ų ���
            //MonsterController index�� ��ȯ            
            attackType = ChooseSkillType(index);
        }
        else
        {
            //�⺻ ����
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
