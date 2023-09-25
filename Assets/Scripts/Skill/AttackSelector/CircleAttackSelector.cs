using UnityEngine;
using System.Collections;
using System;
using ARPGDemo.Character;

namespace ARPGDemo.Skill
{
    /// <summary>
    /// 圆形攻击选择类：选择  圆形范围/区域 中的敌人 作为攻击目标                            
    /// </summary>
    public class CircleAttackSelector:IAttackSelector 
    {
        /// <summary>
        /// 选择目标方法：选择 圆形范围/区域 中的敌人作为要攻击的 目标
        /// </summary>
        /// <param name="skillData">技能对象</param>
        /// <param name="transform">变换对象：选择时的参考点 ；技能拥有者</param>
        /// <returns></returns>
        public GameObject[] SelectTarget(SkillData skillData, Transform skillTransform)
        {  
            //1 物体没有tag标记：通过射线找；
            //   有tag标记 通过tag找 性能高！  但是需要 :指定半径 
            var colliders= Physics.OverlapSphere(skillTransform.position, 
                skillData.attackDistance);
            if (colliders == null || colliders.Length == 0) return null;
            //2 找标记为 c.tag in attackTargetTags={"Enemy","Boss"} 所有的物体同时 活着的 HP>0
            //                           string string[]   Array.Index(stringArr，string )>=0/   
           var enemys=ArrayHelper.FindAll(colliders,c=>
                (Array.IndexOf(skillData.attackTargetTags,c.tag)>=0)
                &&(c.gameObject.GetComponent<CharacterStatus>().HP>0));
           if (enemys == null || enemys.Length == 0) return null;
            //3 根据技能攻击类型 确定返回单个或多个
           switch (skillData.attackType)
           { 
               case SkillAttackType.Group:
                   return ArrayHelper.Select(enemys, e => e.gameObject);
                   break;
               case SkillAttackType.Single:
                   var collider= ArrayHelper.Min(enemys,
                       e => Vector3.Distance(skillTransform.position,
                           e.transform.position));
                   return new GameObject[] { collider.gameObject };
                   break;
           }
           return null;
        }
    }
}
