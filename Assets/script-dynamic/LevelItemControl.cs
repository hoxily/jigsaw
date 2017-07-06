using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ConfigObjects;

public class LevelItemControl : MonoBehaviour
{
    private Image thumb;
    private Button clickToPlay;
    private Text title;
    private RectTransform rectTF;
    private LevelConfig levelConfig;

    private void Awake()
    {
        thumb = GetComponent<Image>();
        clickToPlay = GetComponent<Button>();
        title = transform.Find("Title").GetComponent<Text>();
        rectTF = GetComponent<RectTransform>();
    }

    public void Setup(LevelConfig levelConfig)
    {
        this.levelConfig = levelConfig;
        float height = rectTF.sizeDelta.y;
        Sprite sprite = EntryPoint.instance.loadAsset<Sprite>("thumb", levelConfig.id + ".jpg") as Sprite;
        float ratio = (float)sprite.texture.width / (float)sprite.texture.height;
        float width = height * ratio;
        rectTF.sizeDelta = new Vector2(width, height);
        thumb.sprite = sprite;
        title.text = levelConfig.title;
        clickToPlay.onClick.AddListener(OnClickPlay);
    }

    private void OnClickPlay()
    {
        LevelsPageControl.instance.Hide();
        if (PlayingPageControl.instance == null)
        {
            Debug.LogError("PlayingPageControl.instance is null!");
        }
        PlayingPageControl.instance.Show();
        PlayingPageControl.instance.Setup(levelConfig);
    }
}
