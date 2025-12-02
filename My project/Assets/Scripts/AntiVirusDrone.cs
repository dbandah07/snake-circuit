using UnityEngine;

public class AntiVirusDrone : MonoBehaviour
{
    public float speed = 2f;
    public float distance = 3f;

    public bool isFrozen = false;

    public enum MovementType { horiz, vert }
    public MovementType movementType = MovementType.horiz;


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

        float offset = Mathf.PingPong(Time.time * speed, distance * 2) - distance;
        if (movementType == MovementType.horiz)
        {
            transform.position = new Vector3(starPos.x + offset, starPos.y, starPos.z); // hori hazard
        }
        else
        {
            transform.position = new Vector3(starPos.x, starPos.y + offset, starPos.z); // vertical hazard
        }
    }
    public void ForceReposition(Vector2 newPos)
    {
        transform.position = newPos;
        starPos = newPos;
    }

}
