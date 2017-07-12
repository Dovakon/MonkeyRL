using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExportData : MonoBehaviour {

    public List<data> state = new List<data>();

  
    void Start ()
    {
        
        
    }
	


    public void WriteText()
    {
        print("test");
        string filePath = Application.dataPath + "/data.txt";

        if (!File.Exists(filePath))
        {
            using (StreamWriter sw = File.CreateText(filePath)) ;
        }

        StreamWriter writer;

        //if (PlayerPrefs.GetInt("Episode") == 0)
        //{
        //    writer = new StreamWriter(filePath);
        //    writer.BaseStream.Seek(0, SeekOrigin.End);
        
        //}

        writer = new StreamWriter(filePath, append: false);


        //writer.WriteLine("Episode: " + PlayerPrefs.GetInt("Episode") + " Final Score: " + score);


        for (int i = 0; i < state.Count; i++)
        {

            writer.WriteLine(state[i].value + " " + state[i].time + " " + state[i].state + " " + state[i].action + " " + state[i].reward);

        }

        //PlayerPrefs.SetInt("Episode", PlayerPrefs.GetInt("Episode") + 1);
        writer.WriteLine("");
        writer.Flush();
        writer.Close();

    }

    public void SaveAction(int val, float t, int st, string act, int r)
    {
        state.Add(new data(val, t, st, act, r));

    }

}


[Serializable]
public class data
{
    public int value;
    public double time;
    public int state;
    public string action;
    public int reward;


    public data(int val, float t, int st, string act, int r)
    {
        value = val;
        time = Math.Round(t, 2);
        state = st;
        action = act;
        reward = r;
    }
}