using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ObjectSO", menuName ="Game/ObjectSO")]
public class ObjectSO : ScriptableObject
{
    [SerializeField] private List<ObjectLvl> _objectsLvlUps = new();
    public List<ObjectLvl> ObjectsLvlUps => _objectsLvlUps;
}


[System.Serializable]  
public class ObjectLvl
{
    public Sprite Icon;
    public int Lvl;
}