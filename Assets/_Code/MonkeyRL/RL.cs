using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RL : MonoBehaviour {

    public SARSA sarsa;

	
	void Start () {
		
	}
	
	
	void Update () {

        if (Input.GetKeyDown(KeyCode.L))
        {
            sarsa.StartLearning();
        }
        if (Input.GetButton("Cancel"))
        {
            Application.Quit();
        }

    }
}
