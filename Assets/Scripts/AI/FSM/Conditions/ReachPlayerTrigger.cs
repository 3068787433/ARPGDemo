using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AI.FSM
{
    /// <summary>
    /// 目标进入攻击范围
    /// </summary>
    class ReachPlayerTrigger:FSMTrigger
    {
        public override void Init()
        {
            triggerid = FSMTriggerID.ReachPlayer;
        }
        public override bool HandleTrigger(BaseFSM fsm)
        {
            if(fsm.targetObject!=null)
            {
                bool b = Vector3.Distance(fsm.transform.position,
                 fsm.targetObject.position) < fsm.chState.attackDistance;
                return b;
            }
            return false;
        }
    }
}
