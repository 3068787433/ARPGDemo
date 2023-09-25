using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace ARPGDemo.Character
{
    /// <summary>
    /// 动画事件行为 : 播放某个动画F1{攻击动画}  某个行为发生 F2 {攻击方法}
    /// </summary>
    public class AnimationEventBehaviour:MonoBehaviour
    {
        /// <summary>
        /// 动画组件
        /// </summary>
        private Animator anim;
        private void Start()
        {
            anim = GetComponent<Animator>();
        }
        /// <summary>
        /// 撤销动画播放
        /// </summary>
        public void OnCancelAnim(string animName)
        {
            anim.SetBool(animName, false);
        }

        //定义委托：数据类型嵌套
        public delegate void AttackHandler();
        //1定义事件：使用事件设计模式：步骤  定义事件名称=声明委托对象
        public event AttackHandler attackHandler;
        //3触发事件
        /// <summary>
        /// 攻击时使用  ：触发攻击事件
        /// </summary>
        public void OnAttack()
        {
            if (attackHandler != null)
                attackHandler();
        }
    }

    
}
