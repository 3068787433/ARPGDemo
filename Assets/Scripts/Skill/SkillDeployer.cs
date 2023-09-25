using UnityEngine;
using System.Collections.Generic;
namespace ARPGDemo.Skill
{
    /// <summary>
    /// 技能 施放器【类】
    /// </summary>
    abstract  public class SkillDeployer:MonoBehaviour
    {
        //字段
        //要施放的技能        
        private SkillData m_SkillData;
        public SkillData skillData
        {
            get { return m_SkillData; }
            set //使用一个属性 初始化所有字段！
            {
                if (value == null) return;
                m_SkillData = value;
                //调用 施放器的配置工厂类 从而实现了 初始化字段！
                attackSelector = DeployerConfigFactory.
                    CreateAttackSelector(m_SkillData);
                listSelfImpact = DeployerConfigFactory.
                    CreateSelfImpact(m_SkillData);
                listTargetImpact = DeployerConfigFactory.
                    CreateTargetImpact(m_SkillData);
            
            }
        }
        //攻击算法：
        protected IAttackSelector attackSelector;
        protected List<ISelfImpact> listSelfImpact = new List<ISelfImpact>();
        protected List<ITargetImpact> listTargetImpact = new List<ITargetImpact>();
        //private void Start()
        //{ 
        //     //初始化代码多，业务逻辑多，可能性多 ：多变的
        //     调用 施放器的配置工厂类 从而实现了 初始化字段！
        //}
        abstract public void DeploySkill();
        /// <summary>
        /// 重置目标：重新选择目标
        /// </summary>
        public GameObject[] ResetTarget() 
        {
            var targets= attackSelector.SelectTarget(m_SkillData, transform);
            if(targets!=null&&targets.Length>0) return targets;
            return null;
        }
        /// <summary>
        /// 技能回收
        /// </summary>
        public void CollectSkill() 
        {
            if (m_SkillData.durationTime > 0)
            {
                GameObjectPool.instance.
                    CollectObject(this.gameObject, m_SkillData.durationTime);
            }
            else
            {
                GameObjectPool.instance.
                       CollectObject(this.gameObject, 0.2f);
            }
        }

    }
}
