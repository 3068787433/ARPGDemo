using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI.FSM
{
   public  class NoHealthTrigger:FSMTrigger
    {
       public override void Init()
       {
           triggerid = FSMTriggerID.NoHealth;
       }
       public override bool HandleTrigger(BaseFSM fsm)
       {           
           return fsm.chState.HP <= 0;
       }
    }
}
