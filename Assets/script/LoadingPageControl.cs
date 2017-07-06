using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPageControl : MonoBehaviour
{
    private Text loadingTips;
    private Slider loadingProgress;
    private Text percent;
    private Transform pageRoot;
    public static LoadingPageControl instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        instance = this;
        pageRoot = transform.Find("PageRoot");
        loadingProgress = pageRoot.Find("Slider").GetComponent<Slider>();
        percent = pageRoot.Find("Percent").GetComponent<Text>();
        loadingTips = pageRoot.Find("Loading").GetComponent<Text>();
    }

    public void Show()
    {
        pageRoot.gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        pageRoot.gameObject.SetActive(false);
    }

    public void UpdateProgress(float progress)
    {
        loadingProgress.value = progress;
        percent.text = (progress * 100.0f).ToString("F0") + "%";
    }

    public void BeginProgress(string message)
    {
        loadingProgress.value = 0.0f;
        percent.text = "0%";
        loadingTips.text = message;
    }
}
