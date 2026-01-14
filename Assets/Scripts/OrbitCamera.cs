using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//摄像机操作   

public class OrbitCamera : MonoBehaviour
{
    public Transform CenObj; // 围绕的物体
    private Vector3 Rotion_Transform;

    private new Camera camera;

    // 缩放限制
    public float minDistance = 10f; // 最小距离
    public float maxDistance = 16f; // 最大距离
    private float currentDistance; // 当前距离

    void Start()
    {
        camera = GetComponent<Camera>();
        Rotion_Transform = CenObj.position;

        // 初始化当前距离
        currentDistance = Vector3.Distance(transform.position, Rotion_Transform);
    }

    void Update()
    {
        Ctrl_Cam_Move();
        Cam_Ctrl_Rotation();
    }

    // 镜头的远离和接近
    public void Ctrl_Cam_Move()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            // 根据滚轮的值调整当前距离
            currentDistance -= scroll * 2f; // 调整缩放速度，1f可以自定义
            currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance); // 限制距离
            // 更新摄像机的位置
            Vector3 direction = (transform.position - Rotion_Transform).normalized;
            transform.position = Rotion_Transform + direction * currentDistance;
        }
    }

    // 摄像机的旋转
    public void Cam_Ctrl_Rotation()
    {
        var mouse_x = Input.GetAxis("Mouse X"); // 获取鼠标X轴移动
        var mouse_y = -Input.GetAxis("Mouse Y"); // 获取鼠标Y轴移动

        if (Input.GetKey(KeyCode.Mouse1))
        {
            transform.RotateAround(Rotion_Transform, Vector3.up, mouse_x * 5);
            transform.RotateAround(Rotion_Transform, transform.right, mouse_y * 5);
        }
    }
}
