using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PreviewControl : MonoBehaviour
{
	Animator animator;
	int layerIndex;
    Button preview;
	void Awake ()
	{
		animator = GetComponent<Animator> ();
		layerIndex = animator.GetLayerIndex ("Base Layer");
        preview = GetComponent<Button>();
        preview.onClick.AddListener(OnPreviewButtonClicked);
	}

	public void OnPreviewButtonClicked ()
	{
		if (animator.GetCurrentAnimatorStateInfo (layerIndex).IsName ("Empty")) {
			animator.SetTrigger ("PreviewClicked");
		}
	}

    private void OnEnable()
    {
        OnPreviewButtonClicked();
    }
}
