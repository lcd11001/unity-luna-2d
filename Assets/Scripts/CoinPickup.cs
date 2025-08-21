using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField]
    private int coinAmount = 50; // Amount of coin to add when picked up

    [Tooltip("Event channel to raise when a coin is picked up")]
    [SerializeField]
    private CoinEventChannelSO coinEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (coinEvent != null)
        {
            coinEvent.RaiseEvent(coinAmount);
        }
        Destroy(gameObject); // Destroy the pickup after collection
    }
}
