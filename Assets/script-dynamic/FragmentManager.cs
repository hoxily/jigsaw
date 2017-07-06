using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigObjects;

public class FragmentManager
{
	Sprite source;
    GameObject fragmentPrefab;
	float minX = -3f;
	float maxX = 3f;
	float minY = 2f;
	float maxY = -2f;
	int rows;
	int columns;
	float lengthEpsilon = 0.12f;
	float angleEpsilon = 10f;
	List<FragmentGroup> groups = new List<FragmentGroup>();
	Fragment[] fragments;
    public Transform fragmentsRoot;
    public float pixelsPerUnitX;
    public float pixelsPerUnitY;

    public FragmentManager()
	{
		fragmentPrefab = EntryPoint.instance.loadAsset<GameObject>("uiprefabs", "Fragment.prefab") as GameObject;
        fragmentsRoot = new GameObject("FragmentsRoot").transform;
	}

    public void CleanUp()
    {
        foreach (Fragment f in fragments)
        {
            f.fragmentManager = null;
            f.group = null;
        }
        fragments = null;
        source = null;
        foreach (FragmentGroup group in groups)
        {
            Object.Destroy(group.group);
        }
        groups.Clear();
    }

	public void SetupFragments (LevelConfig levelConfig)
	{
        rows = levelConfig.rows;
        columns = levelConfig.columns;
        source = EntryPoint.instance.loadAsset<Sprite>("sprites", levelConfig.id + ".jpg") as Sprite;
		Texture2D texture = source.texture;
		fragments = new Fragment[rows * columns];
        // 切成 1 : heightUnit 的矩形
		pixelsPerUnitX = (float)texture.width / (float)columns;
        pixelsPerUnitY = (float)texture.height / (float)rows;
        float height = pixelsPerUnitY / pixelsPerUnitX;
		for (int i =0; i<fragments.Length; i++) {
			float x = Random.Range (minX, maxX);
			float y = Random.Range (minY, maxY);
			FragmentGroup group = new FragmentGroup (this);
			GameObject instance = Object.Instantiate (fragmentPrefab, new Vector3 (x, y, 0f), Quaternion.identity, fragmentsRoot) as GameObject;
			Fragment fragment = instance.GetComponent<Fragment> ();
            if (fragment == null)
            {
                fragment = instance.AddComponent<Fragment>();
            }
			group.AddMember (fragment);
			fragment.index = i;
			fragment.row = i / columns;
			fragment.column = i % columns;
			fragment.group = group;
            Rect rect = new Rect(fragment.column * pixelsPerUnitX, (rows - fragment.row - 1) * pixelsPerUnitY, pixelsPerUnitX, pixelsPerUnitY);
            fragment.spriteRenderer.sprite = Sprite.Create(texture,
                                                            rect,
                                                            new Vector2(0.5f, 0.5f),
                                                            pixelsPerUnitX);
            fragment.GetComponent<BoxCollider2D>().size = new Vector3(1.0f, height, 1.0f);
            fragment.fragmentManager = this;
			fragments [i] = fragment;
			group.SetSortingOrder (i);
			groups.Add (group);
		}
        CompressSortingOrderRange();
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
        // 设置z值，以使得点击击中的目标符合视觉。
        for (int i = 0; i < groups.Count; i++)
        {
            Vector3 pos = groups[i].group.transform.position;
            groups[i].group.transform.position = new Vector3(pos.x, pos.y, groups.Count - i);
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

    private void CheckWin()
    {
        if (groups.Count  == 1)
        {
            SoundManager.instance.PlayWin();
        }
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
						if (CheckDistanceAndAngle (offset, 1.0f, Vector2.right)) {
							if (group == right.group) {
								groups.Remove (current.group);
								group.Merge (current.group);
							} else {
								groups.Remove (right.group);
								group.Merge (right.group);
							}
							group.Align ();
							SoundManager.instance.PlayMerge();
                            CheckWin();
							return;
						}
					}
				}
				int downIndex = CalculateIndex (row + 1, col);
				if (downIndex != -1) {
					Fragment down = fragments [downIndex];
					if (current.group != down.group && (current.group == group || down.group == group)) {
						Vector3 offset = down.transform.position - current.transform.position;
						if (CheckDistanceAndAngle (offset, pixelsPerUnitY / pixelsPerUnitX, Vector2.down)) {
							if (group == down.group) {
								groups.Remove (current.group);
								group.Merge (current.group);
							} else {
								groups.Remove (down.group);
								group.Merge (down.group);
							}
							group.Align ();
                            SoundManager.instance.PlayMerge();
                            CheckWin();
							return;
						}
					}
				}
			}
		}
    }

    bool CheckDistanceAndAngle (Vector2 offset, float distance, Vector2 standard)
	{
		return (Mathf.Abs (offset.magnitude - distance) <= lengthEpsilon)
			&& (Mathf.Abs (Vector2.Angle (standard, offset)) <= angleEpsilon);
	}

}
