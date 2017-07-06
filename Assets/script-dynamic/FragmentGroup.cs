using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FragmentGroup: IComparer<FragmentGroup>,System.IComparable
{
	#region IComparable implementation

	int System.IComparable.CompareTo (object obj)
	{
		FragmentGroup other = obj as FragmentGroup;
		return this.sortingOrder - other.sortingOrder;
	}

	#endregion

	#region IComparer implementation
	int IComparer<FragmentGroup>.Compare (FragmentGroup x, FragmentGroup y)
	{
		return x.sortingOrder - y.sortingOrder;
	}
	#endregion

	public int sortingOrder;
	public GameObject group;
	public List<Fragment> fragments;
    private FragmentManager fragmentManager;

	public FragmentGroup (FragmentManager fragmentManager)
	{
        this.fragmentManager = fragmentManager;
		group = new GameObject ("Group");
		group.transform.position = Vector3.zero;
        group.transform.parent = fragmentManager.fragmentsRoot;
		sortingOrder = 0;
		fragments = new List<Fragment> ();
	}

	public void AddMember (Fragment fragment)
	{
		fragments.Add (fragment);
		fragment.transform.SetParent (group.transform);
		fragment.sortingOrder = sortingOrder;
		fragment.group = this;
	}

	public void Merge (FragmentGroup other)
	{
		foreach (Fragment fragment in other.fragments) {
			AddMember (fragment);
		}
		GameObject.Destroy (other.group);
	}

	public void SetSortingOrder (int order)
	{
		sortingOrder = order;
		foreach (Fragment fragment in fragments) {
			fragment.sortingOrder = order;
		}
	}

	public void Align ()
	{
		Fragment standard = fragments [0];
		for (int i = 1; i<fragments.Count; i++) {
			Fragment fragment = fragments [i];
			Vector3 offset = new Vector3 (fragment.column - standard.column, -(fragment.row - standard.row) * fragmentManager.pixelsPerUnitY / fragmentManager.pixelsPerUnitX, 0f);
			fragment.transform.position = standard.transform.position + offset;
		}
	}
}
