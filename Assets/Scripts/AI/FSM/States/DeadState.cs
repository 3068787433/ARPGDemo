using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI.FSM
{
    public class DeadState : FSMState
    {
        public override void Init()
        {
            stateid = FSMStateID.Dead;
        }
        //被频繁反复的调用
        public override void Action(BaseFSM fsm)
        {
          
        }
        public override void EnterState(BaseFSM fsm)
        {
            fsm.PlayAnimation(fsm.animParams.Dead);
            fsm.enabled = false;//死亡状态 不再转入其它状态
        }
    }
}
