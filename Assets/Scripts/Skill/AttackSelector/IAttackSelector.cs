using UnityEngine;
using System.Collections;

namespace ARPGDemo.Skill
{
    /// <summary>
    /// 攻击选择接口【算法】：选择  什么范围/区域 中的敌人 作为攻击目标 ，
    ///                                 例如：圆形范围/区域 中或扇形范围/区域 中
    /// </summary>
    public interface IAttackSelector 
    {

        /// <summary>
        /// 选择目标方法：选择 哪些敌人作为要攻击的 目标
        /// </summary>
        /// <param name="skillData">技能对象</param>
        /// <param name="transform">变换对象：选择时的参考点 ；技能拥有者</param>
        /// <returns></returns>
        GameObject[] SelectTarget(SkillData skillData, Transform skillTransform);     
    }
}
