using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	
	void Start () {
        PlayerPrefs.SetInt("Episode", 0);	
	}
	
	
	void Update () {
	
        if(Input.GetKey("space"))
        {
            SceneManager.LoadScene(1);
        }
	}
}
