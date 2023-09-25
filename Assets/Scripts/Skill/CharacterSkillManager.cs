using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using ARPGDemo.Character;
using System.Collections;

namespace ARPGDemo.Skill
{
    public class CharacterSkillManager:MonoBehaviour
    {
        //字段  类使用 有的字段可以默认值，
        //                 有的不可以，需要有非默认值，需要程序员赋值 
        //                                                          简单类型  直接赋值，属性窗口修改
        //                                                          复杂类型  代码赋值【Start，.....]
        //管理多个技能数据对象---容器 所有技能
       public List<SkillData> skills= new List<SkillData>();        
        //方法
        //1.初始化
        private void Start()
        {
            //为skills中的 skill的有些字段初始化！
            foreach(var skill in skills)
            {
                //skill.prefabName==null||skill.prefabName==""
                //string.IsNullOrEmpty(skill.prefabName)
                if(!(string.IsNullOrEmpty(skill.prefabName))&&skill.skillPrefab==null)
                { 
                    skill.skillPrefab = LoadPrefab(skill.prefabName); 
                }
                if (!(string.IsNullOrEmpty(skill.hitFxName)) && skill.hitFxPrefab == null)
                {
                    skill.hitFxPrefab = LoadPrefab(skill.hitFxName);
                }              
                //为技能指定拥有者！！
                skill.Owner = this.gameObject;
            }  
        }
        /// <summary>
        /// 动态加载 预制件资源 
        /// </summary>
        /// <param name="resName">预制件资源名称</param>
        private GameObject LoadPrefab(string resName)
        {
            //动态加载 预制件资源
            //var prefabGo=Resources.Load<GameObject>(resName);
            var prefabGo = ResourceManager.Load<GameObject>(resName);//ok
            #region 使用游戏对象池 防止第一次使用技能时出现的卡帧现象
            var tempGo=GameObjectPool.instance.CreateObject(
                resName,prefabGo,
                transform.position,transform.rotation
                );
            GameObjectPool.instance.CollectObject(tempGo);
            #endregion
            return prefabGo;
        }
        //2.准备技能
        public SkillData PrepareSkill(int id)
        {
              //1根据技能id找 技能容器中是否有这个技能
              var skill = skills.Find(s => s.skillID == id);
              //2如果找到，同时 技能已经冷却  而且 SP足够，返回
              if (skill != null)
              {
                  if (skill.coolRemain == 0 && skill.costSP <=
                      skill.Owner.GetComponent<CharacterStatus>().SP)
                  {
                      return skill;
                  }
              }
              return null;
        }
        //3.施放技能  调用施放器的施放的方法即可
        public void DeploySkill(SkillData skillData)
        {
            //1 创建技能预制件对象  对象池创建
            var tempGo = GameObjectPool.instance.CreateObject(
               skillData.prefabName,skillData.skillPrefab,
               transform.position, transform.rotation
               );
            //2 为技能预制件对象 设置当前要使用 这个技能
            var deployer=tempGo.GetComponent<SkillDeployer>();
            //3 调用施放器的施放的方法
            deployer.skillData = skillData;
            deployer.DeploySkill();
            //4 冷却计时
            StartCoroutine(CoolTimeDown(skillData));
            //技能对象需要回收 留给施放器完成        
        }
        //4.技能冷却处理
        private IEnumerator CoolTimeDown(SkillData skillData)
        {
            skillData.coolRemain = skillData.coolTime;
            //冷却处理？ 循环 直到coolRemain=0 
            while (skillData.coolRemain > 0)
            { 
                //每隔1秒减 1
                yield return new WaitForSeconds(1);
                skillData.coolRemain -= 1;
            }
            skillData.coolRemain = 0;//为了保险起见  计算机基础！ 
        }
        //5.获取技能冷却剩余时间
        public int GetSkillCoolRemain(int id)
        {
            return skills.Find(s => s.skillID == id).coolRemain;
        }

    }
}
