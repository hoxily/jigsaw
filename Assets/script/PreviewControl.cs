using UnityEngine;
using System.Collections;

public class PreviewControl : MonoBehaviour
{
	Animator animator;
	int layerIndex;

	void Start ()
	{
		animator = GetComponent<Animator> ();
		layerIndex = animator.GetLayerIndex ("Base Layer");
	}

	public void OnPreviewButtonClicked ()
	{
		if (animator.GetCurrentAnimatorStateInfo (layerIndex).IsName ("Empty")) {
			animator.SetTrigger ("PreviewClicked");
		}
	}
}
