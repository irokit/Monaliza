using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FrameRotate : MonoBehaviour
{
    public GameObject rotatePrompt; //提示
    private CanvasGroup canvasGroup;

    public Transform rotationCenter; // 旋转中心
    private float rotationSpeed = 200f; // 旋转速度
    private float angleToRotate = 180f; // 每次旋转的角度
    private bool isOpening = false; // 是否正在旋转
    private float currentAngle = 0f; // 当前旋转角度

    private bool isClicked = false;
    void Start()
    {
        canvasGroup = rotatePrompt.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0; // 初始透明度为0
    }
    private void Update()
    {
        // 检测鼠标点击
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (!isClicked)
                {
                    isClicked = true; // 确保只在第一次点击时执行
                    StartCoroutine(FadeInOut());
                }
                // 射线与物体相交，处理鼠标点击事件
                GameObject clickedObject = hit.collider.gameObject;

                // 判断点击的物体是否为带有“frame”标签的物体
                if (clickedObject.CompareTag("frame") && !isOpening)
                {
                    isOpening = true;
                    StartCoroutine(OpenDoor()); // 启动旋转协程
                }
            }
        }
    }

    private IEnumerator OpenDoor()
    {
        // 计算目标角度
        float targetAngle = currentAngle + angleToRotate;

        while (currentAngle < targetAngle)
        {
            float angleToRotateStep = rotationSpeed * Time.deltaTime; // 计算每帧旋转的角度

            // 这里确保不会超出目标角度
            if (currentAngle + angleToRotateStep > targetAngle)
            {
                angleToRotateStep = targetAngle - currentAngle;
            }

            currentAngle += angleToRotateStep; // 更新当前角度
            transform.RotateAround(rotationCenter.position, Vector3.up, angleToRotateStep); // 绕中心旋转

            yield return null; // 等待下一帧
        }

        // 更新当前角度为目标角度
        currentAngle = targetAngle;
        isOpening = false; // 旋转完成，重置状态
    }
    private IEnumerator FadeInOut()
    {
        // 淡入
        float duration = 1f;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(time / duration);
            yield return null;
        }

        // 等待两秒
        yield return new WaitForSeconds(2f);

        // 淡出
        time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1 - (time / duration));
            yield return null;
        }

        //isClicked = false; // 重置点击状态以允许再次点击
    }
}
