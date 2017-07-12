using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    
    public int Speed;
    public int jumpVelocity;
    public int fallMultiplier;
    public static bool Jumped;


    private float translation;
    private bool canJump = true;
    private Animator animator;
    private Rigidbody2D rigibody;
    void Start () {

        animator = GetComponent<Animator>();
        rigibody = GetComponent<Rigidbody2D>();

    }

    void Update()
    {


        if (Input.GetButton("Horizontal"))
        {
            translation = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;

            if (translation < 0)
            {
                //animator.SetBool("Idle_01", false);
                //animator.SetBool("Run_02", true);
                transform.localScale = new Vector2(-1, 1);
            }
            else if (translation > 0)
            {
                transform.localScale = new Vector2(1, 1);
                //animator.SetBool("Run_02", true);
                //animator.SetBool("Idle_01", false);
            }

            transform.position += new Vector3(translation, 0, 0);
        }
        else
        {
            //animator.SetBool("Run_02", false);
            //animator.SetBool("Idle_01", true);
        }


        if (Input.GetButton("Jump") && canJump)
        {
            rigibody.velocity = Vector2.up * jumpVelocity;

            //animator.SetBool("Run_02", false);
            //animator.SetBool("Idle_01", false);
            //animator.SetBool("Jump_01", true);
            canJump = false;
            Jumped = true;

        }

        if (rigibody.velocity.y < 0)
        {
            rigibody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }


        if (Input.GetButton("Cancel"))
        {
            PlayerPrefs.SetInt("Episode", 0);
            Application.Quit();
        }

       
    }

    public void GetHit()
    {
        animator.Play("Damage");
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground")
        {
            canJump = true;
        }
    }

}
