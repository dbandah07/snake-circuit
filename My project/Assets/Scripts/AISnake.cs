using System.Collections.Generic;
using UnityEngine;

public class AISnakeController : MonoBehaviour
{
    public float moveRate = 0.2f;  // slower (slightly)
    private float moveTimer;

    public Transform segmentPrefab;   // SAME segment prefab
    public List<Transform> segments = new List<Transform>();

    // not growing in right direction...
    private Vector3 lastTailPos;

    private Vector2Int direction = Vector2Int.zero;
    private Vector3 lastHeadPos;

    private Transform targetPacket;   // packet

    // debugging, i think ai is too fast, data packets doubke spawn:
    public float thinkDelay = 0.5f;
    private float thinkTimer = 0f;


    void Start()
    {
        segments.Add(this.transform);  // head

        // check less often, due to logs 
        InvokeRepeating(nameof(FindPacket), 0.25f, 0.2f);
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
        if (!GameManager.instance.bossRunning) return;

        if (targetPacket == null) return;

        thinkTimer += moveRate;
        if (thinkTimer >= thinkDelay)
        {
            thinkTimer = 0f;

            Vector2 diff = targetPacket.position - transform.position;

            if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
            {
                direction = new Vector2Int(diff.x > 0 ? 1 : -1, 0);
            }
            else
            {
                direction = new Vector2Int(0, diff.y > 0 ? 1 : -1);
            }
        }
        lastHeadPos = transform.position;
        Vector3 newPos = transform.position + new Vector3(direction.x, direction.y, 0);
        transform.position = newPos;

        // move body
        for (int i = 1; i < segments.Count; i++)
        {
            Vector3 temp = segments[i].position;
            segments[i].position = lastHeadPos;
            lastHeadPos = temp;
        }
        lastTailPos = lastHeadPos;
}



    // find packet
    void FindPacket()
    {
        if (!GameManager.instance.bossRunning) return;

        GameObject[] packets = GameObject.FindGameObjectsWithTag("Packet");
        if (packets.Length == 0) return;

        Transform closest = packets[0].transform;
        float bestDist = Vector2.Distance(transform.position, closest.position);

        for (int i = 1; i < packets.Length; i++)
        {
            float d = Vector2.Distance(transform.position, packets[i].transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                closest = packets[i].transform;
            }
        }

        targetPacket = closest;
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

