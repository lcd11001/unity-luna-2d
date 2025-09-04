using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [field: SerializeField]
    public int Score { get; private set; }

    [field: SerializeField]
    public float Time { get; private set; }

    [field: SerializeField]
    public int CurrentLevel { get; private set; }

    [SerializeField]
    private List<GameLevel> gameLevels;

    public static GameManager Instance { get; private set; }

    private bool isGameActive = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadCurrentLevel();
    }

    private void Update()
    {
        // Should use event instead, to avoid direct reference
        // if (Lander.Instance == null || Lander.Instance.GetState() != Lander.State.Flying)
        // {
        //     return;
        // }
        if (isGameActive)
        {
            Time += UnityEngine.Time.deltaTime;
        }
    }

    public void HandleLandedSuccessful(int score)
    {
        AddScore(score);
    }

    public void HandleCoinPickedUp(int amount)
    {
        AddScore(amount);
    }

    public void HandleLanderStateChanged(Lander.State state)
    {
        isGameActive = state == Lander.State.Flying;
    }

    private void AddScore(int amount)
    {
        Score += amount;
        Debug.Log("Score added: " + amount + ", new total: " + Score);
    }

    private void LoadCurrentLevel()
    {
        // Load the current level data
        if (gameLevels != null && gameLevels.Count > 0)
        {
            foreach (var level in gameLevels)
            {
                if (level.LevelNumber == CurrentLevel)
                {
                    // Instantiate the level prefab
                    var newLevel = Instantiate(level, Vector3.zero, Quaternion.identity);

                    // Position the player at the spawn point
                    if (Lander.Instance != null && newLevel.PlayerSpawnPoint != null)
                    {
                        Lander.Instance.transform.position = newLevel.PlayerSpawnPoint.position;
                    }
                    break;
                }
            }
        }
    }
}
