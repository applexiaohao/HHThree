

using System;
using System.Collections.Generic;
using UnityEngine;

public class HHTGameLayoutPanel
{
	private HHTGameLayoutPanel ()
	{
	}


	public static float GameSizeWidth = 400.0f;
	public static UIPanel GetGamePanel(int size,GameObject parent)
	{
		UIPanel panel = NGUITools.AddChild<UIPanel>(parent);

		//get single cell's size
		float cell_size = GameSizeWidth / size;

		//get min x
		float min_x = 0f - GameSizeWidth / 2.0f + cell_size / 2.0f;

		//get max y
		float max_y = GameSizeWidth / 2.0f - cell_size / 2.0f;

		HHObjectItemContainer.ContainerList = new List<HHObjectItemContainer>();
		HHObjectItemContainer.Size = size;

		for (int i = 0; i < size; i++) 
		{
			for (int j = 0; j < size; j++) 
			{
				int index = i * size + j;

				float x = min_x + j % size * cell_size;

				float y = max_y - i * cell_size;

				Vector4 rect = new Vector4(x,y,cell_size,cell_size);

				HHObjectItemContainer item = HHObjectItemContainer.Create(panel.gameObject);
				item.gameObject.name = "Container : " + index.ToString();
				item.Index = index;

				item.SetContainerRect(rect);

				HHObjectItemContainer.ContainerList.Add(item);
			}
		}

		return panel;
	}
}


