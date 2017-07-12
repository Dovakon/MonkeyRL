using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Policy/Vvalue")]
public class Policy : ScriptableObject {
    
    public List<Vvalue> vValue = new List<Vvalue>();
}

public struct Vvalue
{
    public int state;
    public string action;
}