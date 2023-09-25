using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ARPGDemo.Character;//
using UnityEngine;
using AI.Perception;

namespace AI.FSM
{
    /// <summary>
    /// 状态机：有限状态机最核心的一个类
    /// </summary>
    public class BaseFSM:MonoBehaviour
    {
        #region 1.0
        //字段 6
        private CharacterAnimation chAnim;
        private FSMState currentState;
        public FSMStateID currentStateId;

        private FSMState defaultState;
        public FSMStateID defaultStateId;
        //所有的状态对象=状态集合=状态库
        private List<FSMState> states = new List<FSMState>();
        //方法 1
        public void ChangActiveState(FSMTriggerID triggerid)
        { 
            //1 根据当前条件 确定 下一个状态是谁？待机》条件 》死亡
            var nextStateId=currentState.GetOutputState(triggerid);
            //如果是None返回
            if (nextStateId == FSMStateID.None)
                return;
            //如果不是None，继续判断是不是默认
            FSMState nextState=null;//状态对象
            if (nextStateId == FSMStateID.Default)
            {    nextState = defaultState;  }
            else
            {   nextState = states.Find(s => s.stateid == nextStateId); }                
            //2 退出当前状态
            currentState.ExitState(this); //大领导 张三
            //*****更新当前状态对象和状态编号
            currentState = nextState;//更新当前状态对象=编号 //新大领导：李四
            currentStateId = currentState.stateid;//
            //3 进入下一个状态
            currentState.EnterState(this);
        }
        #endregion

        #region 2.0
        //字段
        public string aiConfigFile="AI_01.txt";//AI配置文件《》硬编码 定制：状态转换表
        public AnimationParams animParams;//?
        public CharacterStatus chState;
        //方法
        public void PlayAnimation(string animPara)
        {
            chAnim.PlayAnimation(animPara);
        }
        private void Awake()
        {
            ConfigFSM();
        }
        //配置状态机：
        //方法1 硬编码 
        //方法2 使用AI配置文件 确定 条件 状态的映射关系
        private void ConfigFSM()
        {
            #region  方法1 硬编码 修改量大，代码复用性差 》找规律
            /*
            //1 创建状态对象
            IdleState idle = new IdleState();
            //2 添加条件映射
            idle.AddTrigger(FSMTriggerID.NoHealth, FSMStateID.Dead);
            idle.AddTrigger(FSMTriggerID.SawPlayer, FSMStateID.Pursuit);
            //3 放入状态集合=状态库
            states.Add(idle);

            //如下同理。添加其它的 状态映射

            //1 创建状态对象
            DeadState dead = new DeadState();
            //2 添加条件映射
            
            //3 放入状态集合=状态库
            states.Add(dead);

            //1 创建状态对象
            //2 添加条件映射
            //3 放入状态集合=状态库
            PursuitState pursuit = new PursuitState();
            pursuit.AddTrigger(FSMTriggerID.NoHealth, FSMStateID.Dead);
            pursuit.AddTrigger(FSMTriggerID.ReachPlayer, FSMStateID.Attacking);
            pursuit.AddTrigger(FSMTriggerID.LosePlayer, FSMStateID.Default);
            states.Add(pursuit);
             */
            #endregion 

            #region 使用AI配置文件 确定 条件 状态的映射关系
            //读取配置文件中的信息到字典中[IO]
            var dic = AIConfigurationReader.Load(aiConfigFile);//
            foreach(var stateName in dic.Keys)
            {
                //1 创建状态对象
                var type = Type.GetType("AI.FSM." + stateName + "State");
                var stateObj = Activator.CreateInstance(type) as FSMState;
                //2 添加条件映射
                foreach(var triggerId in dic[stateName].Keys)
                {
                    //string >对应 枚举
                    var trigger=(FSMTriggerID)(Enum.Parse(typeof(FSMTriggerID),triggerId));
                    var state=(FSMStateID)
                        (Enum.Parse(typeof(FSMStateID),dic[stateName][triggerId]));

                    stateObj.AddTrigger(trigger, state);
                }
                //3 放入状态集合=状态库
                states.Add(stateObj);
            }
           
            #endregion
        }
        /// <summary>
        /// 指定默认状态
        /// </summary>
        private void InitDefaultState()
        {
            //根据属性窗口指定的 默认状态编号 为其它三个字段赋值=初始化
            defaultState = states.Find(s => s.stateid == defaultStateId);
            currentState = defaultState;
            currentStateId = defaultStateId;
        }
        private void OnEnable()
        {
            InitDefaultState();//执行的时机 执行频率
            //临时调用 
            //InvokeRepeating("ResetTarget", 0, 0.2f);//3.0-1
        }        
        /// <summary>
        /// 初始化 字段
        /// </summary>
        public void Start()
        {
            chState = GetComponent<CharacterStatus>();
            chAnim = GetComponent<CharacterAnimation>();
            navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();//3.0-2
            chSkillSys = GetComponent<CharacterSkillSystem>();//4.0

            sightSensor = GetComponent<SightSensor>();
            if (sightSensor != null)
            { 
                sightSensor.OnPerception+=sightSensor_OnPerception;
                sightSensor.OnNonPerception+=sightSensor_OnNonPerception;
            }
        }
        //实时更新状态:实时检查条件的变化，条件一变-行为就变
        private void Update()
        {
            currentState.Reason(this);
            currentState.Action(this);
        }
        #endregion

        #region 3.0 ARPGDemo3.2 添加 待机》【发现目标】》追逐 需要的字段和功能
        //关注的目标tag
        public string[] targetTags = {"Player"};//
        //关注的目标物体
        public Transform targetObject=null;// string>Object
        //视距
        public float sightDistance =10;//注意 测试时
        //跑【追】的速度
        public float moveSpeed = 5;//
        //寻路：方法 路点  寻路组件网格寻路 
        private UnityEngine.AI.NavMeshAgent navAgent;

        //方法
        /// <summary>
        /// 1 重置目标
        /// </summary>
        private void ResetTarget()
        {
            //1 有tag标记 通过tag找 性能高！  
            List<GameObject> listTargets = new List<GameObject>();
            for (int i = 0; i < targetTags.Length; i++)
            {
                var targets = GameObject.FindGameObjectsWithTag(targetTags[i]);

                if (targets != null && targets.Length > 0)
                { listTargets.AddRange(targets); }
            }
            if (listTargets.Count == 0) return;
            //2  过滤：比较距离【指定半径】 所有的物体  同时 活着的 HP>0
            var enemys = listTargets.FindAll(go =>
                (Vector3.Distance(go.transform.position,
                this.transform.position) <sightDistance)
                && (go.GetComponent<CharacterStatus>().HP > 0)
                );
            if (enemys == null || enemys.Count == 0) return;
            //3  取最近的一个 
            targetObject=ArrayHelper.Min(enemys.ToArray(),
                        e => Vector3.Distance(this.transform.position,
                            e.transform.position)).transform;
        }
        /// <summary>
        /// 2 向目标跑，移向目标
        /// </summary>
        /// <param name="pos">目标当前的位置</param>
        /// <param name="speed">跑的速度</param>
        /// <param name="stopDistance">停止距离</param>
        public void MoveToTarget(Vector3 pos,float speed,float stopDistance)
        {
            navAgent.speed = speed;
            navAgent.stoppingDistance = stopDistance;
            navAgent.SetDestination(pos);
        }
        /// <summary>
        /// 3 停止移动
        /// </summary>
        public void StopMove()
        {
            navAgent.enabled = false;//
            navAgent.enabled = true;//
        }
        private void OnDisable()
        {
            //InitDefaultState();//执行的时机 执行频率   
            //放开 需求决定 HP=0》idle
            //删除              HP=0》dead
            //临时调用 
            //CancelInvoke("ResetTarget");
            //因为使用了感应器，所以死亡状态 感应器失效=移除
            if (currentStateId != FSMStateID.Dead)
            {
                currentState.ExitState(this);
                currentState = states.Find(p => p.stateid == FSMStateID.Idle);
                currentStateId = currentState.stateid;
                PlayAnimation(animParams.Idle);
            }
            else
            {                
                var sensors=GetComponents<AbstractSensor>();
                foreach (var item in sensors)
                {
                    item.enabled = false;
                }
            }
        }  
        #endregion

        #region 4.0
        private CharacterSkillSystem chSkillSys;
        public void AutoUseSkill()
        {
            chSkillSys.UseRandomSkill();
        }

        #endregion

        #region 5.0 定义寻路需要字段和方法
        //路点数组，
        public Transform[] wayPoints;
        //巡逻到达的距离，
        public float patrolArrivalDistance = 1;
        //速度，
        public float walkSpeed = 3;//
        //是否完成
        public bool IsPatrolComplete = false;
        //巡逻的方式
        public PatrolMode patrolMode = PatrolMode.Once;
        #endregion

        #region 6.0 引入智能感应器
        private SightSensor sightSensor;//1 引入 2注册事件
        public void sightSensor_OnPerception(List<AbstractTrigger> listTrigger)
        {
            targetObject = null;
            var  tempList=listTrigger.FindAll(p=>Array.IndexOf(targetTags,p.tag)>=0);
            if (tempList.Count > 0)
            {
                tempList = 
                    tempList.FindAll(p =>p.GetComponent<CharacterStatus>().HP > 0);
                if (tempList.Count > 0)
                {
                    targetObject = ArrayHelper.Min(tempList.ToArray(),
                        p => Vector3.Distance(p.transform.position,
                            transform.position)).transform;
                }
            }            
        }
        public void sightSensor_OnNonPerception()
        {
            targetObject = null;
        }  

        #endregion
    }
}
