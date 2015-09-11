using UnityEngine;
using System.Collections;

public class TAppDelegate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		UIPanel window = NGUITools.CreateUI(false);
		HHTGameLayoutPanel.GetGamePanel(5,window.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
