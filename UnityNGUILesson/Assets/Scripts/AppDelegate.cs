using UnityEngine;
using System.Collections;

public class AppDelegate : MonoBehaviour {


	//root element
	private UIPanel window;
	
	void Start () 
	{
		//create root window
		this.window = NGUITools.CreateUI(false);

		HHGameLayoutPanel.GetGamePanel(5,this.window.gameObject);
	}

}
