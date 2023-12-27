using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public PlayerInputActions playerInputActions;
    public GameSettings defaultGameSettings;
    public GameSettings gameSettings;

    private int score = 0;
    [HideInInspector]
    public UnityEvent<int> UpdateScoreEvent;
    public bool isLost;
    [HideInInspector]
    public UnityEvent<int> GameOverEvent;

    public int currentFruitid = 1;
    public int nextFruitid = 1;
    [HideInInspector]
    public UnityEvent<int, int> UpdateNextFruitEvent;

    [HideInInspector]
    public UnityEvent OnFruitMerge;
    [HideInInspector]
    public UnityEvent OnFruitDrop;

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
        if (GameOverEvent == null)
        {
            GameOverEvent = new UnityEvent<int>();
        }
        if (UpdateNextFruitEvent == null)
        {
            UpdateNextFruitEvent = new UnityEvent<int, int>();
        }
        if (OnFruitMerge == null)
        {
            OnFruitMerge = new UnityEvent();
        }
        if (OnFruitDrop == null)
        {
            OnFruitDrop = new UnityEvent();
        }
        // Init Player Controls
        playerInputActions = new PlayerInputActions();
        playerInputActions.GameScene.Enable();

        // Init game
        isLost = false;
        Physics2D.gravity = new Vector2(0, gameSettings.gravity);
        BuildFruitDictionaries();
        currentFruitid = Random.Range(1, gameSettings.maxStartingFruit);
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
        playerInputActions.GameScene.Disable();
        if (!isLost)
        {
            isLost = true;
            GameOverEvent?.Invoke(score);
        }
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
        // Trigger merge soundFX
        OnFruitMerge?.Invoke();


        int originalFruitid = sizeToFruitid[originalSize];
        int newFruitid = originalFruitid + 1;

        // Calculate new fruit position
        Vector2 newPos = new Vector2((fruit1.transform.position.x + fruit2.transform.position.x) / 2,
                                      (fruit1.transform.position.y + fruit2.transform.position.y) / 2);

        // Remove old fruit (Merged)
        fruit1.valid = false;
        fruit2.valid = false;
        Destroy(fruit1.gameObject);
        Destroy(fruit2.gameObject);

        // Update Score
        ChangeScore(idToPoints[originalFruitid]);

        // Spawn new fruit if possible (If two watermelon merge, no new fruit is created)
        if (newFruitid < gameSettings.fruitList.Length)
        {
            float correctSize = gameSettings.fruitList[newFruitid].size;

            // Add small explosion to make room
            MergeExplosion(newPos, correctSize);

            // Create new fruit
            GameObject newFruit = Instantiate(gameSettings.fruitList[newFruitid].fruitPrefab);
            newFruit.transform.localScale = new Vector3(correctSize, correctSize, correctSize);
            newFruit.transform.position = newPos;

            FruitPhysics fp = newFruit.GetComponent<FruitPhysics>();
            fp.type = gameSettings.fruitList[newFruitid];
            fp.GetComponent<Collider2D>().enabled = true;
            fp.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

           
        }


        return true;
    }

    private void MergeExplosion(Vector2 explosionCenter, float size)
    {
        if (gameSettings.mergeExplosionForce == 0)
        {
            return;
        }

        // Merge explosion test
        /*
        GameObject o = Instantiate(gameSettings.fruitList[0].fruitPrefab);
        o.transform.localScale = new Vector3(1, 1, 1);
        o.GetComponent<SpriteRenderer>().enabled = false;
        o.transform.position = explosionCenter;
        o.GetComponent<CircleCollider2D>().radius = (size * 0.5f) * gameSettings.mergeExplosionRadiusModifier;
        o.GetComponent<CircleCollider2D>().isTrigger = true;
        o.GetComponent<CircleCollider2D>().enabled = true;
        */

        // Merge explosion
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionCenter, (size * 0.5f) * gameSettings.mergeExplosionRadiusModifier, LayerMask.GetMask("Fruit"));
        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            Vector2 hitPos = new Vector2(hit.transform.position.x, hit.transform.position.y);
            Vector2 explosionDir = hitPos - explosionCenter;
            explosionDir.Normalize();
            explosionDir.y += gameSettings.mergeExplosionUpwardsModifier;
            explosionDir.Normalize();
            if (rb != null)
            {
                //rb.AddForce(explosionDir * Mathf.Lerp(0f, gameSettings.mergeExplosionForce, (1 - Vector2.Distance(hitPos, explosionCenter))), ForceMode2D.Impulse);
                rb.AddForce(explosionDir * gameSettings.mergeExplosionForce, ForceMode2D.Impulse);
            }
        }
    }
}
