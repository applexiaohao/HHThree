

using System;
using UnityEngine;

public class HHObjectItem:UILabel
{
	public static HHObjectItem Create(GameObject parent)
	{
		return NGUITools.AddChild<HHObjectItem>(parent);
	}


	void Start()
	{
		this.Init();

		base.Start();
	}

	public HHObjectItemContainer Container{
		get{ 
			return this.transform.parent.GetComponent<HHObjectItemContainer>();
		}
		set{
			Vector3 t_v = value.transform.localPosition;
			Vector3 s_v = this.transform.localPosition;
			Vector3 v = t_v - s_v;

			/*
			TweenPosition.Begin(this.gameObject,0.5f, v,false).onFinished.Add(new EventDelegate(()=>{
				this.transform.parent = value.transform;
				this.ResetCenter();
			}));
			*/
			this.transform.parent = value.transform;
			this.ResetCenter();
		}
	}

	void Init()
	{
		this.depth =100;

		UIFont font = NGUITools.AddMissingComponent<UIFont>(this.gameObject);
		font.dynamicFont = Resources.Load<Font>("Vera");
		this.bitmapFont = font;

		this.Type = UnityEngine.Random.Range(0,4);
		this.text = this.Type.ToString();

		NGUITools.AddMissingComponent<BoxCollider>(this.gameObject);
		this.autoResizeBoxCollider = true;
		this.ResizeCollider();

		NGUITools.AddMissingComponent<HHDragDropItem>(this.gameObject);

		HHObjectItemContainer con = this.transform.parent.GetComponent<HHObjectItemContainer>();
		this.SetRect(0,0,con.localSize.x,con.localSize.y);

		this.ResetCenter();
	}

	public void ResetCenter()
	{
		this.transform.localPosition = new Vector3(0f,0f,0f);
	}

	public int Type{set;get;}
}

