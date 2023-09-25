using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace ARPGDemo.Character
{
    /// <summary>
    /// 角色动画系统
    /// </summary>
    public class CharacterAnimation:MonoBehaviour
    {
        /// <summary>
        /// 动画组件
        /// </summary>
        private Animator anim;
        private void Start()
        {
            anim = GetComponentInChildren<Animator>();
        }
        // 一个方法：两个功能，开始动画，结束动画
        string preAnimName = "idle";
        public void PlayAnimation(string animName)
        {
            anim.SetBool(preAnimName, false);
            anim.SetBool(animName, true);
            preAnimName = animName;//
        }
    }
}
