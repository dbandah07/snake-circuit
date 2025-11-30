using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // score +
    // glitch cells

    public static GameManager instance;

    public int score = 0;
    public TextMeshProUGUI scoreTXT;
    public TextMeshProUGUI layerTXT;

    public Animator glitchAnim;


    public GameObject Layer2Objects;
    private int currLayer = 1;
    private bool isTrans = false; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    


    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        scoreTXT.text = "SCORE: 0";
        layerTXT.text = "LAYER: 1";

        if (Layer2Objects != null)
        {
            Layer2Objects.SetActive(false);
        }

    }

    // Update is called once per frame
    public void AddScore(int val)
    {
        score += val;
        scoreTXT.text = "SCORE: " + score;

        int layer = (score / 5) + 1;
        layerTXT.text = "LAYER: " + layer;
        if (layer > currLayer)
        {
            currLayer = layer;
            StartCoroutine(DoLayerTransition(layer));
        }

    }

    public void PlayGlitch()
    {

    //    if (glitchAnim != null)
    //    {
            glitchAnim.SetTrigger("Flash");
    //    }
    }

    void ActivateLayer(int layer)
    {
        if (layer == 2)
        {
            if (Layer2Objects != null)
            {
                Layer2Objects.SetActive(true);
            }
        }
    }

    IEnumerator DoLayerTransition(int layer)
    {
        if (isTrans) yield break;
        isTrans = true;

        // freeze snake
        SnakeController snake = FindObjectOfType<SnakeController>();
        Rigidbody2D rb = snake.GetComponent<Rigidbody2D>();
        Collider2D col = snake.GetComponent<Collider2D>(); 

        Vector3 frozenPos = snake.transform.position;

        snake.enabled = false;       // stop logic
        rb.simulated = false;        // stop physics
        col.enabled = false;         // stop collisions

        HardFreeze[] hazards = Layer2Objects.GetComponentsInChildren<HardFreeze>(true);
        foreach (var h in hazards)
            h.Freeze();

        snake.HardFreeze();

        // glitch
        PlayGlitch();

        // delay BEFORE
        yield return new WaitForSeconds(0.1f);

        // activate ly2
        ActivateLayer(layer);

        // delay 4 reaction time
        yield return new WaitForSeconds(0.15f);

        // unfreeze 

        Debug.Log("Unfreezing snake");
        snake.transform.position = frozenPos;
        col.enabled = true;
        rb.simulated = true;
        snake.enabled = true;
        
        Debug.Log("Unfreezing hazards of layer 2");
        foreach (var h in hazards)
            h.Unfreeze();

        Debug.Log("Unfreeze COMPLETE");

        isTrans = false;
    }
}
