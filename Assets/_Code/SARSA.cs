
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
    private List<State> state;
    private List<Action> action;
    //private float translation;
    //private bool canJump = true;

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
        StartCoroutine(Learning());
    }


    void Update()
    {

      
        if (Input.GetButton("Cancel"))
        {
            PlayerPrefs.SetInt("Episode", 0);
            Application.Quit();
        }


    }
    
    IEnumerator Learning()
    {

        for (int i = 0; i < 121; i++)
        {
            state.Add(new State());
            state[i].StateValue = i;
            if (i == 39)
            {
                state[i].Reward = 20;
            }
            //else if (i == 13 || i == 14 || i == 15)
            //{
            //    state[i].Reward = -20;
            //}
            else
            {
                state[i].Reward = -1;
            }


        }


        float Alpha = .1f;
        float Gamma = .9f;
        float eGreddy = 5;
        int currentEpisode = 0;
        int currentAction = 0;
        float startTime;
        int Episodes = 50;
        int AfterEpisodes = 1;
        int AfterEGreddy = 1;

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

        yield return new WaitForSeconds(.5f);

        episodeUI.text = currentEpisode.ToString();

        while (currentEpisode < Episodes)
        {
            
            nextState = character.CurrentState();
            currentStateUI.text = nextState.ToString();

            eGreddy = 30 / ((Time.time - startTime) * (currentEpisode + 1));
            print(Time.time);
            print(eGreddy);

            if (Random.value <= eGreddy)
            {
                nextAction = DoRandomAction();
            }
            else
            {
                nextAction = GetMaxQ();
            }

            QValue = state[currentState].Action[firstAction];
            qValue = state[nextState].Action[nextAction];

            QValue = QValue + Alpha * (state[nextState].Reward + (Gamma * qValue) - QValue);

            state[currentState].Action[firstAction] = QValue;


            if (currentState == 39)
            {
                currentState = 0;
                firstAction = "R";
                character.ResetPoss();
                character.Move(firstAction);
                startTime = Time.time;
                yield return new WaitForSeconds(.5f);

                currentEpisode++;
                episodeUI.text = currentEpisode.ToString();

            }
            else
            {
                currentState = nextState;
                firstAction = nextAction;
                character.Move(firstAction);
                yield return new WaitForSeconds(.5f);

            }

            currentAction++;
            action.Add(new Action(currentAction, Time.time - startTime, currentState, firstAction, state[currentState].Reward));

        }

        character.StopMoving();
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

    string GetMaxQ()
    {
        string action = "N";


        if (state[nextState].Action[action] < state[nextState].Action["L"])
        {
            action = "L";
        }
        if (state[nextState].Action[action] < state[nextState].Action["R"])
        {
            action = "R";
        }
        if (state[nextState].Action[action] < state[nextState].Action["UL"])
        {
            action = "UL";
        }
        if (state[nextState].Action[action] < state[nextState].Action["UR"])
        {
            action = "UR";
        }
        if (state[nextState].Action[action] < state[nextState].Action["U"])
        {
            action = "U";
        }

        return action;
    }

    public void WriteVtable()
    {

        string filePath = Application.dataPath + "/Qtable.txt";

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
            string MaxAction = GetMaxQ();

            //writer.WriteLine("V: " + state[i].StateValue + "   Nothing: " + state[i].Action["N"] + "  Left: " + state[i].Action["L"] + "   Right: " + state[i].Action["R"]
            //    + "   Up & Left: " + state[i].Action["UL"] + "  Up & Right: " + state[i].Action["UR"] + "   Up: " + state[i].Action["U"]);
            writer.WriteLine("V: " + state[i].StateValue + "   " + state[i].Action[MaxAction]);

        }

        //PlayerPrefs.SetInt("Episode", PlayerPrefs.GetInt("Episode") + 1);
        writer.WriteLine("");
        writer.Flush();
        writer.Close();

    }

    public void WriteActions()
    {
        
        string filePath = Application.dataPath + "/Actions.txt";

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