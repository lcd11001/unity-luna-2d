using UnityEngine;

public class FuelEventListener : GameEventListener<float>
{
    protected override void Respond(float value)
    {
        base.Respond(value);
        Debug.Log("Fuel picked up: " + value);
    }
}
