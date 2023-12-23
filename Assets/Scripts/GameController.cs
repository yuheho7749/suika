using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public GameSettings gameSettings;

    private int score = 0;
    public UnityEvent<int> UpdateScoreEvent;
    public UnityEvent<int> LoseEvent;

    public int currentFruitid = 1;
    public int nextFruitid = 1;
    public UnityEvent<int, int> UpdateNextFruitEvent;

    private Dictionary<float, int> sizeToFruitid;
    private Dictionary<int, int> idToPoints;

    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(this);
        } 
        else 
        {
            instance = this;
        }

        if (UpdateScoreEvent == null)
        {
            UpdateScoreEvent = new UnityEvent<int>();
        }
        if (LoseEvent == null)
        {
            LoseEvent = new UnityEvent<int>();
        }
        if (UpdateNextFruitEvent == null)
        {
            UpdateNextFruitEvent = new UnityEvent<int, int>();
        }

        Physics2D.gravity = new Vector2(0, gameSettings.gravity);
        BuildFruitDictionaries();
        nextFruitid = Random.Range(1, gameSettings.maxStartingFruit);
    }

    private void BuildFruitDictionaries()
    {
        sizeToFruitid = new Dictionary<float, int>();
        for (int i = 0; i < gameSettings.fruitList.Length; i ++)
        {
            sizeToFruitid.Add(gameSettings.fruitList[i].size, i);
        }

        idToPoints = new Dictionary<int, int>();
        for (int i = 0; i < gameSettings.fruitList.Length; i++)
        {
            idToPoints.Add(i, gameSettings.pointSystem[i]);
        }
    }

    public void Lose()
    {
        Debug.Log("You Lose");
        LoseEvent?.Invoke(score);
        Invoke("Restart", 5f); // Temp
    }

    public void Restart()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Update()
    {
        // Test Code to switch gravity
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Physics2D.gravity = -Physics2D.gravity;
        }
    }

    public Fruit GetCurrentFruit()
    {
        UpdateNextFruitEvent?.Invoke(currentFruitid, nextFruitid);
        return gameSettings.fruitList[currentFruitid];
    }

    public void ConsumeCurrentFruit()
    {
        currentFruitid = nextFruitid;
        nextFruitid = Random.Range(1, gameSettings.maxStartingFruit);
        UpdateNextFruitEvent?.Invoke(currentFruitid, nextFruitid);
    }

    public int ChangeScore(int diff)
    {
        score += diff;
        UpdateScoreEvent?.Invoke(score);
        return score;
    }

    public int SetScore(int newScore)
    {
        score = newScore;
        UpdateScoreEvent?.Invoke(score);
        return score;
    }

    public bool HandleFruitMerge(FruitPhysics fruit1, FruitPhysics fruit2, float originalSize)
    {
        int originalFruitid = sizeToFruitid[originalSize];
        int newFruitid = originalFruitid + 1;

        // Calculate new fruit position
        Vector2 newPos = new Vector2((fruit1.transform.position.x + fruit2.transform.position.x) / 2,
                                      (fruit1.transform.position.y + fruit2.transform.position.y) / 2);

        // Remove old fruit
        fruit1.valid = false;
        fruit2.valid = false;
        Destroy(fruit1.gameObject);
        Destroy(fruit2.gameObject);

        // Update Score
        ChangeScore(idToPoints[originalFruitid]);

        // Merge fruit if possible. Otherwise (in watermelon's case), disappear.
        if (newFruitid < gameSettings.fruitList.Length)
        {
            // Create new fruit
            GameObject newFruit = Instantiate(gameSettings.fruitList[newFruitid].fruitPrefab);
            float correctSize = gameSettings.fruitList[newFruitid].size;
            newFruit.transform.localScale = new Vector3(correctSize, correctSize, correctSize);
            newFruit.transform.position = newPos;

            FruitPhysics fp = newFruit.GetComponent<FruitPhysics>();
            fp.type = gameSettings.fruitList[newFruitid];
            fp.GetComponent<Collider2D>().enabled = true;
            fp.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        }


        return true;
    }
}
