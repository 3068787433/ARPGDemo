using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace AI.Perception
{
    /// <summary>
    /// 感知触发系统:感知系统 中介者
    /// </summary>
    public   class SensorTriggerSystem:
        MonoSingleton<SensorTriggerSystem>
    {
        private SensorTriggerSystem() { }
        /// <summary>
        /// 检查时间间隔
        /// </summary>
        public float checkInterval=0.2f;
        /// <summary>
        /// 感应器列表
        /// </summary>
        private List<AbstractSensor> listSensor=new List<AbstractSensor>();
        /// <summary>
        /// 触发器列表
        /// </summary>
        private List<AbstractTrigger> listTrigger=new List<AbstractTrigger>();

        /// <summary>
        /// 添加感应器
        /// </summary>
        public void AddSensor(AbstractSensor sensor)
        {
            listSensor.Add(sensor);
        }
        /// <summary>
        /// 添加触发器
        /// </summary>
        public void AddTrigger(AbstractTrigger trigger)
        {
            listTrigger.Add(trigger);
        }
        /// <summary>
        /// 检查触发条件:每个感应器 检查 对应所有触发器
        /// </summary>
        private void CheckTrigger()
        {
            for (int i = 0; i < listSensor.Count;i++ )
            {
                if (listSensor[i].enabled) 
                { 
                   
                    listSensor[i].OnTestTrigger(listTrigger);
                }
            }
        }
        /// <summary>
        /// 更新系统:
        /// </summary>
        private void UpdateSystem()
        {
            listSensor.RemoveAll(s => s.isRemove);
            listTrigger.RemoveAll(t => t.isRemove);
        }
        private void OnCheck()
        {
            UpdateSystem();
            CheckTrigger();
        }
        private void OnEnable()
        {
            InvokeRepeating("OnCheck", 0, checkInterval);
        }
        private void OnDisable()
        {
            CancelInvoke("OnCheck");
        }
    }
}
