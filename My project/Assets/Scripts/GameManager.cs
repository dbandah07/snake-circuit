using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    // score +
    // glitch cells

    public static GameManager instance;

    public int score = 0;
    public TextMeshProUGUI scoreTXT;
    public TextMeshProUGUI layerTXT;

    public Animator glitchAnim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    public void AddScore(int val)
    {
        score += val;

        scoreTXT.text = "SCORE: " + score;

    }

    public void PlayGlitch()
    {

    //    if (glitchAnim != null)
    //    {
            glitchAnim.SetTrigger("Flash");
    //    }
    }
}
