using System.Collections.Generic; // 引入用于列表的命名空间
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Exit : MonoBehaviour
{
    void Start()
    {
        Button exitButton = transform.GetComponent<Button>();
        exitButton.onClick.AddListener(Exitmanager);

    }
    public void Exitmanager()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//在Unity编译器中结束运行
        #else
        Application.Quit();//在可执行程序中结束运行
        #endif
    }
}
