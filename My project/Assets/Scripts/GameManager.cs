using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // ui + score + glitch
    public int score = 0;
    public TextMeshProUGUI scoreTXT;
    public TextMeshProUGUI layerTXT;
    public Animator glitchAnim;

    // packets required per layer
    public int[] layerThresholds;
    public int[] packetsNeededPerLayer = { 5, 4, 3, 2 };

    // layer 2
    public MovingFirewall[] Layer2_Firewalls;
    public AntiVirusDrone[] Layer2_Drones;


    // layer 3
    public GameObject Layer3BG;
    public GameObject[] Layer3Prefabs;
    public Transform[] L3HazardPos;
    public Transform Layer3Container;

    // layer 4
    public Transform[] L4HazardPos;
    public Transform[] L4VisualPos;

    // layer 5: BOSS
    public GameObject Layer5Container;
    public GameObject Warning;
    public GameObject Siren;

    public GameObject OrangeBG;
    public ProgressBarAnti bossBar;
    public int bossTargetScore = 10;
    public bool bossRunning = false;

    // AI SNAKE
    public GameObject AISnakePrefab;
    public Transform AISpawnPoint;

    // KEEP SCORE
    public int p_Score= 0;
    public int AI_Score = 0;
    public int goal = 10;

    public PacketSpawner packetSpawner;

    private int currLayer = 1;
    private bool isTrans = false;

    public Transform playerSpawnPoint;

    private bool isRaceOver = false; 

    public RaceCutsceneUI raceCutsceneUI;
    public TextMeshProUGUI aiScoreTXT;

    // Start is called once before the first execution of Update after the MonoBehaviour is created



    void Start()
    {

        if (instance == null) instance = this;
        else Destroy(gameObject);

        layerThresholds = new int[packetsNeededPerLayer.Length];
        int total = 0;
        for (int i = 0; i < packetsNeededPerLayer.Length; i++)
        {
            total += packetsNeededPerLayer[i];
            layerThresholds[i] = total;
        }


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

        if (Layer5Container != null)
            Layer5Container.SetActive(false);
        Warning.SetActive(false);
        Siren.SetActive(false);
        OrangeBG.SetActive(false);
        if (aiScoreTXT != null) aiScoreTXT.gameObject.SetActive(false);

    }

    void Update()
    {
        // DEBUGGING/TESTING
        int i = 0;
        if (Input.GetKeyDown(KeyCode.T))
        {
            AddScore(5 - i);
            i++;
            
        }
    }

    // Update is called once per frame
    public void AddScore(int val)
    {
        score += val;
        scoreTXT.text = "SCORE: " + score;
        int nextLayerIndex = currLayer - 1;

        if (nextLayerIndex < layerThresholds.Length &&
            score >= layerThresholds[nextLayerIndex])
        {
            currLayer++;
            layerTXT.text = "LAYER: " + currLayer;
            StartCoroutine(DoLayerTransition(currLayer));
        }
    }

    public void PlayGlitch()
    {
        glitchAnim.SetTrigger("Flash");
    }


    void RepositionExistingHazards(Transform[] targets)
    {
        int index = 0;

        // move drones FIRST
        foreach (AntiVirusDrone d in Layer2_Drones)
        {
            if (index < targets.Length)
            {
                d.ForceReposition(targets[index].position);
                index++;
            }
        }

        // then firewalls
        foreach (MovingFirewall w in Layer2_Firewalls)
        {
            if (index < targets.Length)
            {
                w.transform.position = targets[index].position;
                w.ResetPosition();
                index++;
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
            {
                fw.gameObject.SetActive(true);
                fw.isFrozen = false;
                fw.ResetPosition();
            }
            foreach (AntiVirusDrone d in Layer2_Drones)
            {
                d.gameObject.SetActive(true);
                d.isFrozen = false;
            }

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

        if (layer == 4)
        {
            PlayGlitch();
            yield return new WaitForSeconds(0.1f);

            // visual rearrangement
            yield return StartCoroutine(GlitchyRearrangeVisuals(L4VisualPos));

            // hazards 
            RepositionExistingHazards(L4HazardPos);

            // zoom
            CameraZoom zoom = Camera.main.GetComponent<CameraZoom>();
            if (zoom != null)
                StartCoroutine(zoom.ZoomIn());
        }

        if (layer == 5)
        {
            //Debug.Log("BOSS LEVEL");
            yield return StartCoroutine(BossLayerTransition());

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

    IEnumerator GlitchyRearrangeVisuals(Transform[] targets)
    {
        int count = Mathf.Min(targets.Length, Layer3Container.childCount);

        // jitter + spin
        float glitchTime = 0.3f;
        float t = 0f;

        while (t < glitchTime)
        {
            t += Time.deltaTime;
            float rot = Random.Range(-25f, 25f);

            foreach (Transform child in Layer3Container)
            {
                child.rotation = Quaternion.Euler(0, 0, rot);
                child.position += (Vector3)Random.insideUnitCircle * 0.06f;
            }

            yield return null;
        }

        // NEW POS
        for (int i = 0; i < count; i++)
        {
            Layer3Container.GetChild(i).position = targets[i].position;
        }

        // SPIN
        float settleTime = 0.25f;
        t = 0f;
        while (t < settleTime)
        {
            t += Time.deltaTime;
            float rot = Mathf.Lerp(35f, 0f, t / settleTime);

            foreach (Transform child in Layer3Container)
            {
                child.rotation = Quaternion.Euler(0, 0, rot);
            }

            yield return null;
        }
    }

    IEnumerator BossLayerTransition()
    {
        // freeze player movement
        SnakeController snake = FindObjectOfType<SnakeController>();
        Rigidbody2D rb = snake.GetComponent<Rigidbody2D>();
        Collider2D col = snake.GetComponent<Collider2D>();

        snake.enabled = false;
        rb.simulated = false;
        col.enabled = false;

        // TRIPLE GLITCH
        for (int i = 0; i < 3; i++)
        {
            PlayGlitch();
            yield return new WaitForSeconds(0.15f);
        }

        OrangeBG.SetActive(true);
        Image bg = OrangeBG.GetComponent<Image>();
        Color c = bg.color;
        c.a = 1f;
        bg.color = c;

        // WARNING FLASH
        Warning.SetActive(true);
        //Debug.Log("WARNING ACTIVE = " + Warning.activeSelf);

        for (int i = 0; i < 4; i++)
        {
            Warning.SetActive(i % 2 == 0);
            yield return new WaitForSeconds(0.1f);
        }
        Warning.SetActive(false);
        //Debug.Log("WARNING ACTIVE = " + Warning.activeSelf);

        // SIREN FLASH
        Siren.SetActive(true);
        for (int i = 0; i < 4; i++)
        {
            Siren.SetActive(i % 2 == 0);
            yield return new WaitForSeconds(0.1f);
        }
        Siren.SetActive(true);
        bossBar.fillBar.fillAmount = 0;
        yield return StartCoroutine(bossBar.PlayScanBar(2f, 0.25f));

        Siren.SetActive(false); // turns off
        bossBar.gameObject.SetActive(false);
        OrangeBG.SetActive(false);

        // DISABLE 
        DisableAllHazardsForBoss();

        // ENABLE BOSS UI
        Layer5Container.SetActive(true);
        bossRunning = true;

        // RACE 
        p_Score = 0;
        AI_Score = 0;
        // hide
        layerTXT.gameObject.SetActive(false);

        // show ai 
        if (aiScoreTXT != null)
        {
            aiScoreTXT.gameObject.SetActive(true);
            aiScoreTXT.text = "AI: " + AI_Score;
        }

        // spawn AI
        if (AISnakePrefab != null)
            Instantiate(AISnakePrefab, AISpawnPoint.position, Quaternion.identity);

        // spawn FIRST packet of race
        packetSpawner.SpawnPacket();

        // UNFREEZE PLAYER
        snake.transform.position = snake.transform.position;
        snake.enabled = true;
        rb.simulated = true;
        col.enabled = true;
        snake.transform.position = playerSpawnPoint.position;

    }
    void DisableAllHazardsForBoss()
    {
        foreach (AntiVirusDrone d in Layer2_Drones)
        {
            d.gameObject.SetActive(false);
        }

        foreach (MovingFirewall fw in Layer2_Firewalls)
        {
            fw.gameObject.SetActive(false);
        }

    }

    public void PlayerCollect_Normal()
    {
        AddScore(1);
        packetSpawner.SpawnPacket();
    }


    public void PlayerCollectRacePacket()
    {
        if (!bossRunning) return;

        p_Score++;

        if (scoreTXT != null) scoreTXT.text = "SCORE: " + (score + p_Score);

        if (p_Score >= goal)
        {
            PlayerWinsRace();
        }
        else
        {
            packetSpawner.SpawnPacket();
        }
    }

    public void AICollectRacePacket()
    {
        if (!bossRunning) return;

        AI_Score++;

        if (aiScoreTXT != null) aiScoreTXT.text = "AI: " + AI_Score;

        if (AI_Score >= goal)
        {
            AIWinsRace();
        }
        else
        {
            packetSpawner.SpawnPacket();
        }
    }

    public void PlayerLoseOneSegment()
    {
        SnakeController snake = FindObjectOfType<SnakeController>();
        snake.LoseOneSegment();
    }

    void PlayerWinsRace()
    {
        if (isRaceOver) return;

        isRaceOver = true;
        Debug.Log("PLAYER WINS!");
        EndRace(true);
        // VICTORY SCREEN
    }

    public void AIWinsRace()
    {
        if(isRaceOver) return;
        isRaceOver = true;
        Debug.Log("AI WINS!");

        EndRace(false);
        // DEFEAT
    }
    public void EndRace(bool playerWin)
    {
        bossRunning = false;
        isRaceOver = true; 

        // destroy ALL packets
        GameObject[] packets = GameObject.FindGameObjectsWithTag("Packet");
        foreach (GameObject p in packets)
            Destroy(p);

        // references
        AISnakeController ai = FindObjectOfType<AISnakeController>();
        SnakeController player = FindObjectOfType<SnakeController>();

        if (playerWin)
        {
            if (ai != null)
            {
                foreach (Transform seg in ai.segments)
                {
                    if (seg != null) Destroy(seg.gameObject);

                    Destroy(seg.gameObject);
                }
                Destroy(ai.gameObject);
            }

            if (player != null) player.enabled = false;

            if (raceCutsceneUI != null) raceCutsceneUI.ShowPlayerWin();
        }
        else
        {
            if (player != null)
            {
                foreach (Transform seg in player.segments)
                {
                    if (seg != null) Destroy(seg.gameObject);

                    Destroy(seg.gameObject);
                }
                Destroy(player.gameObject);
            }

            if (ai != null) ai.enabled = false;

            if (raceCutsceneUI != null) raceCutsceneUI.ShowAIWin();

        }

        // TRIGGER CUT SCENE 

        // if player wins:
        // ERROR could not complete anti virus scan
        // snake w coffee ?? 
        // play again (btn) ?

        // if snake wins: 
        // run anti virus scan bar (again) FILLED up all the way this time. 
        // ANTI VIRUS SCAN COMPLETE
        // GAME OVER, play again (btn) ?
        // Turn off AI race UI
        if (aiScoreTXT != null) aiScoreTXT.gameObject.SetActive(false);
        if (scoreTXT != null) scoreTXT.gameObject.SetActive(false);
        if (layerTXT != null) layerTXT.gameObject.SetActive(false);


    }

}