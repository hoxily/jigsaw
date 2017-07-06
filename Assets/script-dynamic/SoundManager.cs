using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSourceBGM;
    private AudioSource temporaryAudioSource;
    private AudioClip win;
    private AudioClip merge;
    private AudioClip bgm;
    public static SoundManager instance
    {
        get;
        private set;
    }

    void Awake()
    {
        instance = this;
        audioSourceBGM = transform.Find("BGM").GetComponent<AudioSource>();
        temporaryAudioSource = transform.Find("Temporary").GetComponent<AudioSource>();
        win = EntryPoint.instance.loadAsset<AudioClip>("sounds", "win.wav") as AudioClip;
        merge = EntryPoint.instance.loadAsset<AudioClip>("sounds", "merge.wav") as AudioClip;
        bgm = EntryPoint.instance.loadAsset<AudioClip>("sounds", "bgm.mp3") as AudioClip;
    }

    private void Start()
    {
        audioSourceBGM.clip = bgm;
        audioSourceBGM.Play();
        audioSourceBGM.loop = true;
    }

    public void PlayMerge()
    {
        temporaryAudioSource.PlayOneShot(merge);
    }

    public void PlayWin()
    {
        temporaryAudioSource.PlayOneShot(win);
    }
}
