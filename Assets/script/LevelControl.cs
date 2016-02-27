using UnityEngine;
using System.Collections;

public class LevelControl : MonoBehaviour
{
	public void OnQuitButtonClicked ()
	{
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit ();
		#endif
	}

	public void OnLevelButtonClicked (string levelName)
	{
		Application.LoadLevel (levelName);
	}

	public void OnBackButtonClicked ()
	{
		Application.LoadLevel ("levels");
	}

	public void OnReloadButtonClicked ()
	{
		Application.LoadLevel (Application.loadedLevel);
	}
}
