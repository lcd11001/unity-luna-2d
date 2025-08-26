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

public class OnLandingEvent : UnityEvent
{
    public LandingType type;
    public float scoreMultiplier;
    public int score;
    public float landingDotVector;
    public float landingSpeed;
}

[CreateAssetMenu(fileName = "LandedEventChannel", menuName = "ScriptableObjects/GameEventChannel/Landed")]
public class LandedEventChannelSO : GameEventChannelSO<OnLandingEvent>
{
}
