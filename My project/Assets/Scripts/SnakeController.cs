using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeController : MonoBehaviour
{
    // store direction 
    // move snake forward
    // store body seg pos
    // add seg when collecting packets
    // detect collisions

    public float moveRate = 0.15f; // time between moves
    private float moveTimer;

    public Vector2Int direction = Vector2Int.zero; // starting direction

    public Transform segmentPrefab;

    public List<Transform> segments = new List<Transform>();

    private Vector3 lastHeadPos;

    private bool isDead = false;
    private bool hasStarted = false;

    private Vector2 touchStartPos;
    private bool touchMoved = false;
    public float minSwipeDistance = 50f;
    private AudioSource audioSource;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        segments = new List<Transform>();
        segments.Add(this.transform); // first seg = head;
    }

    // Update is called once per frame
    void Update()
    {

        HandleTouchControls();

        if (!hasStarted)
        {
            if (Input.GetKeyDown(KeyCode.W)) { direction = Vector2Int.up; hasStarted = true; }
            if (Input.GetKeyDown(KeyCode.S)) { direction = Vector2Int.down; hasStarted = true; }
            if (Input.GetKeyDown(KeyCode.A)) { direction = Vector2Int.left; hasStarted = true; }
            if (Input.GetKeyDown(KeyCode.D)) { direction = Vector2Int.right; hasStarted = true; }

            return; // do NOT allow movement until input
        }

        // KEYBOARD CONTROLS
        if (Input.GetKeyDown(KeyCode.W) && direction != Vector2Int.down) direction = Vector2Int.up;
        else if (Input.GetKeyDown(KeyCode.S) && direction != Vector2Int.up) direction = Vector2Int.down;
        else if (Input.GetKeyDown(KeyCode.A) && direction != Vector2Int.right) direction = Vector2Int.left;
        else if (Input.GetKeyDown(KeyCode.D) && direction != Vector2Int.left) direction = Vector2Int.right;
    }

    private void FixedUpdate()
    {
        moveTimer += Time.fixedDeltaTime;

        if (moveTimer >= moveRate)
        {
            moveTimer = 0f;
            Move();
        }
    }
    void Move()
    {
        if (!hasStarted) return;
        if (direction == Vector2Int.zero) return;

        lastHeadPos = transform.position;

        // Move head
        Vector3 newPos = transform.position + new Vector3(direction.x, direction.y, 0);
        transform.position = newPos;

        // Move tail segments
        for (int i = 1; i < segments.Count; i++)
        {
            Vector3 tempPos = segments[i].position;
            segments[i].position = lastHeadPos;
            lastHeadPos = tempPos;
        }
    }

    public void Grow()
    {
        Transform newSeg = Instantiate(segmentPrefab);
        newSeg.position = segments[segments.Count - 1].position;
        segments.Add(newSeg);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log("Collided w " + collision.name + " Tag is " + collision.tag + " Layer is " + LayerMask.LayerToName(collision.gameObject.layer));

        if (isDead) return;

        if (collision.CompareTag("Packet"))
        {
            StartCoroutine(HandlePacketPickup(collision.gameObject));
            return;
        }

        else if (collision.CompareTag("Wall") || collision.CompareTag("Enemy"))
        {
            //Debug.Log("DEATH TRIGGERED BY: " + collision.name + " TAG: " + collision.tag + " LAYER: " + LayerMask.LayerToName(collision.gameObject.layer));

            isDead = true; // free logic
            enabled = false; // stop movement, snake don't tweak (pls)

            GameManager.instance.PlayGlitch();


            AntiVirusDrone drone = collision.GetComponent<AntiVirusDrone>();
            if (drone != null) drone.isFrozen = true;


            StartCoroutine(DeathSequence());
        }
        else if (collision.CompareTag("AISegment") || collision.CompareTag("AISnakeHead"))
        {
            if (segments.Count <= 1)
            {
                GameManager.instance.EndRace(false);
                return;
            }
            GameManager.instance.PlayerLoseOneSegment();
        }
    }

    void HandleTouchControls()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
                touchMoved = false;
            }

            else if (touch.phase == TouchPhase.Moved)
            {
                touchMoved = true;
            }

            else if (touch.phase == TouchPhase.Ended && touchMoved)
            {
                Vector2 touchEndPos = touch.position;
                Vector2 swipe = touchEndPos - touchStartPos;

                if (swipe.magnitude < minSwipeDistance)
                    return;  

                // horizontal swipe
                if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
                {
                    if (swipe.x > 0 && direction != Vector2Int.left)
                        direction = Vector2Int.right;
                    else if (swipe.x < 0 && direction != Vector2Int.right)
                        direction = Vector2Int.left;
                }
                else // vertical swipe
                {
                    if (swipe.y > 0 && direction != Vector2Int.down)
                        direction = Vector2Int.up;
                    else if (swipe.y < 0 && direction != Vector2Int.up)
                        direction = Vector2Int.down;
                }

                hasStarted = true; 
            }
        }
    }
    public void HardFreeze()
    {
        moveTimer = 0f;
        direction = Vector2Int.zero;
    }



    IEnumerator DeathSequence()
    {
        GameManager.instance.PlayGlitch();

        yield return new WaitForSeconds(0.2f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
            );
    }
    IEnumerator HandlePacketPickup(GameObject packet)
    {
        if (audioSource != null) audioSource.Play();
        Destroy(packet);
        if (GameManager.instance.bossRunning)
        {
            Grow();
            GameManager.instance.PlayerCollectRacePacket();
        }
        else
        {
            Grow();
            GameManager.instance.PlayerCollect_Normal();
        }
        yield return null;
    }
    public void LoseOneSegment()
    {
        // if player only has head, they die
        if (segments.Count <= 1)
        {
            Destroy(gameObject);
            GameManager.instance.AIWinsRace();
            return;
        }


        // otherwise remove segment 
        Transform lastSeg = segments[segments.Count - 1];
        segments.RemoveAt(segments.Count - 1);
        Destroy(lastSeg.gameObject);
    }

}
