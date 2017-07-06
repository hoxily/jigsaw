using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    public readonly System.DateTime versionBegin = System.DateTime.Parse("2017-07-06 01:09");
    public readonly System.DateTime versionNow = System.DateTime.Parse("2017-07-06 19:54");
    public int version;
    public readonly string baseUrl = "http://192.168.1.211:8080/ABs/";
    public List<string> bundles = new List<string>
    {
        "sprites",
        "thumb",
        "uiprefabs",
        "sounds",
    };
    public List<string> levelIDs = new List<string>
    {
        "1",
        "2",
        "3",
        "4",
    };
    public static EntryPoint instance
    {
        get;
        private set;
    }
#if !UNITY_EDITOR
    private Dictionary<string, AssetBundle> abs = new Dictionary<string, AssetBundle>();
#endif
    public Transform uiPagesParent
    {
        get;
        private set;
    }
    private void Awake()
    {
        instance = this;
        version = System.Convert.ToInt32((versionNow - versionBegin).TotalMinutes);
    }
    IEnumerator Start()
    {
        yield return null;
#if !UNITY_EDITOR
        LoadingPageControl.instance.Show();
        LoadingPageControl.instance.BeginProgress("正在等待缓存模块准备完毕，请稍候……");
        while (!Caching.ready)
        {
            yield return null;
        }
        foreach (string bundle in bundles)
        {
            string url = baseUrl + bundle;
            WWW www = WWW.LoadFromCacheOrDownload(url, version);
            LoadingPageControl.instance.BeginProgress("正在加载资源包 " + bundle + "，请稍候……");
            while (!www.isDone && string.IsNullOrEmpty(www.error))
            {
                LoadingPageControl.instance.UpdateProgress(www.progress);
                yield return null;
            }
            if (www.isDone && string.IsNullOrEmpty(www.error))
            {
                abs.Add(bundle, www.assetBundle);
            }
            else
            {
                Debug.LogErrorFormat("资源包下载失败，{0}", www.error);
                Application.Quit();
            }
        }
        LoadingPageControl.instance.Hide();
#endif
        uiPagesParent = GameObject.Find("/UIRoot/NormalPages").transform;

        LoadUIPrefab<LevelsPageControl>("LevelsPage.prefab", uiPagesParent);
        LoadUIPrefab<PlayingPageControl>("PlayingPage.prefab", uiPagesParent);
        LoadUIPrefab<SoundManager>("SoundManager.prefab", null);
    }

    void LoadUIPrefab<T>(string name, Transform parent) where T : Component
    {
        GameObject go = loadAsset<GameObject>("uiprefabs", name) as GameObject;
        go = Instantiate(go, parent);
        if (go.GetComponent<T>() == null)
        {
            go.AddComponent<T>();
        }
    }

    public Object loadAsset<T>(string bundleName, string assetName)
    {
#if UNITY_EDITOR
        return UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/abs/" + bundleName + "/" + assetName, typeof(T));
#else
        AssetBundle ab = null;
        if (abs.TryGetValue(bundleName, out ab))
        {
            return ab.LoadAsset(assetName, typeof(T));
        }
        else
        {
            return null;
        }
#endif
    }
}
