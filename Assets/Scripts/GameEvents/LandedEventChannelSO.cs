using System;
using UnityEngine;
using UnityEngine.Events;

public enum LandingType
{
    Success,
    WrongLandingArea,
    TooSteepAngle,
    TooFastLanding
}

[Serializable]
public class OnLandingEvent
{
    public LandingType type;
    public float scoreMultiplier;
    public int score;
    public float landingDotVector;
    public float landingSpeed;

    public override string ToString()
    {
        return $"Type: {type}, Score: {score}, Multiplier: {scoreMultiplier}, DotVector: {landingDotVector}, Speed: {landingSpeed}";
    }
}

[CreateAssetMenu(fileName = "LandedEventChannel", menuName = "ScriptableObjects/GameEventChannel/Landed")]
public class LandedEventChannelSO : GameEventChannelSO<OnLandingEvent>
{
}
