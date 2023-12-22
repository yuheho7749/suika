using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour
{
    private Fruit currentFruit;
    private GameObject currentFruitObject;

    private void Start()
    {
        SpawnNewFruit();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 relativeMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float offset = GameController.instance.gameSettings.fruitList[GameController.instance.gameSettings.maxStartingFruit-1].size / 2;
        if (currentFruit && GameController.instance.gameSettings.useDynamicDropperEdgeOffset)
        {
            offset = currentFruit.size / 2;
        }

        float mouseX = Mathf.Clamp(relativeMousePos.x, 
            -GameController.instance.gameSettings.dropperMaxWidth + offset, 
            GameController.instance.gameSettings.dropperMaxWidth - offset);

        gameObject.transform.position = new Vector2(mouseX, gameObject.transform.position.y);

        if (currentFruit && Input.GetMouseButtonDown(0))
        {
            DropFruit();
        }
    }

    void SpawnNewFruit()
    {
        currentFruit = GameController.instance.GetCurrentFruit();
        currentFruitObject = Instantiate(currentFruit.fruitPrefab, transform);

        currentFruitObject.transform.localScale = new Vector3(currentFruit.size, currentFruit.size, currentFruit.size);
    }

    void DropFruit()
    {
        FruitPhysics fp = currentFruitObject.GetComponent<FruitPhysics>();
        fp.type = currentFruit;
        fp.Drop();
        GameController.instance.ConsumeCurrentFruit();
        currentFruit = null;
        Invoke("SpawnNewFruit", GameController.instance.gameSettings.dropperSpawnDelay);
    }
}
