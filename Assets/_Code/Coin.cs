using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    public int coinScore;

    void Start () {
		
	}


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "player")
        {
            GameManager.CoinAdded(coinScore);
            Destroy(gameObject);
        }
    }

}
