using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI.FSM
{
    public class PursuitState : FSMState
    {
        public override void Init()
        {   //设置状态编号为 待机状态的编号
            stateid = FSMStateID.Pursuit;//!!
        }        
        public override void Action(BaseFSM fsm)
        {   //1：条件需要有追逐的目标 
            if (fsm.targetObject == null) return;
            //2：播放待机动画
            fsm.PlayAnimation(fsm.animParams.Run);
            //3:  主要控制追的 速度，靠近的距离=攻击距离
            fsm.MoveToTarget(fsm.targetObject.position,
                fsm.moveSpeed,
                fsm.chState.attackDistance);//attackDistance不要为零            
        }
        public override void ExitState(BaseFSM fsm)
        {   //4:停下来-转换状态【追逐>攻击] 
            fsm.StopMove();
        }
    }
}
