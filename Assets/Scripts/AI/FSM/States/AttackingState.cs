using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace AI.FSM
{
    /// <summary>
    /// 攻击状态
    /// </summary>
    class AttackingState:FSMState
    {
        public override void Init()
        {
            stateid = FSMStateID.Attacking;
        }
        //被频繁反复的调用
        private float attackTime;
        public override void Action(BaseFSM fsm)
        {
            if (attackTime>fsm.chState.attackSpeed)
            {   //调用攻击的方法，利用状态机调 fsm.
                fsm.AutoUseSkill();//攻击一次，希望反复攻击 
                attackTime = 0;
            }
            attackTime = attackTime + Time.deltaTime;//
            fsm.transform.LookAt(fsm.targetObject);
        }
        public override void EnterState(BaseFSM fsm)
        {
            fsm.StopMove();
            fsm.PlayAnimation(fsm.animParams.Idle);//!!!!
        }
        public override void ExitState(BaseFSM fsm)
        {            
        }
    }
}
