using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace AI.Perception
{
   public abstract class AbstractSensor:MonoBehaviour
    {
        /// <summary>
        /// 是否移除，是否禁用
        /// </summary>
        public bool isRemove;
        /// <summary>
        /// 没有感知事件: F1>F2  [没有看到 做什么】
        /// </summary>
        public event Action OnNonPerception;
        /// <summary>
        /// 感知事件: F1>F2  [看到  做什么】
        /// </summary> 
        public event Action<List<AbstractTrigger>> OnPerception;
        private void Start()
        {
            Init();
            //把当前感应器放到 感应系统中
            SensorTriggerSystem sys = SensorTriggerSystem.instance;
            sys.AddSensor(this); 
           
        }
        abstract public void Init();
        private void OnDestroy()
        {
            isRemove = true;   //把当前感应器 从 感应系统中 移除
        }
        /// <summary>
        /// 检测触发器：检查所有的触发器【触发 条件】
        /// </summary>
        public void OnTestTrigger(List<AbstractTrigger> listTriggers)
        {
            //找到启用的所有触发器 
            listTriggers = listTriggers.FindAll(t => t.enabled
                && t.gameObject != this.gameObject
                && TestTrigger(t));
           //触发感知事件
            if (listTriggers.Count > 0)
            {
                if (OnPerception != null)
                    OnPerception(listTriggers);
            }
            else
            {
                if (OnNonPerception != null)                
                    OnNonPerception();                       
            }
        }
       
       /// <summary>
       /// 检测触发器 是否被感知
       /// </summary>
       /// <param name="trigger"></param>
        abstract protected bool TestTrigger(AbstractTrigger trigger);      
    }
}
