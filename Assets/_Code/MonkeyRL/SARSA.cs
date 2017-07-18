
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

    public float Episodes;

    private List<State> state;
    private List<Action> action;
    
    //RL Settings
    int currentState;
    int nextState;
    
    void Start()
    {
        state = new List<State>();
        action = new List<Action>();

        //for (int i = 0; i < 120; i++)
        //{
        //    state.add(new state());
        //    state[i].statevalue = i;
        //    if (i == 39 || i == 79 || i == 119)
        //    {
        //        state[i].reward = 1000;
        //    }
        //    else if (i == 27 || i == 48) //trap
        //    {
        //        state[i].reward = -50;
        //    }
        //    else if (i == 8) // coin +2
        //    {
        //        state[i].reward = -10;
        //    }
        //    else if (i == 88 || i == 103 || i == 108) // coin +4 
        //    {
        //        state[i].reward = -1;
        //    }
        //    else
        //    {
        //        state[i].reward = -10;
        //    }


        //}
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(RunPolicy());
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            WriteQtable();
            WriteVtable();
            WriteActions();
            SavePolicy();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            InstantiateArrows();
        }

    }
    
    public void StartLearning()
    {

        for (int i = 0; i < 120; i++)
        {
            state.Add(new State());
            state[i].StateValue = i;
            if (i == 39 || i == 79 || i == 119)
            {
                state[i].Reward = 50000;
            }
            else if (i == 27 || i == 48) //Trap
            {
                state[i].Reward = -50;
            }
            else if (i == 8) // Coin +2
            {
                state[i].Reward = -10;
            }
            else if (i == 88 || i == 103 || i == 108) // Coin +4 
            {
                state[i].Reward = -1;
            }
            else
            {
                state[i].Reward = -10;
            }


        }

        StartCoroutine(Learning());
    }

    IEnumerator Learning()
    {

        float Alpha = .1f;
        float Gamma = .9f;
        float eGreddy;
        int currentEpisode = 0;
        int currentAction = 0;
        //float startTime;
        

        float QValue;
        float qValue;

        string firstAction;
        string nextAction;

        int reachTree = 0;

        yield return new WaitForSeconds(1f);

        //First Action 
        //Move Right
        currentState = 0;
        firstAction = DoRandomAction();
        character.Move(firstAction);
        

        action.Add(new Action(currentAction, Time.time, currentState, firstAction, state[currentState].Reward));


        yield return new WaitForSeconds(.2f);

        episodeUI.text = currentEpisode.ToString();

        while (currentEpisode < Episodes)
        {

            nextState = character.CurrentState();

            currentStateUI.text = nextState.ToString();
            
            eGreddy = (currentEpisode / Episodes);
            eGreddy = Mathf.Clamp(eGreddy, .1f, 0.9f);
            
            if (Random.value >= eGreddy)
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

            float rw = state[nextState].Reward; //- Time.time;
            QValue = QValue + Alpha * (rw + (Gamma * qValue) - QValue);

            state[currentState].Action[firstAction] = QValue;


            if (currentState == 39 || currentState == 79 || currentState == 119)
            {
                currentEpisode++;
                episodeUI.text = currentEpisode.ToString();

               

                currentState = 0;
                firstAction = "R";
                character.ResetPoss();
                character.Move(firstAction);
                yield return new WaitForSeconds(.2f);
                


            }
            else if (currentState == 27 || currentState == 48)
            {
                //currentEpisode++;
                //episodeUI.text = currentEpisode.ToString();

                currentState = nextState;
                firstAction = nextAction;
                character.Move(firstAction);
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
            action.Add(new Action(currentAction, Time.time, currentState, firstAction, state[currentState].Reward));

        }
        print(reachTree);

        character.StopMoving();
        WriteQtable();
        WriteVtable();
        WriteActions();
        SavePolicy();
    }

    string DoRandomAction()
    {
        int action = Random.Range(0, 3);

        if (action == 0)
        {
            return "L";
        }
        else if (action == 1)
        {
            return "R";
        }
        else if (action == 2)
        {
            return "U";
        }
        
        return null;
    }

    string GetMaxQ(int st)
    {
        string action = "L";

        
        if (state[st].Action[action] < state[st].Action["R"])
        {
            action = "R";
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
                print(currentState);
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
           
            Policy.Instance.policyState.state.Add(pol);
            Policy.Instance.SavePolicy();
            
        }
    }

    void InstantiateArrows()
    {
        Policy.Instance.LoadPolicy();


        for (int i = 0; i < state.Count; i++)
        {
            string action = Policy.Instance.policyState.state[i].action;
            int Zrotation = 0;
            float Xposs,Yposs;

            Xposs = i;
            Yposs = 0;
            if(Xposs < 40)
            {
                Yposs = 1;
            }
            else if (Xposs < 80)
            {
                Yposs = 3;
                Xposs -= 40; 
            }
            else if (Xposs < 120)
            {
                Yposs = 5;
                Xposs -= 80;
            }
            
            if (action == "L")
            {
                Zrotation = 180;
            }
            else if (action == "R")
            {
                Zrotation = 0;
            }
            else if (action == "U")
            {
                Zrotation = 90;
            }
            Instantiate(arrow, new Vector3(Xposs + .30f, Yposs, 0), Quaternion.Euler(new Vector3(0, 0, Zrotation)));
           
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
        writer = new StreamWriter(filePath, append: false);
        
        for (int i = 0; i < state.Count; i++)
        {
            string MaxAction = GetMaxQ(i);
            writer.WriteLine("V: " + state[i].StateValue + "   " + state[i].Action[MaxAction] + "   " + MaxAction);

        }
        
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
        
        writer = new StreamWriter(filePath, append: false);

        for (int i = 0; i < action.Count; i++)
        {

            writer.WriteLine(action[i].value + "  " + action[i].time + "  " + action[i].state + "  " + action[i].action + "  " + action[i].reward);

        }


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
        writer = new StreamWriter(filePath, append: false);


        for (int i = 0; i < state.Count; i++)
        {
            string MaxAction = GetMaxQ(i);

            writer.WriteLine("V: " + state[i].StateValue +"  Left: " + state[i].Action["L"] + "   Right: " + state[i].Action["R"]
                 + "   Up: " + state[i].Action["U"]);
        }

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
        
       
        Action.Add("L", 0);
        Action.Add("R", 0);
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