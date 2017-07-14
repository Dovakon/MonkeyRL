﻿
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SARSA : MonoBehaviour
{


    public CharacterMovement character;
    public Text episodeUI;
    public Text currentStateUI;
    public GameObject arrow;

    private List<State> state;
    private List<Action> action;

    //public Policy policy;

    //RL Settings
    int currentState;
    int nextState;



    //lerp settings
    //private float elapsed;
    //public float movingTime;

    //private float startPosX;
    //private float nextPosX;
    

    void Start()
    {
        state = new List<State>();
        action = new List<Action>();

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(RunPolicy());
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SavePolicy();
        }
        if (Input.GetButton("Cancel"))
        {
            PlayerPrefs.SetInt("Episode", 0);
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(Learning());
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            InstantiateArrows();
        }

    }
    
    IEnumerator Learning()
    {

        for (int i = 0; i < 120; i++)
        {
            state.Add(new State());
            state[i].StateValue = i;
            if (i == 39 || i == 79 || i == 119)
            {
                state[i].Reward = 500;
            }
            else if ((i==56 || i==57))
            {
                state[i].Reward = -20;
            }
            else if (i == 16)
            {
                state[i].Reward = -4;
            }
            else if (i == 96)
            {
                state[i].Reward = -2;
            }
            else
            {
                state[i].Reward = -5;
            }


        }
        
        float Alpha = .1f;
        float Gamma = .9f;
        float eGreddy = 5;
        int currentEpisode = 0;
        int currentAction = 0;
        float startTime;
        int Episodes = 100;
        //int AfterEpisodes = 70;
        //int AfterEGreddy = 1;

        float QValue;
        float qValue;

        string firstAction;
        string nextAction;

        yield return new WaitForSeconds(1f);

        //First Action 
        //Move Right
        currentState = 0;
        firstAction = "R";
        character.Move(firstAction);
        startTime = 0;

        action.Add(new Action(currentAction, Time.time - startTime, currentState, firstAction, state[currentState].Reward));

        yield return new WaitForSeconds(.2f);

        episodeUI.text = currentEpisode.ToString();

        while (currentEpisode < Episodes)
        {

            nextState = character.CurrentState();
            currentStateUI.text = nextState.ToString();

            eGreddy = 100 / ((Time.time - startTime) * (currentEpisode + 1));
            //print(eGreddy);

            if (Random.value <= eGreddy)
            {
                print("random");
                nextAction = DoRandomAction();
            }
            else
            {
                nextAction = GetMaxQ(nextState);
                print("Max");
            }

            QValue = state[currentState].Action[firstAction];
            qValue = state[nextState].Action[nextAction];

            QValue = QValue + Alpha * (state[nextState].Reward + (Gamma * qValue) - QValue);

            state[currentState].Action[firstAction] = QValue;


            if (currentState == 39 || currentState == 79 || currentState == 119)
            {
                currentState = 0;
                firstAction = "R";
                character.ResetPoss();
                character.Move(firstAction);
                startTime = Time.time;
                yield return new WaitForSeconds(.2f);

                currentEpisode++;
                episodeUI.text = currentEpisode.ToString();

            }
            else if (currentState == 56 || currentState == 57)
                {
                currentState = 0;
                firstAction = "R";
                character.ResetPoss();
                character.Move(firstAction);
                //startTime = Time.time;
                yield return new WaitForSeconds(.2f);

                
            }
            else
            {
                currentState = nextState;
                firstAction = nextAction;
                character.Move(firstAction);
                yield return new WaitForSeconds(.2f);

            }

            currentAction++;
            action.Add(new Action(currentAction, Time.time - startTime, currentState, firstAction, state[currentState].Reward));

        }

        character.StopMoving();
        WriteQtable();
        WriteVtable();
        WriteActions();
    }

    string DoRandomAction()
    {
        int action = Random.Range(0, 6);

        if (action == 0)
        {
            return "N";
        }
        else if (action == 1)
        {
            return "L";
        }
        else if (action == 2)
        {
            return "R";
        }
        else if (action == 3)
        {
            return "UL";
        }
        else if (action == 4)
        {
            return "UR";
        }
        else if (action == 5)
        {
            return "U";
        }
        return null;
    }

    string GetMaxQ(int st)
    {
        string action = "N";


        if (state[st].Action[action] < state[st].Action["L"])
        {
            action = "L";
        }
        if (state[st].Action[action] < state[st].Action["R"])
        {
            action = "R";
        }
        if (state[st].Action[action] < state[st].Action["UL"])
        {
            action = "UL";
        }
        if (state[st].Action[action] < state[st].Action["UR"])
        {
            action = "UR";
        }
        if (state[st].Action[action] < state[st].Action["U"])
        {
            action = "U";
        }

        return action;
    }


    IEnumerator RunPolicy()
    {
        character.ResetPoss();
        int currentState = 0;
        Policy.Instance.LoadPolicy();

        string nextMove = Policy.Instance.policyState.state[currentState].action;
        character.Move(nextMove);
        yield return new WaitForSeconds(.2f);

        while (true)
        {
            if (character.CurrentState() == 39 || character.CurrentState() == 79 || character.CurrentState() == 119)
            {
                break;
            }
            else
            {
                currentState = character.CurrentState();
            }

            nextMove = Policy.Instance.policyState.state[currentState].action;
            character.Move(nextMove);
            yield return new WaitForSeconds(.2f);
        }
    }

    void SavePolicy()
    {
        for (int i = 0; i < state.Count; i++)
        {
            string MaxAction = GetMaxQ(i);

            PolicyValues pol = new PolicyValues();
            pol.action = MaxAction;
            pol.value = i;
            //Policy.Instance.policyState.state[i].action = MaxAction;
            //Policy.Instance.policyState.state[i].value = i;

            Policy.Instance.policyState.state.Add(pol);
            Policy.Instance.SavePolicy();

            //pol.state = i;
            //pol.action = MaxAction;
            //policy.vValue.Add(pol);
            print(pol.value + "    " + pol.action);

        }
    }

    void InstantiateArrows()
    {
        for (int i = 0; i < state.Count; i++)
        {
            string action = GetMaxQ(i);
            int Zrotation = 0;
            float Xposs,Yposs;

            Xposs = i * .5f;
            Yposs = 0;
            if(Xposs < 20)
            {
                Yposs = 1;
            }
            else if (Xposs < 40)
            {
                Yposs = 3;
                Xposs -= 20; 
            }
            else if (Xposs < 60)
            {
                Yposs = 5;
                Xposs -= 40;
            }

            if (action == "N")
            {
                Zrotation = 270;
            }
            if (action == "L")
            {
                Zrotation = 180;
            }
            else if (action == "R")
            {
                Zrotation = 0;
            }
            else if (action == "UL")
            {
                Zrotation = -135;
            }
            else if (action == "UR")
            {
                Zrotation = 45;
            }
            else if (action == "U")
            {
                Zrotation = 90;
            }
            Instantiate(arrow, new Vector3(Xposs, Yposs, 0), Quaternion.Euler(new Vector3(0, 0, Zrotation)));
            //obj.transform.parent = this.gameObject.transform;
        }
    }

    public void WriteVtable()
    {

        string filePath = Application.dataPath + "/StreamingAssets/Vtable.txt";

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
            string MaxAction = GetMaxQ(i);

            //writer.WriteLine("V: " + state[i].StateValue + "   Nothing: " + state[i].Action["N"] + "  Left: " + state[i].Action["L"] + "   Right: " + state[i].Action["R"]
            //    + "   Up & Left: " + state[i].Action["UL"] + "  Up & Right: " + state[i].Action["UR"] + "   Up: " + state[i].Action["U"]);
            writer.WriteLine("V: " + state[i].StateValue + "   " + state[i].Action[MaxAction] + "   " + MaxAction);

        }

        //PlayerPrefs.SetInt("Episode", PlayerPrefs.GetInt("Episode") + 1);
        writer.WriteLine("");
        writer.Flush();
        writer.Close();

    }

    public void WriteActions()
    {
        
        string filePath = Application.dataPath + "/StreamingAssets/Actions.txt";

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


        for (int i = 0; i < action.Count; i++)
        {

            writer.WriteLine(action[i].value + "  " + action[i].time + "  " + action[i].state + "  " + action[i].action + "  " + action[i].reward);

        }

        //PlayerPrefs.SetInt("Episode", PlayerPrefs.GetInt("Episode") + 1);
        writer.WriteLine("");
        writer.Flush();
        writer.Close();

    }

    public void WriteQtable()
    {

        string filePath = Application.dataPath + "/StreamingAssets/Qtable.txt";

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
            string MaxAction = GetMaxQ(i);

            writer.WriteLine("V: " + state[i].StateValue + "   Nothing: " + state[i].Action["N"] + "  Left: " + state[i].Action["L"] + "   Right: " + state[i].Action["R"]
                + "   Up & Left: " + state[i].Action["UL"] + "  Up & Right: " + state[i].Action["UR"] + "   Up: " + state[i].Action["U"]);
            

        }

        //PlayerPrefs.SetInt("Episode", PlayerPrefs.GetInt("Episode") + 1);
        writer.WriteLine("");
        writer.Flush();
        writer.Close();

    }
}


[System.Serializable]
public class State
{

    public Dictionary<string, float> Action;
    public float Reward;
    public int StateValue;

    public State()
    {
        Action = new Dictionary<string, float>();
        
        Action.Add("N", 0);
        Action.Add("L", 0);
        Action.Add("R", 0);
        Action.Add("UL", 0);
        Action.Add("UR", 0);
        Action.Add("U", 0);
    }
}

[System.Serializable]
public class Action
{
    public int value;
    public double time;
    public int state;
    public string action;
    public float reward;


    public Action(int val, float t, int st, string act, float r)
    {
        value = val;
        time = System.Math.Round(t, 2);
        state = st;
        action = act;
        reward = r;
    }
}