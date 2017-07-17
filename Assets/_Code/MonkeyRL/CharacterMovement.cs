
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour {

    enum Movement {N, L, R, U};

    Movement chosenMovement;

    public int Speed;
    public int jumpVelocity;
    public int fallMultiplier;
    
    private bool canJump;
    private Rigidbody2D rigibody;
    

    void Start () {

        rigibody = GetComponent<Rigidbody2D>();
        chosenMovement = Movement.N;
    }


    void Update()
    {
        if (chosenMovement == Movement.N)
        {
            //Do Nothing
        }
        if (chosenMovement == Movement.L)
        {
            transform.Translate(Vector2.left * Speed * Time.deltaTime);
        }
        if (chosenMovement == Movement.R)
        {
            transform.Translate(Vector2.right * Speed * Time.deltaTime);
        }
        if (chosenMovement == Movement.U)
        {
            if (canJump)
            {  
                rigibody.velocity = Vector2.up * jumpVelocity;
                canJump = false;
            }
            
        }
        
        if (rigibody.velocity.y < 0)
        {
            rigibody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }



    }

    public void Move(string action)
    {
        //N ----> Nothing
        //L ----> Left
        //R ----> Right
        //UL ----> Up & Left
        //UR ----> Up & Right
        //U ----> Up
            
        
        if (action == "L")
        {
            chosenMovement = Movement.L;
            //action = "L";
        }
        if (action == "R")
        {
            chosenMovement = Movement.R;
            //action = "R";
        }
        if (action == "U")
        {
            chosenMovement = Movement.U;
            //action = "U";
        }
        
    }

    public int CurrentState()
    {

        //find state

        float possX = transform.position.x;
        float possY = transform.position.y;

        int state = 0;

        if (possY < 2)
        {
            state = (int)possX;
        }
        else if (possY < 4)
        {
            state = (int)(possX) + 40;
        }
        else if (possY < 6)
        {
            state = (int)(possX) + 80;
        }

        if (state < 0)
        {
            Debug.LogError("Vgike Eksw Apo To StateSpace");
            return 0;
        }
        else
        {
            return state;
        }
        
    }

    public void ResetPoss()
    {
        transform.position = new Vector2(.1f, 0);
    }

    public void StopMoving()
    {
        chosenMovement = Movement.N;
    }

    private void OnCollisionEnter2D(Collision2D coll)
{
    if (coll.gameObject.tag == "Ground")
    {
        canJump = true;
    }
}
}
