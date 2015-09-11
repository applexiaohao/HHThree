

using System;
using UnityEngine;
using System.Collections.Generic;

public class HHGameCell:UILabel
{
	/// <summary>
	/// Set cell's rect
	/// </summary>
	/// <param name="rect">cell's position and size</param>
	public void SetCellRect(Vector4 rect)
	{
		this.SetRect(0f,0f,rect.z,rect.w);

		this.transform.localPosition = new Vector3(rect.x,rect.y,0f);
	}

	/// <summary>
	/// Changes the state.
	/// </summary>
	public void ChangeState()
	{
		this.text = this.text.Equals("1") ? "0":"1";
	}

	#region four cell

	public bool IsValid(int index)
	{
		return index >= 0 || index < CellSize * CellSize;
	}

	public bool IsSameRow(HHGameCell sender)
	{
		if (sender == null) {
			return false;
		}
		return this.transform.localPosition.y == sender.transform.localPosition.y;
	}
	/**
	 * 
	 *  0.0 0.1 2 
	 *  3 4 5 
	 *  6 7 8
	 */ 
	public HHGameCell Up()
	{
		int index = this.Index - CellSize;

		if (!IsValid(index)) 
		{
			return null;
		}

		return CellList.Find((HHGameCell sender) =>{
			return sender.Index == index;
		});
	}
	public HHGameCell Left()
	{
		int index = this.Index - 1;
		
		if (!IsValid(index)) 
		{
			return null;
		}
		
		HHGameCell left = CellList.Find((HHGameCell sender) =>{
			return sender.Index == index;
		});

		if (!IsSameRow(left)) {
			return null;
		}

		return left;
	}
	public HHGameCell Bottom()
	{
		int index = this.Index + CellSize;
		
		if (!IsValid(index)) 
		{
			return null;
		}
		
		return CellList.Find((HHGameCell sender) =>{
			return sender.Index == index;
		});
	}
	public HHGameCell Right()
	{
		int index = this.Index + 1;
		
		if (!IsValid(index)) 
		{
			return null;
		}
		
		HHGameCell right = CellList.Find((HHGameCell sender) =>{
			return sender.Index == index;
		});
		
		if (!IsSameRow(right)) {
			return null;
		}
		
		return right;
	}

	#endregion

	public int Index{set;get;}

	public static int CellSize{set;get;}
	public static List<HHGameCell> CellList{set;get;}


	private void InitCell()
	{
		this.text = "1";

		UIFont font = NGUITools.AddMissingComponent<UIFont>(this.gameObject);

		font.dynamicFont = (Font)Resources.Load<Font>("Vera");

		this.bitmapFont = font;

		NGUITools.AddMissingComponent<BoxCollider>(this.gameObject);

		this.autoResizeBoxCollider = true;

		this.ResizeCollider();

		UIButton button = NGUITools.AddMissingComponent<UIButton>(this.gameObject);

		button.onClick.Add(new EventDelegate(()=>{

			HHGameCell top 		= this.Up();
			HHGameCell left 	= this.Left();
			HHGameCell bottom 	= this.Bottom();
			HHGameCell right 	= this.Right();

			if (top != null) {
				top.ChangeState();
			}
			if (left != null) {
				left.ChangeState();
			}
			if (bottom != null) {
				bottom.ChangeState();
			}
			if (right != null) {
				right.ChangeState();
			}

			this.ChangeState();
		}));
	}

	void Start()
	{
		this.InitCell();
		base.Start();
	}
}


