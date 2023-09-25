using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace ARPGDemo.Character
{
    /// <summary>
    /// 主角状态
    /// </summary>
    public class PlayerStatus : CharacterStatus
    {
        /// <summary>
        /// 经验
        /// </summary>
        public int Exp;
        /// <summary>
        /// 最大经验
        /// </summary>
        public int MaxExp;

        /// <summary>
        /// 收集经验
        /// </summary>
        public void CollectExp()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 升级
        /// </summary>
        public void LevelUp()
        {
            throw new System.NotImplementedException();
        }

        public override void Dead()
        {
            print("PlayerStatus Dead ");
        }
        public override void OnDamage(int damageVal)
        {
            base.OnDamage(damageVal);
        }
       
    }
}