using UnityEngine;
namespace ARPGDemo.Skill
{
    /// <summary>
    /// 自身影响算法[接口】
    /// </summary>
    public interface ISelfImpact
    {
        /// 影响自身的方法
        /// <param name="deployer">技能施放器</param>
        /// <param name="skillData">技能数据对象</param>
        /// <param name="goSelf">自身或队友对象</param>
        void SelfImpact(SkillDeployer deployer,SkillData skillData,GameObject goSelf);
    }
}