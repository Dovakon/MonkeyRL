using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


public class Policy : MonoBehaviour{

    public static Policy Instance { get; private set; }

    void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            
            Destroy(gameObject);
        }
        
        Instance = this;
        
    }


    public PolicyStates policyState;


    public void SavePolicy()
    {
        string filePath = Application.dataPath + "/StreamingAssets/XML/Policy.xml";

        XmlSerializer serializer = new XmlSerializer(typeof(PolicyStates));

        FileStream stream = new FileStream(filePath, FileMode.Create);

        serializer.Serialize(stream, policyState);

        stream.Close();
    }
    public void LoadPolicy()
    {
        string filePath = Application.dataPath + "/StreamingAssets/XML/Policy.xml";

        XmlSerializer serializer = new XmlSerializer(typeof(PolicyStates));

        FileStream stream = new FileStream(filePath, FileMode.Open);

        policyState =  serializer.Deserialize(stream) as PolicyStates;

        stream.Close();
    }
}

[System.Serializable]
public class PolicyStates
{
    public List<PolicyValues> state = new List<PolicyValues>();
}

[System.Serializable]
public class PolicyValues
{
    public int value;
    public string action;
}