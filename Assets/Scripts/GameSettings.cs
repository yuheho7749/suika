using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameSettings")]
public class GameSettings : ScriptableObject
{
    public float gravity = -9.8f;
    public float dropperMaxWidth = 3f;

    public int maxStartingFruit = 1;

    public PhysicsMaterial2D physicsMaterial;
    public Fruit[] fruitList;

    public int[] pointSystem;

}
