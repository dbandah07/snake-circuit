using UnityEngine;

public class MovingFirewall : MonoBehaviour
{
    public float speed = 3f;
    public float distance = 2f;

    public bool isFrozen = true; 

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (isFrozen) return; 

        float offset = Mathf.PingPong(Time.time * speed, distance * 2) - distance;
        transform.position = new Vector3(startPos.x + offset, startPos.y, startPos.z);
    }

}
