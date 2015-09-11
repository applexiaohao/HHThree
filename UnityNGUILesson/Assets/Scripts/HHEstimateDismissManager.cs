
using System;
using System.Collections.Generic;

public class HHEstimateDismissManager
{
	public static List<HHObjectItem> RemoveList = new List<HHObjectItem>();

	public static void AddItem(HHObjectItemContainer sender)
	{
		HHObjectItem item = sender.transform.GetChild(0).GetComponent<HHObjectItem>();

		RemoveList.Add(item);
	}

	public delegate void RefreshDelegate(bool result);
	public static void Refresh(RefreshDelegate block)
	{
		block(RemoveList.Count == 0);

		RemoveList.ForEach((HHObjectItem item)=>{

			item.Container.TopTopDown();
			NGUITools.Destroy(item.gameObject);

		});

		RemoveList.Clear();
	}
}


