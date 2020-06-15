using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "FieldNameHere", menuName = "ScriptableObjects/InteractionField", order = 1)]
public class InteractionField : ScriptableObject
{
    
    /// <summary>
    /// Name of this interaction field, e.g. "Electric".
    /// </summary>
    public string name;
    
}
