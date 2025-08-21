using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [field: SerializeField]
    public int Score { get; private set; }

    public static GameManager Instance { get; private set; }

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

    public void HandleLandedSuccessful(int score)
    {
        AddScore(score);
    }

    public void HandleCoinPickedUp(int amount)
    {
        AddScore(amount);
    }

    private void AddScore(int amount)
    {
        Score += amount;
        Debug.Log("Score added: " + amount + ", new total: " + Score);
    }
}
