using UnityEngine;
using TMPro;

// Tips: seperate logic vs visual representation
[RequireComponent(typeof(LandingPad))]
public class LandingPadVisual : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;

    private LandingPad landingPad;

    private void Awake()
    {
        landingPad = GetComponent<LandingPad>();
        // Debug.Log("LandingPadVisual initialized with ScoreMultiplier: " + landingPad.ScoreMultiplier);
        if (scoreText != null && landingPad != null)
        {
            scoreText.text = "x" + landingPad.ScoreMultiplier;
        }
    }
}
