using UnityEngine;
using ARPGDemo.Character;

namespace ARPGDemo.Skill
{
    /// <summary>
    /// 自身影响类：SP消耗了，减少了
    /// </summary>
    public class CostSPSelfImpact:ISelfImpact
    {
        /// 影响自身的方法
        /// <param name="deployer">技能施放器</param>
        /// <param name="skillData">技能数据对象</param>
        /// <param name="goSelf">自身或队友对象</param>
        public void SelfImpact(SkillDeployer deployer, SkillData skillData,
            GameObject goSelf)
        {
            //找到技能拥有者的SP=找到技能拥有者的SP-skillData.costSP
            if (skillData.Owner == null) return;
            var chStatus=skillData.Owner.GetComponent<CharacterStatus>();
            chStatus.SP -=skillData.costSP;
        }
    }
}