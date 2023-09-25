using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI.FSM
{

    /// <summary>
    /// 发现目标
    /// </summary>
    public class SawPlayerTrigger : FSMTrigger
    {
       public override void Init()
       {
           triggerid = FSMTriggerID.SawPlayer;
       }
       public override bool HandleTrigger(BaseFSM fsm)
       {
           bool b = false;
           if (fsm.targetObject != null) b = true;
           return b;
       }
    }
}
