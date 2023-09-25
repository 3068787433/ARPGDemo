using UnityEngine;
using ARPGDemo.Character;
using System.Collections;

namespace ARPGDemo.Skill
{
    //目标影响类：HP的减少【伤害】
    public class DamageTargetImpact: ITargetImpact
    {
        private int baseDamage = 0;
        /// 影响目标的操作【方法】     
        /// <param name="deployer">技能施放器</param>
        /// <param name="skillData">技能数据对象</param>
        /// <param name="goSelf">目标对象</param>
        public void TargetImpact(SkillDeployer deployer, SkillData skillData,
            GameObject goTarget)
        { 
            //获取技能拥有者的基础伤害
            if (skillData.Owner != null && skillData.Owner.gameObject != null)
            {
                baseDamage = skillData.Owner.
                    GetComponent<CharacterStatus>().Damage;
            }
            //执行伤害
            deployer.StartCoroutine(RepeatDamage(deployer, skillData));        
        }
        /// <summary>
        /// 单次伤害
        /// </summary>
        /// <param name="skill"></param>
        /// <param name="goTarget">伤害的目标物体</param>
        private void OnceDamage(SkillData skill,GameObject goTarget)
        { 
            //1调用 角色的OnDamage方法
            var chStatus=goTarget.GetComponent<CharacterStatus>();
            var damageVal = baseDamage * skill.damage;
            chStatus.OnDamage((int)damageVal);
            //2将受击特效 挂载 到目标身上
            if (skill.hitFxPrefab != null && chStatus.HitFxPos != null)
            {
                //1>创建一个受击特效预制件对象
                var hitGo = GameObjectPool.instance.CreateObject(
                     skill.hitFxName, skill.hitFxPrefab,
                     chStatus.HitFxPos.position, chStatus.HitFxPos.rotation);
                //2>将该对象挂载挂点上
                hitGo.transform.parent = chStatus.HitFxPos;
                //3>特效播放完回收
                GameObjectPool.instance.CollectObject(hitGo, 0.2f);
            }
        }
        //重复伤害 
        private IEnumerator RepeatDamage(SkillDeployer deploy, SkillData skill)
        {
            float attackTime = 0;
            do
            {
                if (skill.attackTargets != null && skill.attackTargets.Length > 0)
                {
                    //对多个 目标执行伤害 
                    for (int i = 0; i < skill.attackTargets.Length; i++)
                    {
                        OnceDamage(skill, skill.attackTargets[i]);//**
                    }
                }
                //间隔一个时间，再次执行伤害
                yield return new WaitForSeconds(skill.damageInterval);
                attackTime += skill.damageInterval;//!! durationTime不是0，
                                                                         //damageInterval也不能为0
                //攻击一次之后，要重新选取目标
                skill.attackTargets=deploy.ResetTarget();//先去实现施放器的 方法，再完成这里
            }
            while(attackTime<skill.durationTime);//!!防止死循环        
        }
    }
}
