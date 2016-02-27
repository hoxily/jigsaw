using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public AudioClip winSound;
	public AudioClip mergedSound;
	FragmentManager fragmentManager;
	AudioSource audioPlayer;
	bool gameOver;
	void Start()
	{
		fragmentManager = GameObject.Find ("FragmentManager").GetComponent<FragmentManager>();
		gameOver  = false;
		audioPlayer = GetComponent<AudioSource>();
	}
	void Update()
	{
		if(!gameOver){
			if(fragmentManager.groups.Count == 1)
			{
				audioPlayer.PlayOneShot(winSound);
				gameOver = true;
			}
		}
	}
	public void PlayMergeSound()
	{
		audioPlayer.PlayOneShot(mergedSound);
	}
}
