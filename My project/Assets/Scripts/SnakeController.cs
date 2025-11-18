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

    private List<Transform> segments = new List<Transform>();

    private Vector3 lastHeadPos;

    private bool isDead = false;
    private bool hasStarted = false;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        segments = new List<Transform>();
        segments.Add(this.transform); // first seg = head;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasStarted)
        {
            if (Input.GetKeyDown(KeyCode.W)) { direction = Vector2Int.up; hasStarted = true; }
            if (Input.GetKeyDown(KeyCode.S)) { direction = Vector2Int.down; hasStarted = true; }
            if (Input.GetKeyDown(KeyCode.A)) { direction = Vector2Int.left; hasStarted = true; }
            if (Input.GetKeyDown(KeyCode.D)) { direction = Vector2Int.right; hasStarted = true; }

            return; // do NOT allow movement until input
        }

        // normal controls
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
            //Grow();
            //Destroy(collision.gameObject);

            //FindObjectOfType<PacketSpawner>().SpawnSinglePacket();
            //GameManager.instance.PlayGlitch();
            StartCoroutine(HandlePacketPickup(collision.gameObject));
            return;
        }

        else if (collision.CompareTag("Wall") || collision.CompareTag("Enemy"))
        {
            isDead = true; // free logic
            enabled = false; // stop movement, snake don't tweak (pls)

            GameManager.instance.PlayGlitch();


            AntiVirusDrone drone = collision.GetComponent<AntiVirusDrone>();
            if (drone != null) drone.isFrozen = true;


            StartCoroutine(DeathSequence());
        }
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
        GetComponent<Collider2D>().enabled = false;

        Grow();
        GameManager.instance.AddScore(1);

        Destroy(packet);
        FindObjectOfType<PacketSpawner>().SpawnSinglePacket();

        yield return new WaitForFixedUpdate();

        GetComponent<Collider2D>().enabled = true;
    }

}
