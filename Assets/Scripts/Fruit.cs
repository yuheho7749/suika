using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Fruit")]
public class Fruit : ScriptableObject
{
    public float size = 1f;
    public GameObject fruitPrefab;
}
