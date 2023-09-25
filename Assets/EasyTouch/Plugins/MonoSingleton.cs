/// <summary>
/// Generic Mono singleton.
/// </summary>
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>{
	
	private static T m_Instance = null;
    
	public static T instance{
        get{
			if( m_Instance == null ){
                //��Ϊ��unity�д�������ķ�ʽ�ж��֣�����new��������������
                //������ȡ�ö���ǰ��ȥ�����в��ң����Ƿ��Ѿ������������
            	m_Instance = GameObject.FindObjectOfType(typeof(T)) as T;//�ҵ��˲����ֶ�����

                if( m_Instance == null ){
                    //û�ҵ��ٴ���
                    //����ʱ���������̳�MonoBehaviour��������ȥnew������ͨ�����ű����������ϵķ�ʽ
                    //�ȴ���һ�������壬Ȼ�󽫽ű�����ȥ�����õ�������
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