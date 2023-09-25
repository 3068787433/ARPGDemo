using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ARPGDemo.Skill;

namespace ARPGDemo.Character
{
    /// <summary>
    /// 角色系统和技能系统外观类：角色技能外观类
    /// </summary>
    public  class CharacterSkillSystem:MonoBehaviour
    {
        //字段
        private CharacterAnimation chAnim=null;        
        private CharacterSkillManager skillMgr = null;

        private GameObject currentAttackTarget = null;
        private SkillData currentUseSkill = null;
        //方法
        private void Start()
        {
            chAnim = GetComponent<CharacterAnimation>();
            skillMgr = GetComponent<CharacterSkillManager>();
            //使用攻击事件 注册  动画组件 》动画片段》
            //调用方法 OnAttack 触发事件》施放技能
            GetComponentInChildren<AnimationEventBehaviour>().
                attackHandler += DeploySkill;//使用事件 调用施放技能的方法

        }
        public void DeploySkill()
        {
            if(currentUseSkill!=null)
            skillMgr.DeploySkill(currentUseSkill);        
        }
        /// <summary>
        /// 使用指定编号的技能 进行攻击
        /// </summary>
        /// <param name="skillId">技能编号</param>
        /// <param name="isBatter">连续攻击：连攻</param>
        public void AttackUseSkill(int skillId,bool isBatter)
        {
            //如果是连续攻击，获取下一个技能编号
            if (isBatter && currentUseSkill != null)
                skillId = currentUseSkill.nextBatterId;//
            //1 通过编号准备 出 对应的技能数据对象
            currentUseSkill = skillMgr.PrepareSkill(skillId);
            if (currentUseSkill == null) return;
            //2 播放技能 对应的 攻击动画【技能施放 由动画事件调用】
            chAnim.PlayAnimation(currentUseSkill.animationName);//**!!!
            //3 找出受攻击的目标
            var selectedTarget = SelectTarget();
            if (selectedTarget == null) return;//!!!
            //4 显示选中的目标效果(模型 上 红圈)
            //让上一个目标 隐藏红圈，让当前目标 显示红圈
            ShowSelectedFx(false);
            currentAttackTarget = selectedTarget;//将刚刚找出的目标作为当前目标
            ShowSelectedFx(true);
            //5 面向目标  
            transform.LookAt(selectedTarget.transform);
        }
       
        /// <summary>
        /// 选择当前的攻击目标
        /// </summary>
        /// <returns></returns>
        private GameObject SelectTarget()
        {   
            //1 有tag标记 通过tag找 性能高！  暂时不:指定半径 
            //   找标记为 c.tag in attackTargetTags={"Enemy","Boss"}
            //   string string[]   Array.Index(stringArr，string )>=0
            List<GameObject> listTargets = new List<GameObject>();
            for (int i = 0; i < currentUseSkill.attackTargetTags.Length; i++)
            {
                var targets = GameObject.FindGameObjectsWithTag(currentUseSkill.attackTargetTags[i]);

                if (targets != null && targets.Length > 0)
                { listTargets.AddRange(targets); }
            }
            if (listTargets.Count == 0) return null;
            //2  过滤：比较距离【指定半径】 所有的物体  同时 活着的 HP>0
            var enemys = listTargets.FindAll(go =>
                (Vector3.Distance(go.transform.position,
                this.transform.position) < currentUseSkill.attackDistance)
                && (go.GetComponent<CharacterStatus>().HP > 0)
                );
            if (enemys == null || enemys.Count == 0) return null;
            //3  攻击时，一个一个的攻击 
            return   ArrayHelper.Min(enemys.ToArray(),
                        e => Vector3.Distance(this.transform.position,
                            e.transform.position));
        }
        /// <summary>
        /// 显示选中的目标效果(模型 上 红圈)
        /// </summary>
        /// <param name="isShow">显示或隐藏 </param>
        private void ShowSelectedFx(bool isShow)
        { 
            //定义特效
            Transform selectedFx = null;
            if (currentAttackTarget != null)
            {
                selectedFx = TransformHelper.
                    FindChild(currentAttackTarget.transform, "selected");
            }
            //控制物体的Renderer启用或禁用
            if (selectedFx != null)
            {
                selectedFx.GetComponent<Renderer>().enabled = isShow;
            }        
        }       
        public void UseRandomSkill()
        {
             //从技能列表随机抽取一个可用的技能
            //1>找出可用技能集合
             //{ 已经冷却了 + costSP《 技能拥有者的魔法 }
            var usableSkills = skillMgr.skills.FindAll(skill => skill.coolRemain == 0
                && skill.costSP <= skill.Owner.GetComponent<CharacterStatus>().SP);
            //2>随机抽取集合中的一个技能对象 
            if (usableSkills != null && usableSkills.Count > 0)
            {
                 var index=UnityEngine.Random.Range(0, usableSkills.Count);
                 var skillId=usableSkills[index].skillID;  //根据对象找编号
                //调用AttackUseSkill(int skillId,bool isBatter)
                 AttackUseSkill(skillId, false);
            }
        }
    }
}
