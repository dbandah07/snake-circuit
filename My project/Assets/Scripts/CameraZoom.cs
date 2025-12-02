using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour
{
    public float targetZoom = 4.5f;       // SMALLER = zoom in
    public float zoomDuration = 1f;     // visible but not too long

    private float startZoom;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        startZoom = cam.orthographicSize;   // should be 5
    
    }

    void Update()
    {
        Debug.Log("Camera size: " + cam.orthographicSize);
    }

    public IEnumerator ZoomIn()
    {
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / zoomDuration;
            cam.orthographicSize = Mathf.Lerp(startZoom, targetZoom, t);
            yield return null;
        }
        cam.orthographicSize = targetZoom;
    }
}

