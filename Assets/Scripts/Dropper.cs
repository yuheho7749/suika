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
        float offset = 0;
        if (currentFruit)
        {
            offset = currentFruit.size / 2;
        }

        float mouseX = Mathf.Clamp(relativeMousePos.x, 
            -GameController.current.gameSettings.dropperMaxWidth + offset, 
            GameController.current.gameSettings.dropperMaxWidth - offset);

        gameObject.transform.position = new Vector2(mouseX, gameObject.transform.position.y);

        if (Input.GetMouseButtonDown(0))
        {
            DropFruit();
        }
    }

    void SpawnNewFruit()
    {
        currentFruit = GameController.current.GetCurrentFruit();
        currentFruitObject = Instantiate(currentFruit.fruitPrefab, transform);

        currentFruitObject.transform.localScale = new Vector3(currentFruit.size, currentFruit.size, currentFruit.size);
    }

    void DropFruit()
    {
        FruitPhysics fp = currentFruitObject.GetComponent<FruitPhysics>();
        fp.type = currentFruit;
        fp.Drop();
        GameController.current.ConsumeCurrentFruit();
        SpawnNewFruit();
    }
}
