//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using UnityEngine;

//public class HandleData : MonoBehaviour
//{


//    public Transform player;


//    private List<State> state;
//    private State currentState;

//    string action;

//    void Start()
//    {

//        state = new List<State>();
//        //currentState = new State();
//        StartCoroutine(WriteData());

//    }



//    IEnumerator WriteData()
//    {

//        while (true)
//        {
//            if (PlayerController.Jumped)
//            {
//                action = "j";
//                PlayerController.Jumped = false;
//            }

//            if (Input.GetKey("right"))
//            {
//                action += "r";
//            }
//            else if (Input.GetKey("left"))
//            {
//                action += "l";
//            }

//            if (action == "")
//            {
//                action = "n";
//            }

//            //currentState.position = new Vector2(player.position.x, player.position.y);
//            //currentState.time = Time.time;



//            state.Add(new State(player.position.x, player.position.x, action, Time.time));
//            action = "";

//            yield return new WaitForSeconds(0.1f);
//        }
//    }


//    public void WriteText(int score)
//    {
//        string filePath = Application.dataPath + "/data.txt";

//        if (!File.Exists(filePath))
//        {
//            using (StreamWriter sw = File.CreateText(filePath)) ;
//        }

//        StreamWriter writer;

//        if (PlayerPrefs.GetInt("Episode") == 0)
//        {
//            writer = new StreamWriter(filePath);
//            //writer.BaseStream.Seek(0, SeekOrigin.End);



//        }
//        else
//        {
//            writer = new StreamWriter(filePath, append: true);
//        }

//        writer.WriteLine("Episode: " + PlayerPrefs.GetInt("Episode") + " Final Score: " + score);


//        for (int i = 0; i < state.Count; i++)
//        {

//            writer.WriteLine(state[i].time + " " + state[i].xPosition + " " + state[i].yPosition + " " + state[i].action);

//        }

//        PlayerPrefs.SetInt("Episode", PlayerPrefs.GetInt("Episode") + 1);
//        writer.WriteLine("");
//        writer.Flush();
//        writer.Close();

//    }



//}

//[Serializable]
//public class State
//{
//    public double xPosition;
//    public double yPosition;
//    public string action;
//    public double time;


//    public State(double xPos, double yPos, string act, float t)
//    {

//        xPosition = Math.Round(xPos, 4);
//        yPosition = Math.Round(yPos, 4);
//        time = Math.Round(t, 4);

//        action = act;
//    }
//}