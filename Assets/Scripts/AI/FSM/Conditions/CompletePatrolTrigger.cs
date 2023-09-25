using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI.FSM
{
    public  class CompletePatrolTrigger:FSMTrigger
    {
        public override void Init()
        {
            triggerid = FSMTriggerID.CompletePatrol;
        }
        public override bool HandleTrigger(BaseFSM fsm)
        {
            return fsm.IsPatrolComplete;//true
        }
    }
}
