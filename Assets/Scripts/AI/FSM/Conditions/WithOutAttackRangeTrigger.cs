using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AI.FSM
{
    /// <summary>
    /// 玩家离开攻击范围：看不见了，看见但是无法攻击
    /// </summary>
    public class WithOutAttackRangeTrigger:FSMTrigger
    {
        public override void Init()
        {
            triggerid = FSMTriggerID.WithOutAttackRange;
        }
        public override bool HandleTrigger(BaseFSM fsm)
        {
            //1有目标 在视觉范围                    false 
            //2有目标 不在攻击范围但在视觉范围 true 
            //3没有目标                                 false                                          
            if (fsm.targetObject != null)
            {
                var distance = Vector3.Distance(fsm.targetObject.position,
                    fsm.transform.position);
                bool b = distance > fsm.chState.attackDistance 
                    && distance < fsm.sightDistance;
                return b;
            }
            return false;//!!! 攻击目标不存在
        }        
    }
}
