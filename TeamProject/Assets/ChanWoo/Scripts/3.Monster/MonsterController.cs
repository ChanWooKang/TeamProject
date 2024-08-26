using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : FSM<MonsterController>
{
    
    /** 상태
     * INITIAL
     * IDLE         ///(발견전 발견후)?
     * PATROL
     * SENSE, PEEK
     * CHASE
     * ATTACK
     * RETURN
     * DIE
     * KNOCKBACK
     * DISABLE     
     */

}
