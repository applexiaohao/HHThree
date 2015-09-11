
using System;
using UnityEngine;
using System.Collections.Generic;


public class HHObjectItemContainer:UISprite
{

	/// <summary>
	/// Create the specified parent.
	/// </summary>
	/// <param name="parent">Parent.</param>
	public static HHObjectItemContainer Create(GameObject parent)
	{
		HHObjectItemContainer con = NGUITools.AddChild<HHObjectItemContainer>(parent);

		con.Birth();

		return con;
	}

	void Start()
	{
		this.Init();
		base.Start();
	}

	public void Init()
	{
		UIAtlas atlas = Resources.Load("Atlas/Wooden Atlas",typeof(UIAtlas)) as UIAtlas;
		this.atlas = atlas;
		this.spriteName = "Highlight - Thin";
		this.type = Type.Sliced;

		NGUITools.AddMissingComponent<BoxCollider>(this.gameObject);

		this.autoResizeBoxCollider = true;
		this.ResizeCollider();


		UIDragDropContainer container_scirpt = NGUITools.AddMissingComponent<UIDragDropContainer>(this.gameObject);
		container_scirpt.reparentTarget = this.transform;
	}

		
	public int Index{set;get;}

	/// <summary>
	/// Sets the container rect.
	/// </summary>
	/// <param name="rect">Rect.</param>
	public void SetContainerRect(Vector4 rect)
	{
		this.SetRect(0,0,rect.z,rect.w);
		this.transform.localPosition  = new Vector3(rect.x,rect.y,0.0f);

	}

	#region Region
	public bool IsNearBy(HHObjectItemContainer sender)
	{
		return this.Left == sender || this.Right == sender || this.Top == sender || this.Bottom == sender;
	}

	public bool IsSameRow(HHObjectItemContainer sender)
	{
		return this.transform.localPosition.y == sender.transform.localPosition.y;
	}

	public static int Size;
	public bool IsValid(int index)
	{
		return index >=0 && index < Size * Size;
	}

	public static List<HHObjectItemContainer> ContainerList;
	public HHObjectItemContainer Top{
		get{
			int index = this.Index - Size;

			if (!IsValid(index)) {
				return null;
			}

			return ContainerList.Find((HHObjectItemContainer sender)=>{
				return sender.Index == index;
			});
		}
	}
	public HHObjectItemContainer Bottom{
		get{
			int index = this.Index + Size;
			
			if (!IsValid(index)) {
				return null;
			}
			
			return ContainerList.Find((HHObjectItemContainer sender)=>{
				return sender.Index == index;
			});
		}
	}
	public HHObjectItemContainer Left{
		get{
			int index = this.Index - 1;
			
			if (!IsValid(index)) {
				return null;
			}
			
			HHObjectItemContainer c = ContainerList.Find((HHObjectItemContainer sender)=>{
				return sender.Index == index;
			});

			if (!IsSameRow(c)) {
				return null;
			}

			return c;
		}
	}
	public HHObjectItemContainer Right{
		get{
			int index = this.Index + 1;
			
			if (!IsValid(index)) {
				return null;
			}
			
			HHObjectItemContainer c = ContainerList.Find((HHObjectItemContainer sender)=>{
				return sender.Index == index;
			});
			
			if (!IsSameRow(c)) {
				return null;
			}
			
			return c;
		}
	}

	private void Birth()
	{
		HHObjectItem item = HHObjectItem.Create(this.gameObject);
		item.Container = this;
	}

	public HHObjectItem Child{
		get{
			if (this.transform.childCount != 1) {
				return null;
			}

			return this.transform.GetChild(0).GetComponent<HHObjectItem>();
		}
	}

	public void TopTopDown()
	{
		HHObjectItemContainer temp = this.Top;
		while (temp != null) 
		{
			if (temp.Child != null) 
			{
				NGUIDebug.Log(temp.name + " " + temp.Child.text);

				temp.Child.Container = temp.Bottom;

			}
			temp = temp.Top;
		}
		temp = this.Top;
		while(temp != null)
		{
			if (temp.Child == null) 
			{
				temp.Birth();				
			}
			temp = temp.Top;
		}
	}

	#endregion

	#region Dismiss
	
	public bool Compare(HHObjectItemContainer sender)
	{
		if (this.Child == null || sender.Child == null) {
			return false;
		}
		return this.Child.Type == sender.Child.Type;
	}

	public bool CanBeLeader(int direction)
	{
		bool result = false;

		switch (direction) {
		case 0://Horizontal
		{
			HHObjectItemContainer mid = this.Right;
			if (mid == null) {
				return false;
			}
			HHObjectItemContainer suf = mid.Right;

			if (mid != null && suf != null) 
			{
				result = this.Compare(mid) && this.Compare(suf);
			}

			break;
		}
		case 1://Vertical
		{
			HHObjectItemContainer mid = this.Bottom;

			if (mid == null) {
				return false;
			}

			HHObjectItemContainer suf = mid.Bottom;
			
			if (mid != null && suf != null) 
			{
				result = this.Compare(mid) && this.Compare(suf);
			}

			break;
		}
		}

		return result;
	}
	public bool CanBeSuffer(int direction)
	{
		bool result = false;
		
		switch (direction) {
		case 0://Horizontal
		{
			HHObjectItemContainer mid = this.Left;

			if (mid == null) {
				return false;
			}

			HHObjectItemContainer lea = mid.Left;
			
			if (mid != null && lea != null) 
			{
				result = this.Compare(mid) && this.Compare(lea);
			}
			
			break;
		}
		case 1://Vertical
		{
			HHObjectItemContainer mid = this.Top;

			if (mid == null) {
				return false;
			}

			HHObjectItemContainer lea = mid.Top;
			
			if (mid != null && lea != null) 
			{
				result = this.Compare(mid) && this.Compare(lea);
			}
			
			break;
		}
		}
		
		return result;
	}
	public bool CanBeMiddle(int direction)
	{
		bool result = false;
		
		switch (direction) {
		case 0://Horizontal
		{
			HHObjectItemContainer suf = this.Right;
			HHObjectItemContainer lea = this.Left;
			
			if (suf != null && lea != null) 
			{
				result = this.Compare(suf) && this.Compare(lea);
			}
			
			break;
		}
		case 1://Vertical
		{
			HHObjectItemContainer suf = this.Bottom;
			HHObjectItemContainer lea = this.Top;
			
			if (suf != null && lea != null) 
			{
				result = this.Compare(suf) && this.Compare(lea);
			}
			
			break;
		}
		}
		
		return result;
	}

	public void Search(int direction,int flag,HHObjectItemContainer sender)
	{
		if (sender == null) {
			return;
		}
		//horizontal left
		if (direction == 0 && flag == 0) 
		{
			HHObjectItemContainer item = sender.Left;

			if (item != null && sender.Compare(item)) 
			{
				HHEstimateDismissManager.AddItem(item);

				Search(direction,flag,item);
			}
		}

		//horizontal right
		if (direction == 0 && flag == 1) 
		{
			HHObjectItemContainer item = sender.Right;
			
			if (item != null && sender.Compare(item)) 
			{
				HHEstimateDismissManager.AddItem(item);
				Search(direction,flag,item);
			}
		}

		//vertical top
		if (direction == 1 && flag == 0) 
		{
			HHObjectItemContainer item = sender.Top;
			
			if (item != null && sender.Compare(item)) 
			{
				HHEstimateDismissManager.AddItem(item);
				Search(direction,flag,item);
			}
		}

		//vertical bottom
		if (direction == 1 && flag == 1) 
		{
			HHObjectItemContainer item = sender.Bottom;
			
			if (item != null && sender.Compare(item)) 
			{
				HHEstimateDismissManager.AddItem(item);
				Search(direction,flag,item);
			}
		}
	}
	public void ScanHorizontalDirection()
	{
		if (CanBeLeader(0)) 
		{
			HHEstimateDismissManager.AddItem(this);
			HHEstimateDismissManager.AddItem(this.Right);
			HHEstimateDismissManager.AddItem(this.Right.Right);

			Search(0,0,this);
			Search(0,1,this.Right.Right);
			return;
		}

		if (CanBeMiddle(0)) 
		{
			HHEstimateDismissManager.AddItem(this);
			HHEstimateDismissManager.AddItem(this.Left);
			HHEstimateDismissManager.AddItem(this.Right);

			Search(0,0,this.Left);
			Search(0,1,this.Right);
			return;
		}

		if (CanBeSuffer(0)) {
			HHEstimateDismissManager.AddItem(this.Left.Left);
			HHEstimateDismissManager.AddItem(this.Left);
			HHEstimateDismissManager.AddItem(this);

			Search(0,0,this.Left.Left);
			Search(0,1,this);
			return;
		}
	}
	public void ScanVerticalDirection()
	{
		if (CanBeLeader(1)) 
		{
			HHEstimateDismissManager.AddItem(this.Bottom.Bottom);
			HHEstimateDismissManager.AddItem(this.Bottom);
			HHEstimateDismissManager.AddItem(this);

			Search(1,0,this);
			Search(1,1,this.Bottom.Bottom);
			return;
		}
		
		if (CanBeMiddle(1)) 
		{
			HHEstimateDismissManager.AddItem(this.Bottom);
			HHEstimateDismissManager.AddItem(this);
			HHEstimateDismissManager.AddItem(this.Top);

			Search(1,0,this.Top);
			Search(1,1,this.Bottom);
			return;
		}
		
		if (CanBeSuffer(1)) {
			HHEstimateDismissManager.AddItem(this.Top.Top);
			HHEstimateDismissManager.AddItem(this);
			HHEstimateDismissManager.AddItem(this.Top);

			Search(1,0,this.Top.Top);
			Search(1,1,this);
			return;
		}
	}
	#endregion
}


