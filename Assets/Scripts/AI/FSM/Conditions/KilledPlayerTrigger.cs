using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using cstatus = ARPGDemo.Character.CharacterStatus;

namespace AI.FSM
{
    /// <summary>
    /// 打死目标
    /// </summary>
    class KilledPlayerTrigger:FSMTrigger
    {
          public override void Init()
         {
            triggerid = FSMTriggerID.KilledPlayer;
         }
          public override bool HandleTrigger(BaseFSM fsm)
          {
              if (fsm.targetObject != null)
              {
                  bool b = fsm.targetObject.
                      GetComponent<cstatus>().HP <= 0;
                  if (b) fsm.targetObject = null;//!!!
                  return b;
              }
              return true;
          }
    }
}
