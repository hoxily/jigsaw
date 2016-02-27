using UnityEngine;
using System.Collections;

public class Fragment : MonoBehaviour
{
	[HideInInspector]
	public FragmentManager fragmentManager;
	Vector3 previousMousePosition;
	Vector3 previousPosition;
	Camera mainCamera;
	[HideInInspector]
	public SpriteRenderer spriteRenderer;
	[HideInInspector]
	public int index;
	[HideInInspector]
	public int row;
	[HideInInspector]
	public int column;
	public FragmentGroup group;

	public int sortingOrder {
		get {
			return spriteRenderer.sortingOrder;
		}
		set {
			spriteRenderer.sortingOrder = value;
		}
	}

	void Awake ()
	{
		mainCamera = Camera.main;
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	void OnMouseOver ()
	{
		previousMousePosition = mainCamera.ScreenToWorldPoint (Input.mousePosition);
		previousPosition = transform.parent.position;
	}
	
	void OnMouseDown ()
	{
		group.SetSortingOrder (fragmentManager.MaxSortingOrder + 1);
		fragmentManager.CompressSortingOrderRange ();
	}

	void OnMouseDrag ()
	{
		Vector3 offset = mainCamera.ScreenToWorldPoint (Input.mousePosition) - previousMousePosition;
		transform.parent.position = previousPosition + offset;
	}

	void OnMouseUpAsButton ()
	{
		fragmentManager.CheckAndMergeGroup (group);
	}
}
