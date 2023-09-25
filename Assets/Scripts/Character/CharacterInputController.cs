using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//using ARPGDemo.Skill;
namespace ARPGDemo.Character
{
    /// <summary>
    /// 角色输入控制
    /// </summary>
    public class CharacterInputController:MonoBehaviour
    {
        /// <summary>
        /// 马达
        /// </summary>
        private CharacterMotor chMotor;
        /// <summary>
        /// 初始化：第一次赋值
        /// </summary>
        private void Start()
        {
            chMotor = GetComponent<CharacterMotor>();
        }
        /// <summary>
        /// 摇杆移动执行的方法
        /// </summary>
        public void JoystickMove(MovingJoystick move)
        {
            chMotor.Move(move.joystickAxis.x, move.joystickAxis.y);
        }
        /// <summary>
        /// 摇杆停止时执行的方法
        /// </summary>
        public void JoystickMoveEnd(MovingJoystick move)
        {
            chMotor.Move(move.joystickAxis.x, move.joystickAxis.y);
        }

        private void ButtonDown(string buttonName)
        {
           // var chSkillMgr= GetComponent<CharacterSkillManager>();
           //SkillData skill = null;
           var chSkillSys=GetComponent<CharacterSkillSystem>();
            switch (buttonName)
            { 
                case "Skill1":
                    //skill=chSkillMgr.PrepareSkill(11);    
                    chSkillSys.AttackUseSkill(11, false);
                    break;
                case "Skill2":
                    //skill=chSkillMgr.PrepareSkill(12);   
                    chSkillSys.AttackUseSkill(12, false);
                    break;                
            }
            //if(skill!=null) chSkillMgr.DeploySkill(skill);
        }
        //Easytouch的 事件通知的使用方式 1 设置UI：2 注册事件 3实现方法

        private void OnEnable()
        {
            EasyJoystick.On_JoystickMove += JoystickMove;
            EasyJoystick.On_JoystickMoveEnd += JoystickMoveEnd;
            EasyButton.On_ButtonDown+=ButtonDown;
        }
        private void OnDisable()
        {
            EasyJoystick.On_JoystickMove -= JoystickMove;
            EasyJoystick.On_JoystickMoveEnd -= JoystickMoveEnd;
            EasyButton.On_ButtonDown -= ButtonDown;
        }
    }
}
