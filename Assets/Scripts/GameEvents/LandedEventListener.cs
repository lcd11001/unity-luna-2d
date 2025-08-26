using UnityEngine;

public class LandedEventListener : GameEventListener<OnLandingEvent>
{
    protected override void Respond(OnLandingEvent value)
    {
        base.Respond(value);
        Debug.Log("Landed with type: " + value);
    }
}
