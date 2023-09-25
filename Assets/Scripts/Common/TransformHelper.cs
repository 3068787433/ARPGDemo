using UnityEngine;
using System.Collections;

public class TransformHelper : MonoBehaviour {

	//找到FindChild 放在这里 递归！
    public static Transform FindChild(Transform trans, string goName)
    {
        Transform child = trans.Find(goName);
        if (child != null)
            return child;

        Transform go;
        for (int i = 0; i < trans.childCount; i++)
        {
            child = trans.GetChild(i);
            go = FindChild(child, goName);
            if (go != null)
                return go;
        }
        return null;
    }
    /// <summary>
    /// 转向
    /// </summary>
    public static void LookAtTarget(Vector3 target, 
        Transform transform, float rotationSpeed)
    {
        if (target != Vector3.zero)
        {
            Quaternion dir = Quaternion.LookRotation(target);
            transform.rotation = Quaternion.Lerp(transform.rotation, 
                dir, rotationSpeed);
        }
    }
}
