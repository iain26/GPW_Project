using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	//Load scenes
	public void LoadCharacterCreation() 
	{
		Application.LoadLevel ("CharacterCreation");
	}
	public void LoadDraw_Phase()
	{
		Application.LoadLevel ("Draw_Phase");
	}
	public void LoadShop()
	{
		Application.LoadLevel ("Shop");
	}
	public void LoadSettings()
	{
		Application.LoadLevel ("Settings");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
