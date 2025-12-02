using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{


    public static GameManager instance;

    // ui + score + glitch
    public int score = 0;
    public TextMeshProUGUI scoreTXT;
    public TextMeshProUGUI layerTXT;
    public Animator glitchAnim;


    // layer 2
    public MovingFirewall[] Layer2_Firewalls;
    public AntiVirusDrone[] Layer2_Drones;


    // layer 3
    public GameObject Layer3BG;
    public GameObject[] Layer3Prefabs;

    public Transform[] L3HazardPos;

    public Transform Layer3Container;


    private int currLayer = 1;
    private bool isTrans = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created



    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        scoreTXT.text = "SCORE: 0";
        layerTXT.text = "LAYER: 1";
        foreach (MovingFirewall fw in Layer2_Firewalls)
            fw.gameObject.SetActive(false);

        // Drones ACTIVE + moving in layer 1
        foreach (AntiVirusDrone d in Layer2_Drones)
            d.gameObject.SetActive(true);

        if (Layer3BG != null)
            Layer3BG.SetActive(false);

        if (Layer3Container != null)
            foreach (Transform t in Layer3Container)
                t.gameObject.SetActive(false);
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


    void RepositionExistingHazards(Transform[] targets)
    {
        int index = 0;

        // move drones FIRST
        foreach (AntiVirusDrone d in Layer2_Drones)
        {
            if (index < targets.Length)
                d.transform.position = targets[index].position;
            index++;
        }

        // then firewalls
        foreach (MovingFirewall w in Layer2_Firewalls)
        {
            if (index < targets.Length)
                w.transform.position = targets[index].position;
            index++;
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

        snake.enabled = false;
        rb.simulated = false;
        col.enabled = false;

        snake.HardFreeze();


        // glitch
        PlayGlitch();
        yield return new WaitForSeconds(0.12f);


        // activate layer;
        if (layer == 2)
        {

            foreach (MovingFirewall fw in Layer2_Firewalls)
                fw.gameObject.SetActive(true);

            foreach (AntiVirusDrone d  in Layer2_Drones)
                d.gameObject.SetActive(true);

        }



        if (layer == 3)
        {
            //foreach (Transform child in Layer2Container)
            //    Destroy(child.gameObject);

            if (Layer3BG != null) Layer3BG.SetActive(true);
            RepositionExistingHazards(L3HazardPos);



            yield return StartCoroutine(SpawnLayer3Hazards());
            //yield return StartCoroutine(SpawnLayer3Obstacles());

            CameraZoom zoom = Camera.main.GetComponent<CameraZoom>();
            if (zoom != null)
                StartCoroutine(zoom.ZoomIn());
        }


        // unfreeze
        snake.transform.position = frozenPos;
        col.enabled = true;
        rb.simulated = true;
        snake.enabled = true;


        isTrans = false;
    }


    // LAYER 3 SPAWN
    IEnumerator SpawnLayer3Hazards()
    {
        foreach (GameObject prefab in Layer3Prefabs)
        {
            GameObject h = Instantiate(prefab, Layer3Container);
            h.transform.SetParent(Layer3Container, worldPositionStays: true);

            HardFreeze hf = h.GetComponent<HardFreeze>();
            if (hf != null) hf.Unfreeze();

            yield return null;
        }
    }

}