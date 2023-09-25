using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace AI.FSM
{
    public  class PatrollingState:FSMState
    {
        public override void Init()
        {
            stateid = FSMStateID.Patrolling;
        }
        private int currentWayPoint;
        public override void Action(BaseFSM fsm)
        {
            //1是否到达当前路点
            if(Vector3.Distance(fsm.transform.position,
                fsm.wayPoints[currentWayPoint].position)<fsm.patrolArrivalDistance)//
            {
                //2是否是最后一个路点
                #region
                if (currentWayPoint == fsm.wayPoints.Length - 1)
                {
                    //根据巡逻的方式，决定 结束，再次开始【循环，来回】
                    switch (fsm.patrolMode)
                    { 
                        case PatrolMode.Once:
                            fsm.IsPatrolComplete = true;
                            return;//!!
                        case PatrolMode.PingPong:
                            System.Array.Reverse(fsm.wayPoints);//[0,1,2]
                            currentWayPoint += 1;//!!!???
                            break;
                        //循环用不用写？fsm.IsPatrolComplete = false;
                    }
                }
                #endregion
                currentWayPoint = (currentWayPoint + 1) % fsm.wayPoints.Length; 
            }
            //移动
            fsm.MoveToTarget(fsm.wayPoints[currentWayPoint].position,
                fsm.walkSpeed, fsm.patrolArrivalDistance);
            //播放动画
            fsm.PlayAnimation(fsm.animParams.Walk);
        }
        public override void EnterState(BaseFSM fsm)
        {
            fsm.IsPatrolComplete = false;
        }
        public override void ExitState(BaseFSM fsm)
        {
            fsm.StopMove();
        }
    }
}
