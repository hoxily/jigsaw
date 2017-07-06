using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Reflection;

public class Init : MonoBehaviour
{
    /// <summary>
    /// 动态加载进来的程序集
    /// </summary>
    public static Assembly loadedAssembly;
    /// <summary>
    /// DLL下载地址
    /// </summary>
    public static readonly string dllUrl = "http://192.168.1.211:8080/Assembly-CSharp-dynamic.dll";

    IEnumerator Start()
    {
        yield return null;
#if UNITY_EDITOR
        GameObject entryGO = new GameObject("EntryPoint");
        entryGO.AddComponent<EntryPoint>();
        LoadingPageControl.instance.Hide();
#else
        LoadingPageControl.instance.Show();
        LoadingPageControl.instance.BeginProgress("正在加载配置信息，请稍候……");
        WWW www = new WWW(dllUrl);
        while (!www.isDone && string.IsNullOrEmpty(www.error))
        {
            LoadingPageControl.instance.UpdateProgress(www.progress);
            yield return null;
        }
        if (www.isDone && string.IsNullOrEmpty(www.error))
        {
            byte[] bytes = www.bytes;
            www.Dispose();
            loadedAssembly = Assembly.Load(bytes);
            System.Type entryType = loadedAssembly.GetType("EntryPoint", false);
            if (entryType != null)
            {
                GameObject entryGO = new GameObject("EntryPoint");
                entryGO.AddComponent(entryType);
                LoadingPageControl.instance.Hide();
            }
            else
            {
                Debug.LogError("找不到DLL中的EntryPoint类！");
                Application.Quit();
            }
        }
        else
        {
            Debug.LogErrorFormat("DLL下载失败，{0}", www.error);
            Application.Quit();
        }
#endif
    }
    private void Awake()
    {
        // 避免熄灭屏幕
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
