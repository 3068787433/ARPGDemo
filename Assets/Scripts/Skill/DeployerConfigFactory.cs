using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARPGDemo.Skill
{
    /// <summary>
    /// 施放器的初始化工厂类=施放器的配置工厂类
    /// </summary>
    class DeployerConfigFactory
    {
        /// <summary>
        /// 工厂方法设计模式：创建目标选择对象的方法 符合 定义
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        public static IAttackSelector CreateAttackSelector(SkillData skill)
        {
            IAttackSelector attackSelector = null;
            #region switch
            //switch (skill.damageMode)
            //{ 
            //    case DamageMode.Circle:
            //        attackSelector = new CircleAttackSelector();
            //        break;
            //    case DamageMode.Sector:
            //        attackSelector = new SectorAttackSelector();
            //        break;
            //}
            #endregion

            #region 反射 创建实例 》》灵活性 复用性好
            //1得到Type类数据类型的对象！
            //string allName = "ARPGDemo.Skill.CircleAttackSelector";
            //string allName = "ARPGDemo.Skill."+"Circle"+"AttackSelector";
            string allName = "ARPGDemo.Skill." + skill.damageMode + "AttackSelector";
            Type typeObj = Type.GetType(allName);
            //2	动态【使用反射技术】创建对象
            object obj = Activator.CreateInstance(typeObj);
            attackSelector = (IAttackSelector)obj;
            #endregion

            return attackSelector;
        }
        /// <summary>
        /// 创建 初始化 自身影响
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        public static List<ISelfImpact> CreateSelfImpact(SkillData skill)
        {
            List<ISelfImpact> list = new List<ISelfImpact>();
            list.Add(new CostSPSelfImpact());
            //list.Add(new ...);
            return list;
        }
        /// <summary>
        ///  创建 初始化 目标影响
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        public static List<ITargetImpact> CreateTargetImpact(SkillData skill)
        {
            return new List<ITargetImpact>() 
            { 
                new DamageTargetImpact()
            }; 
        }
    }
}
