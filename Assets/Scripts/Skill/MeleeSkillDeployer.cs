using UnityEngine;
using System.Collections.Generic;
namespace ARPGDemo.Skill
{
    /// <summary>
    /// 技能 施放器【类】
    /// </summary>
    public class MeleeSkillDeployer:SkillDeployer 
    {
        public override void DeploySkill()
        {
            if (skillData == null) return;
            //1 确定目标
            skillData.attackTargets = ResetTarget();
            //2 执行自身影响
            listSelfImpact.ForEach(p=>p.SelfImpact(this,skillData,skillData.Owner));
            //3 执行目标影响
            listTargetImpact.ForEach(p => p.TargetImpact(this, skillData, null));
            //4回收技能
            CollectSkill();
        }  
    }
}
