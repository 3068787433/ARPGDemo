using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace ARPGDemo.Character
{
    /// <summary>
    /// 角色状态
    /// </summary>
    public abstract class CharacterStatus : MonoBehaviour, IOnDamager
    {
        /// <summary>
        /// 攻击距离
        /// </summary>
        public int attackDistance;

        /// <summary>
        /// 攻击速度
        /// </summary>
        public int attackSpeed;

        /// <summary>
        /// 伤害
        /// </summary>
        public int Damage;

        /// <summary>
        /// 防御
        /// </summary>
        public int Defence;

        /// <summary>
        /// 生命
        /// </summary>
        public int HP;

        /// <summary>
        /// 最大生命
        /// </summary>
        public int MaxHP;

        /// <summary>
        /// 最大魔法
        /// </summary>
        public int MaxSP;

        /// <summary>
        /// 魔法
        /// </summary>
        public int SP;

        /// <summary>
        /// 死亡
        /// </summary>
        abstract public void Dead();

        virtual public void OnDamage(int damageVal)
        {            
             //写所有受到伤害是共性的表现 HP减少！
             //受击者 有防御能力
             damageVal = damageVal - Defence;
             if(damageVal>0)HP -= damageVal;
             if (HP <= 0) Dead();
            //子类可以再加上个性的表现
        }
        //受击 同时播放受击 特效：需要找到 受击特效挂载点
        public Transform HitFxPos;
        private void Start()
        {
            HitFxPos = TransformHelper.FindChild(transform, "HitFxPos");
        }   
    }
}
