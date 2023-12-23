using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameSettings")]
public class GameSettings : ScriptableObject
{
    public float gravity = -9.8f;

    public float dropperMaxWidth = 3f;
    public float dropperSpawnDelay = 0.5f;
    public bool useDynamicDropperEdgeOffset = false;

    public int maxStartingFruit = 1;

    public PhysicsMaterial2D defaultPhysicsMaterial;
    public PhysicsMaterial2D physicsMaterial;
    public Fruit[] fruitList;

    public int[] pointSystem;

}
