using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusic : MonoBehaviour 
{

	AudioSource audioSource;
	static bool AudioBegin = false;


	// Use this for initialization
	void Start () 
	{

		audioSource = GetComponent<AudioSource> ();
		
	}



	void Update () 
	{
		if (!AudioBegin) 
		{
			audioSource.Play ();
			Debug.Log("Audio is playing");
			DontDestroyOnLoad (gameObject);
			Debug.Log("Audio is not destroyed");
			AudioBegin = true;
		} 
	}
}


