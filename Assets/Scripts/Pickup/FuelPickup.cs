using UnityEngine;

public class FuelPickup : MonoBehaviour
{
    [SerializeField]
    private float fuelAmount = 5f; // Amount of fuel to add when picked up

    [Tooltip("Event channel to raise when fuel is picked up")]
    [SerializeField]
    private FuelEventChannelSO fuelEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (fuelEvent != null)
        {
            fuelEvent.RaiseEvent(fuelAmount);
        }
        Destroy(gameObject); // Destroy the pickup after collection
    }
}
