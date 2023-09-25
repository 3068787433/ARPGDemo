using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace ARPGDemo.Character
{
    /// <summary>
    /// 角色马达
    /// </summary>
    public class CharacterMotor:MonoBehaviour
    {
        /// <summary>
        /// 移动速度
        /// </summary>
        public float moveSpeed=5;
        /// <summary>
        /// 转向速度
        /// </summary>
        public float rotationSpeed=0.5f;
        /// <summary>
        /// 动画系统
        /// </summary>
        private CharacterAnimation chAnim;
        /// <summary>
        /// 角色控制器
        /// </summary>
        private CharacterController chController;

        private void Start()
        {
            chAnim = GetComponent<CharacterAnimation>();
            chController = GetComponent<CharacterController>();
        }
        /// <summary>
        /// 移动 =经验！方法多 
        /// </summary>
        public void Move(float x,float z)
        {
            if (x != 0 || z != 0)
            {
                //1转向 前往的方向
                TransformHelper.LookAtTarget(new Vector3(x, 0, z),
                    transform,rotationSpeed);
                //2向目标运动：核心 调用 内置组件提供的方法【角色控制器的 运动的方法】
                //-1 表示，模拟重力：保证主角贴在地面上，不会飘起来！ 
                Vector3 motion = new Vector3(transform.forward.x, -1, transform.forward.z);
                chController.Move(motion*Time.deltaTime*moveSpeed);
                //3播放运动动画  
                chAnim.PlayAnimation("run");
            }
            else
            {
                chAnim.PlayAnimation("idle");
            }
        }

       
    }
}
