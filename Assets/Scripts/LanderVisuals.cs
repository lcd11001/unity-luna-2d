using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(Lander))]
public class LanderVisuals : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem leftThrusterParticles;

    [SerializeField]
    private ParticleSystem midThrusterParticles;

    [SerializeField]
    private ParticleSystem rightThrusterParticles;

    private Lander lander;
    private void Awake()
    {
        lander = GetComponent<Lander>();

        if (lander != null)
        {
            lander.OnUpForce += HandleUpForce;
            lander.OnLeftForce += HandleLeftForce;
            lander.OnRightForce += HandleRightForce;
            lander.OnBeforeForce += HandleBeforeForce;
        }
    }

    private void OnDestroy()
    {
        if (lander != null)
        {
            lander.OnUpForce -= HandleUpForce;
            lander.OnLeftForce -= HandleLeftForce;
            lander.OnRightForce -= HandleRightForce;
            lander.OnBeforeForce -= HandleBeforeForce;
        }
    }

    private void Start()
    {
        SetEnableParticle(midThrusterParticles, false);
        SetEnableParticle(leftThrusterParticles, false);
        SetEnableParticle(rightThrusterParticles, false);
    }

    private void SetEnableParticle(ParticleSystem particleSystem, bool isEnabled)
    {
        ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
        emissionModule.enabled = isEnabled;
    }

    private void HandleRightForce(object sender, EventArgs e)
    {
        SetEnableParticle(leftThrusterParticles, true);
    }

    private void HandleLeftForce(object sender, EventArgs e)
    {
        SetEnableParticle(rightThrusterParticles, true);
    }

    private void HandleUpForce(object sender, EventArgs e)
    {
        SetEnableParticle(leftThrusterParticles, true);
        SetEnableParticle(midThrusterParticles, true);
        SetEnableParticle(rightThrusterParticles, true);
    }

    private void HandleBeforeForce(object sender, EventArgs e)
    {
        SetEnableParticle(midThrusterParticles, false);
        SetEnableParticle(leftThrusterParticles, false);
        SetEnableParticle(rightThrusterParticles, false);
    }
}
