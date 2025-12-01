using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;
using System.Collections;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{


    public static GameManager instance;

    // ui + score + glitch
    public int score = 0;
    public TextMeshProUGUI scoreTXT;
    public TextMeshProUGUI layerTXT;
    public Animator glitchAnim;


    // layer 2
    public GameObject[] Layer2Prefabs;
    public Transform Layer2Container;

    // layer 3
    public GameObject Layer3BG; 
    public GameObject[] Layer3Prefabs;
    public Transform Layer3Container;
    private bool layer3Active = false;

    private int currLayer = 1;
    private bool isTrans = false; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    


    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        scoreTXT.text = "SCORE: 0";
        layerTXT.text = "LAYER: 1";

        if (Layer2Container != null) {
            foreach (Transform child in Layer2Container) child.gameObject.SetActive(false);
        }

        if (Layer3Container != null) {
            foreach (Transform child in Layer3Container) child.gameObject.SetActive(false);

        }
        if (Layer3BG != null) Layer3BG.SetActive(false);
    }

     void Update()
    {
        // DEBUGGING/TESTING

        Debug.Log("Adding 5 to score...");
        if (Input.GetKeyDown(KeyCode.T))
        {
            AddScore(5);
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

    //void ActivateLayer(int layer)
    //{
    //    if (layer == 2)
    //    {
    //        if (Layer2Objects != null)
    //        {
    //            Layer2Objects.SetActive(true);
    //        }
    //    }
    //}

    IEnumerator DoLayerTransition(int layer)
    {
        if (isTrans) yield break;
        isTrans = true;

        // freeze snake
        SnakeController snake = FindObjectOfType<SnakeController>();
        Rigidbody2D rb = snake.GetComponent<Rigidbody2D>();
        Collider2D col = snake.GetComponent<Collider2D>();

        Vector3 frozenPos = snake.transform.position;

        snake.enabled = false;
        rb.simulated = false;
        col.enabled = false;

        snake.HardFreeze(); 


        // glitch
        PlayGlitch();
        yield return new WaitForSeconds(0.12f);


        // activate layer;
        if (layer == 2)
            yield return StartCoroutine(SpawnLayer2Hazards());

        if (layer == 3)
        {
            if (Layer3BG != null) Layer3BG.SetActive(true);
            layer3Active = true;
            yield return StartCoroutine(SpawnLayer3Hazards());
        }


        // unfreeze
        snake.transform.position = frozenPos;
        col.enabled = true;
        rb.simulated = true;
        snake.enabled = true;


        isTrans = false;
    }


    // LAYER 2 SPAWN
    IEnumerator SpawnLayer2Hazards()
    {
        foreach (GameObject prefab in Layer2Prefabs)
        {
            GameObject h = Instantiate(prefab, Layer2Container);

            HardFreeze hf = h.GetComponent<HardFreeze>();
            if (hf != null) hf.Unfreeze();

            yield return null;
        }
    }


    // LAYER 3 SPAWN
    IEnumerator SpawnLayer3Hazards()
    {
        foreach (GameObject prefab in Layer3Prefabs)
        {
            GameObject h = Instantiate(prefab, Layer3Container);

            HardFreeze hf = h.GetComponent<HardFreeze>();
            if (hf != null) hf.Unfreeze();

            yield return null;
        }
    }
}