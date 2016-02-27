using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FragmentManager : MonoBehaviour
{
	public Sprite source;
	public GameObject fragmentPrefab;
	public float minX = -3f;
	public float maxX = 3f;
	public float minY = 2f;
	public float maxY = -2f;
	public int rows = 4;
	public int columns = 4;
	public float lengthEpsilon = 0.3f;
	public float angleEpsilon = 20f;
	[HideInInspector]
	public List<FragmentGroup>
		groups;
	Fragment[] fragments;
	GameManager gameManager;

	void Start ()
	{
		groups = new List<FragmentGroup> ();
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		SetupFragments ();
	}

	void SetupFragments ()
	{
		Texture2D texture = source.texture;
		fragments = new Fragment[rows * columns];
		int pixelsPerUnit = texture.width / columns;
		for (int i =0; i<fragments.Length; i++) {
			float x = Random.Range (minX, maxX);
			float y = Random.Range (minY, maxY);
			FragmentGroup group = new FragmentGroup ();
			GameObject instance = Instantiate (fragmentPrefab, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
			Fragment fragment = instance.GetComponent<Fragment> ();
			group.AddMember (fragment);
			fragment.index = i;
			fragment.row = i / columns;
			fragment.column = i % columns;
			fragment.group = group;
			fragment.spriteRenderer.sprite = Sprite.Create (texture, 
			                                                new Rect (fragment.column * pixelsPerUnit, (rows - fragment.row - 1) * pixelsPerUnit, pixelsPerUnit, pixelsPerUnit),
			                                                new Vector2 (0.5f, 0.5f),
			                                                pixelsPerUnit);
			fragment.fragmentManager = this;
			fragments [i] = fragment;
			group.SetSortingOrder (i);
			groups.Add (group);
		}
	}

	public int MaxSortingOrder {
		get {
			int max = int.MinValue;
			for (int i =0; i<groups.Count; i++) {
				FragmentGroup group = groups [i];
				if (group.sortingOrder > max) {
					max = group.sortingOrder;
				}
			}
			return max;
		}
	}
	/// <summary>
	/// Compresses the sorting order range.
	/// </summary>
	public void CompressSortingOrderRange ()
	{
		groups.Sort ();
		for (int i=0; i<groups.Count; i++) {
			groups [i].SetSortingOrder (i);
		}
	}

	int CalculateIndex (int row, int col)
	{
		if (row < 0 || row >= rows) {
			return -1;
		}
		if (col < 0 || col >= columns) {
			return -1;
		}
		return row * columns + col;
	}

	public void CheckAndMergeGroup (FragmentGroup group)
	{
		for (int row =0; row < rows; row++) {
			for (int col = 0; col < columns; col++) {
				int currentIndex = CalculateIndex (row, col);
				int rightIndex = CalculateIndex (row, col + 1);
				Fragment current = fragments [currentIndex];
				if (rightIndex != -1) {
					Fragment right = fragments [rightIndex];
					if (current.group != right.group && (current.group == group || right.group == group)) {
						Vector3 offset = right.transform.position - current.transform.position;
						if (CheckDistanceAndAngle (offset, Vector3.right)) {
							if (group == right.group) {
								groups.Remove (current.group);
								group.Merge (current.group);
							} else {
								groups.Remove (right.group);
								group.Merge (right.group);
							}
							group.Align ();
							gameManager.PlayMergeSound ();
							return;
						}
					}
				}
				int downIndex = CalculateIndex (row + 1, col);
				if (downIndex != -1) {
					Fragment down = fragments [downIndex];
					if (current.group != down.group && (current.group == group || down.group == group)) {
						Vector3 offset = down.transform.position - current.transform.position;
						if (CheckDistanceAndAngle (offset, Vector3.down)) {
							if (group == down.group) {
								groups.Remove (current.group);
								group.Merge (current.group);
							} else {
								groups.Remove (down.group);
								group.Merge (down.group);
							}
							group.Align ();
							gameManager.PlayMergeSound ();
							return;
						}
					}
				}
			}
		}
	}

	bool CheckDistanceAndAngle (Vector3 offset, Vector3 standard)
	{
		return (Mathf.Abs (offset.sqrMagnitude - 1f) <= lengthEpsilon)
			&& (Mathf.Abs (Vector3.Angle (standard, offset)) <= angleEpsilon);
	}

}
