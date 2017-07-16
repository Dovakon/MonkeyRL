
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

    public Text stateUI;

    private bool canJump;
    private Rigidbody2D rigibody;

    private float startTime;
    
    private int countAction;
    string action;

    ExportData  export;

    void Start () {

        rigibody = GetComponent<Rigidbody2D>();
        export = new ExportData();
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
        //yield return new WaitForSeconds(1f);
        //startTime = Time.time;


        //while (Time.time - startTime < 5)
        //{
            
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
        
            


            

        //    export.SaveAction(countAction, Time.time - startTime, state, action, -1);

        //    stateUI.text = state.ToString();
        //    //

        //    countAction++;


        //}

        //export.WriteText();
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
            state = (int)(possX) + 20;
        }
        else if (possY < 6)
        {
            state = (int)(possX) + 40;
        }
        //print(transform.position+" -  " + state);
        //else if (possY < 4)
            //{
            //    state = (int)possX / 5 + 120;
            //}
            return state;
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
