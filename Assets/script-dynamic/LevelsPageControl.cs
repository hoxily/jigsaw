using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ConfigObjects;

public class LevelsPageControl : MonoBehaviour
{
    private Transform pageRoot;
    private Button quit;
    private Transform levelItemsGroup;
    public static LevelsPageControl instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        instance = this;
        pageRoot = transform.Find("PageRoot");
        quit = pageRoot.Find("Quit").GetComponent<Button>();
        quit.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
        levelItemsGroup = pageRoot.Find("Scroll View/Viewport/LevelsGroup");
    }

    private void Start()
    {
        GameObject levelItemPrefab = EntryPoint.instance.loadAsset<GameObject>("uiprefabs", "LevelItem.prefab") as GameObject;
        foreach (string id in EntryPoint.instance.levelIDs)
        {
            GameObject levelItemGO = Instantiate(levelItemPrefab, levelItemsGroup) as GameObject;
            LevelItemControl control = levelItemGO.GetComponent<LevelItemControl>();
            if (control == null)
            {
                control = levelItemGO.AddComponent<LevelItemControl>();
            }
            string config = (EntryPoint.instance.loadAsset<TextAsset>("thumb", id + ".txt") as TextAsset).text;
            LevelConfig levelConfig = JsonUtility.FromJson<LevelConfig>(config);
            levelConfig.id = id;
            control.Setup(levelConfig);
        }
    }

    public void Show()
    {
        pageRoot.gameObject.SetActive(true);
    }

    public void Hide()
    {
        pageRoot.gameObject.SetActive(false);
    }
}
