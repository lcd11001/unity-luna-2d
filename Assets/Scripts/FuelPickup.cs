using UnityEngine;

public class FuelPickup : MonoBehaviour
{
    [SerializeField]
    private float fuelAmount = 5f; // Amount of fuel to add when picked up

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Lander lander = collision.GetComponent<Lander>();
        if (lander != null)
        {
            lander.AddFuel(fuelAmount);
            Destroy(gameObject); // Destroy the pickup after collection
        }
    }
}
