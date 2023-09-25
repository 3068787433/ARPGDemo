/// <summary>
/// Generic Mono singleton.
/// </summary>
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>{
	
	private static T m_Instance = null;
    
	public static T instance{
        get{
			if( m_Instance == null ){
                //因为在unity中创建对象的方式有多种（可用new、可用添加组件）
                //所以在取得对象前先去场景中查找，看是否已经出现这个对象
            	m_Instance = GameObject.FindObjectOfType(typeof(T)) as T;//找到了不再手动创建

                if( m_Instance == null ){
                    //没找到再创建
                    //创建时，如果对象继承MonoBehaviour，不建议去new，而是通过将脚本挂在物体上的方式
                    //先创建一个空物体，然后将脚本挂上去，最后得到这个组件
                    m_Instance = new GameObject("Singleton of " + typeof(T).ToString(), typeof(T))
                        .GetComponent<T>();
					 m_Instance.Init();
                }
               
            }
            return m_Instance;
        }
    }

    private void Awake(){
   
        if( m_Instance == null ){
            m_Instance = this as T;
        }
    }
 
    public virtual void Init(){}
 

    private void OnApplicationQuit(){
        m_Instance = null;
    }
}