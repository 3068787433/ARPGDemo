using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI.FSM
{
    public class IdleState:FSMState
    {
        public override void Init()
        {
            //设置状态编号为 待机状态的编号
            stateid = FSMStateID.Idle;
        }        
        public override void Action(BaseFSM fsm)
        {
            //播放待机动画
            fsm.PlayAnimation(fsm.animParams.Idle);
        }
    }
}
