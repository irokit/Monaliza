using System.Collections;
using System.Collections.Generic; // 引入用于列表的命名空间
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class RandomPrefabSpawner : MonoBehaviour
{
    public GameObject gamePrompt; //提示
    private CanvasGroup gamecanvasGroup;

    public GameObject addPrompt; //提示
    private CanvasGroup canvasGroup;

    public GameObject prefab; // 要生成的预制体
    public Button spawnButton; // 生成按钮
    public Button clearButton; // 清除按钮

    private List<GameObject> spawnedObjects = new List<GameObject>(); // 存储已生成的预制体
    private Vector3 cameraForward;
    private Vector3 cameraPosition;

    private bool isClicked = false;
    void Start()
    {
        // 为按钮添加点击事件
        spawnButton.onClick.AddListener(SpawnPrefab);
        clearButton.onClick.AddListener(ClearAllPrefabs);
        // 获取摄像头的方向
        cameraForward = Camera.main.transform.forward;

        // 获取摄像头的位置
        cameraPosition = Camera.main.transform.position;

        canvasGroup = addPrompt.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0; // 初始透明度为0

        gamecanvasGroup = gamePrompt.GetComponent<CanvasGroup>();
        StartCoroutine(FadeInOut(gamecanvasGroup));

    }

    void SpawnPrefab()
    {
        if (!isClicked)
        {
            isClicked = true; // 确保只在第一次点击时执行
            StartCoroutine(FadeInOut(canvasGroup));
        }
        // 生成一个随机的偏移量
        float randomX = Random.Range(-15f, 30f); // X轴的随机范围
        float randomY = Random.Range(-15f, 15f); // y轴的随机范围
        float randomZ = Random.Range(0f, 5f);  // Z轴的随机范围，确保在摄像头前方

        // 计算生成的位置
        Vector3 spawnPosition = -(cameraPosition + cameraForward * randomZ + Camera.main.transform.right * randomX+Camera.main.transform.up * randomY);

        // 生成随机大小
        float randomSize = Random.Range(0.3f, 1.3f); // 设置大小范围
        Vector3 randomScale = new Vector3(randomSize, randomSize, randomSize);

        // 实例化预制体并设置位置和大小
        GameObject instance = Instantiate(prefab, spawnPosition, Quaternion.identity);
        instance.transform.localScale = randomScale;

        // 将生成的预制体添加到列表中
        spawnedObjects.Add(instance);
    }

    void ClearAllPrefabs()
    {
        // 遍历已生成的预制体列表并销毁它们
        foreach (GameObject obj in spawnedObjects)
        {
            Destroy(obj);
        }

        // 清空列表
        spawnedObjects.Clear();
    }

    private IEnumerator FadeInOut(CanvasGroup canvasGroup)
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
