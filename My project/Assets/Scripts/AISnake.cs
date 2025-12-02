using System.Collections.Generic;
using UnityEngine;

public class AISnakeController : MonoBehaviour
{
    public float moveRate = 0.3f;  // slower (slightly)
    private float moveTimer;

    public Transform segmentPrefab;   // SAME segment prefab
    private List<Transform> segments = new List<Transform>();

    // not growing in right direction...
    private Vector3 lastTailPos;

    private Vector2Int direction = Vector2Int.right;  // default
    private Vector3 lastHeadPos;

    private Transform targetPacket;   // packet

    // debugging, i think ai is too fast, data packets doubke spawn:
    public float thinkDelay = 0.2f;
    private float thinkTimer = 0f;


    void Start()
    {
        segments.Add(this.transform);  // head

        // Check for new packet every 0.2s
        InvokeRepeating(nameof(FindPacket), 0f, 0.2f);
    }

    void FixedUpdate()
    {
        moveTimer += Time.fixedDeltaTime;

        if (moveTimer >= moveRate)
        {
            moveTimer = 0f;
            Move();
        }
    }

    // movement
    void Move()
    {
        if (targetPacket == null) return;

        thinkTimer += moveRate;
        if (thinkTimer >= thinkDelay)
        {
            thinkTimer = 0f;

            // calculate turn
            Vector2 diff = targetPacket.position - transform.position;

            Vector2Int desired;

            if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
                desired = new Vector2Int(diff.x > 0 ? 1 : -1, 0);
            else
                desired = new Vector2Int(0, diff.y > 0 ? 1 : -1);
            if (!(desired.x == -direction.x && desired.y == -direction.y))
                direction = desired;

        }

        lastHeadPos = transform.position;
        transform.position += new Vector3(direction.x, direction.y, 0);

        // keep track of tail
        lastTailPos = segments[segments.Count - 1].position;


        for (int i = 1; i < segments.Count; i++)
        {
            Vector3 temp = segments[i].position;
            segments[i].position = lastHeadPos;
            lastHeadPos = temp;
        }
    }


    // find packet
    void FindPacket()
    {

        if (!GameManager.instance.bossRunning)
        {
            Debug.Log("Finding packet, boss running = FALSE");
            return;
        }

        Debug.Log("Finding packet... bossRunning = TRUE");

        if (targetPacket == null)
        {
            GameObject p = GameObject.FindWithTag("Packet");
            if (p != null)
            {
                targetPacket = p.transform;
                Debug.Log("AI Found packet at pos: " + targetPacket.position);
            }
            else
            {
                Debug.Log("NO PACKET FOUND");
            }
        }
    }


    // grow - same as player
    public void Grow()
    {
        Transform newSeg = Instantiate(segmentPrefab);
        newSeg.position = lastTailPos; 
        newSeg.tag = "AISegment";
        segments.Add(newSeg);
    }

    // collision
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!GameManager.instance.bossRunning) return;

        if (other.CompareTag("Packet"))
        {
            Destroy(other.gameObject);

            // grow like player
            Grow();

            // score for AI
            GameManager.instance.AICollectRacePacket();
            targetPacket = null;
        }
        else if (other.CompareTag("PlayerSegment"))
        {
            // hit player = player loses segment
            GameManager.instance.PlayerLoseOneSegment();
        }
    }
}

