using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



[CreateAssetMenu(fileName = "New Difficult", menuName = "Setting/Difficult", order = 52)]
public class Difficult : ScriptableObject
{
    public float timeBetweenAction;
    [Range(10,20)]
    public int maxRandomSpawnPlanet;
    [Range(5, 10)]
    public int minRandomSpawnPlanet;
    [Range(1, 3)]
    public int maxRandomRadius;
}
