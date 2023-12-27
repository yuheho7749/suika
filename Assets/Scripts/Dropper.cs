using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dropper : MonoBehaviour
{
    public float clickTolerance = 1;
    private Fruit currentFruit;
    private GameObject currentFruitObject;

    private Vector3 mousePos;

    private void Start()
    {
        GameController.instance.playerInputActions.GameScene.MousePrimaryClick.started += MousePrimaryClick_started;
        SpawnNewFruit();
    }

    private void MousePrimaryClick_started(InputAction.CallbackContext obj)
    {
        if (!GameController.instance.isLost && currentFruit && currentFruitObject && IsMouseInPlayArea())
        {
            DropFruit();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.instance.isLost)
        {
            if (currentFruitObject.transform.parent == transform)
            {
                currentFruitObject.SetActive(false);
            }
            return;
        }

        Vector2 pixelMousePos = GameController.instance.playerInputActions.GameScene.MousePosition.ReadValue<Vector2>();
        mousePos = Camera.main.ScreenToWorldPoint(pixelMousePos);

        float offset = GameController.instance.gameSettings.fruitList[GameController.instance.gameSettings.maxStartingFruit-1].size / 2;
        if (currentFruit && GameController.instance.gameSettings.useDynamicDropperEdgeOffset)
        {
            offset = currentFruit.size / 2;
        }

        float mouseX = Mathf.Clamp(mousePos.x, 
            -GameController.instance.gameSettings.dropperMaxWidth + offset, 
            GameController.instance.gameSettings.dropperMaxWidth - offset);

        gameObject.transform.position = new Vector2(mouseX, gameObject.transform.position.y);
    }

    void SpawnNewFruit()
    {
        GameController.instance.ConsumeCurrentFruit();
        currentFruit = GameController.instance.GetCurrentFruit();
        currentFruitObject = Instantiate(currentFruit.fruitPrefab, transform);

        currentFruitObject.transform.localScale = new Vector3(currentFruit.size, currentFruit.size, currentFruit.size);
    }
    public bool SpawnNewFruitAfterPreviousCollide(FruitPhysics fp)
    {
        if (fp.gameObject == currentFruitObject)
        {
            SpawnNewFruit();
            return true;
        }
        return false;
    }

    void DropFruit()
    {
        FruitPhysics fp = currentFruitObject.GetComponent<FruitPhysics>();
        fp.type = currentFruit;
        fp.Drop(this);
        currentFruit = null;
        GameController.instance.soundFXManager.PlayDropSoundFX();

    }

    private bool IsMouseInPlayArea()
    {
        return -GameController.instance.gameSettings.dropperMaxWidth - clickTolerance <= mousePos.x &&
            mousePos.x <= GameController.instance.gameSettings.dropperMaxWidth + clickTolerance;
    }
}
