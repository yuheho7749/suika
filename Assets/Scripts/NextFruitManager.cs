using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextFruitManager : MonoBehaviour
{
    public float minSize;
    public float maxSize;

    private float minOriginalSize;
    private float maxOriginalSize;

    private GameObject[] fruitImages;

    private int fruitid;

    private void Start()
    {
        fruitImages = new GameObject[GameController.instance.gameSettings.fruitList.Length];

        minOriginalSize = GameController.instance.gameSettings.fruitList[1].size;
        maxOriginalSize = GameController.instance.gameSettings.fruitList[GameController.instance.gameSettings.maxStartingFruit - 1].size;

        for (int i = 0; i < fruitImages.Length; i ++)
        {
            fruitImages[i] = Instantiate(GameController.instance.gameSettings.fruitList[i].fruitPrefab, transform);
            float newFruitSize = ScaleFruitSize(GameController.instance.gameSettings.fruitList[i].size);
            fruitImages[i].transform.localScale = new Vector3(newFruitSize, newFruitSize, newFruitSize);
            fruitImages[i].SetActive(false);
        }

        fruitid = 1;
        ChangeFruit(fruitid, GameController.instance.nextFruitid);
        GameController.instance.UpdateNextFruitEvent.AddListener(ChangeFruit);
    }

    private void ChangeFruit(int oldFruitid, int newFruitid)
    {
        fruitImages[fruitid].SetActive(false);
        fruitid = newFruitid;
        fruitImages[fruitid].SetActive(true);
    }

    private float ScaleFruitSize(float original)
    {
        return Mathf.Lerp(minSize, maxSize, Mathf.InverseLerp(minOriginalSize, maxOriginalSize, original));
    }

    private void OnDestroy()
    {
        GameController.instance.UpdateNextFruitEvent.RemoveListener(ChangeFruit);
    }
}
