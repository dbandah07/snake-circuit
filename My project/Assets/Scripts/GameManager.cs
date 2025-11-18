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
        if (instance == null) instance = this;
        else Destroy(gameObject);

        scoreTXT.text = "SCORE: 0";
        layerTXT.text = "LAYER: 1";

    }

    // Update is called once per frame
    public void AddScore(int val)
    {
        score += val;
        scoreTXT.text = "SCORE: " + score;

        int layer = (score / 5) + 1;
        layerTXT.text = "LAYER: " + layer;
    }

    public void PlayGlitch()
    {

    //    if (glitchAnim != null)
    //    {
            glitchAnim.SetTrigger("Flash");
    //    }
    }
}
