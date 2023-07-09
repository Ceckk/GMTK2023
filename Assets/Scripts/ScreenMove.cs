using UnityEngine;

public class ScreenMove : MonoBehaviour
{
    public float power = 3;
    public float powerUsage = 5;
    public bool usingPower;
    public GameObject circle;
    private Vector3 _offset;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + _offset;

            var bounds = OrthographicBounds(Camera.main);
            pos.x = Mathf.Clamp(pos.x, bounds.min.x, bounds.max.x);
            pos.y = Mathf.Clamp(pos.y, bounds.min.y, bounds.max.y);
            transform.position = new Vector3(pos.x, pos.y);
        }
    }

    public Bounds OrthographicBounds(Camera camera)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }
}
