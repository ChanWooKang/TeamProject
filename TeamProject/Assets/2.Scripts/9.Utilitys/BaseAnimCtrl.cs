using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class BaseAnimCtrl : MonoBehaviour
{

    //공격 방식간 가중치
    public float[] AttackTypeWeight;
    //근접 공격 개수 및 버프
    public float[] _meleeWeightProbs;
    //원거리 공격 개수 및 버프
    public float[] _rangeWeightProbs;

    public eAttackType GetAttackTypeByWeight()
    {
        eAttackType attackType = eAttackType.None;
        float randValue = Random.Range(0.0f, 1.0f);
        int number;
        for (int i = 0; i < AttackTypeWeight.Length; i++)
        {
            if (randValue <= AttackTypeWeight[i])
            {
                number = i + 1;
                attackType = (eAttackType)number;
                break;
            }
        }

        return attackType;
    }

    public int PickPattern(eAttackType type)
    {
        int index = 0;
        float[] probs = _meleeWeightProbs;
        if (type == eAttackType.RangeAttack)
            probs = _rangeWeightProbs;

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
