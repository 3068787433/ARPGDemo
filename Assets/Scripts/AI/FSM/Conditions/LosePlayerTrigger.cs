using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace AI.FSM
{
    /// <summary>
    /// 丢失玩家
    /// </summary>
   public  class LosePlayerTrigger:FSMTrigger
    {
        public override void Init()
         {
            triggerid = FSMTriggerID.LosePlayer;
         }
        public override bool HandleTrigger(BaseFSM fsm)
        {
            if (fsm.targetObject != null)
            {
                bool b = Vector3.Distance(fsm.targetObject.position,
                    fsm.transform.position) > fsm.sightDistance;
                if (b) fsm.targetObject = null;//!!!
                return b;
            }
            return true;//!!
        }
    }
}
