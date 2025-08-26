using UnityEngine;

public class LanderStateEventListener : GameEventListener<Lander.State>
{
    protected override void Respond(Lander.State state)
    {
        base.Respond(state);
        Debug.Log("Lander state changed to: " + state);
    }
}
