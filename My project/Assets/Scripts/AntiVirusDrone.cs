using UnityEngine;
using static UnityEngine.UI.ScrollRect;

public class AntiVirusDrone : MonoBehaviour
{
    public float speed = 2f;
    public float distance = 3f;

    // bounding;
    public float minX = -7.8f;
    public float maxX = 7.8f;
    public float minY = -3.7f;
    public float maxY = 3.7f;

    public bool isFrozen = false;


    private Vector3 starPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        starPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFrozen) return;
        float leftRoom = starPos.x - minX;
        float rightRoom = maxX - starPos.x;
        float allowedX = Mathf.Min(distance, leftRoom, rightRoom);

        float downRoom = starPos.y - minY;
        float upRoom = maxY - starPos.y;
        float allowedY = Mathf.Min(distance, downRoom, upRoom);

        float offset = 0f;


        offset = Mathf.PingPong(Time.time * speed, allowedY * 2) - allowedY;
        transform.position = new Vector3(starPos.x, starPos.y + offset, starPos.z);
    }
    public void ForceReposition(Vector2 newPos)
    {
        transform.position = newPos;
        starPos = newPos;
    }

}
