using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace AI.Perception
{
    /// <summary>
    /// 视觉感应器
    /// </summary>
    public  class SightSensor:AbstractSensor
    {
        //字段
        //视距 
        public float sightDistance;
        //视角 
        public float sightAngle;
        //启用角度检测 
        public bool enableAngle;
        //启用遮挡检测
        public bool enableRay;
        //发射点
        public Transform sendPos;
        public override void Init()
        {
            if (sendPos == null) sendPos = transform;
        }
        protected override bool TestTrigger(AbstractTrigger trigger)
        {    //距离 角度 遮挡
            if (trigger.triggerType != TriggerType.Sight) return false;

            var tempTrigger = trigger as SightTrigger;
            var dir = tempTrigger.recievePos.position - sendPos.position;
            //1 距离
            var result=dir.magnitude < sightDistance;
            //2 角度
            if (enableAngle)
            {
                bool b1 = Vector3.Angle(transform.forward, dir) < sightAngle / 2;
                result = result && b1;
            }
            //3 遮挡 1>射中物体 2>射中的是触发器
            RaycastHit hit;
            if (enableRay)
            {
                bool b1 = Physics.Raycast(sendPos.position, dir, out hit, sightDistance) && hit.collider.gameObject == trigger.gameObject;
                result = result && b1;
            }
            return result;
        }
    }
}
