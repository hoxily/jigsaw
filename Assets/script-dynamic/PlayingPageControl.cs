using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ConfigObjects;

public class PlayingPageControl : MonoBehaviour
{
    private Transform pageRoot;
    private Button back;
    private Button reload;
    private Transform preview;
    private FragmentManager fragmentManager;
    private LevelConfig levelConfig;
    public static PlayingPageControl instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        instance = this;
        pageRoot = transform.Find("PageRoot");
        back = pageRoot.Find("Back").GetComponent<Button>();
        back.onClick.AddListener(() =>
        {
            Hide();
            LevelsPageControl.instance.Show();
        });
        reload = pageRoot.Find("Reload").GetComponent<Button>();
        reload.onClick.AddListener(() =>
        {
            CleanUp();
            Setup(levelConfig);
        });
        preview = pageRoot.Find("Preview");
        if (preview.GetComponent<PreviewControl>() == null)
        {
            preview.gameObject.AddComponent<PreviewControl>();
        }
        fragmentManager = new FragmentManager();
    }

    private void CleanUp()
    {
        preview.GetComponent<Image>().sprite = null;
        fragmentManager.CleanUp();
    }

    public void Show()
    {
        pageRoot.gameObject.SetActive(true);
    }

    public void Hide()
    {
        pageRoot.gameObject.SetActive(false);
        CleanUp();
    }

    public void Setup(LevelConfig levelConfig)
    {
        Sprite sprite = EntryPoint.instance.loadAsset<Sprite>("sprites", levelConfig.id + ".jpg") as Sprite;
        preview.GetComponent<Image>().sprite = sprite;
        this.levelConfig = levelConfig;
        fragmentManager.SetupFragments(levelConfig);
    }
}
