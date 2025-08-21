using UnityEngine;

public class CoinEventListener : GameEventListener<int>
{
    protected override void Respond(int value)
    {
        base.Respond(value);
        Debug.Log("Coin picked up: " + value);
    }
}
