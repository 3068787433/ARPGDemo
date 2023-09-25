using UnityEngine;
public class CameraMovement : MonoBehaviour
{  //摄像机跟踪速度
    public float smooth = 1.5f;
    public Transform player;
    private Vector3 relCameraPos;
    void Awake()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        relCameraPos = transform.position - player.position;
    }
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position,
            player.position + relCameraPos,
            smooth * Time.deltaTime);
    }
}
