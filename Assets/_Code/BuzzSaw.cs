using UnityEngine;
using System.Collections;

public class BuzzSaw : MonoBehaviour
{

    public int Damage;

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "player")
        {
           
            GameManager.EnterTrap(-Damage);
        }
    }
}
