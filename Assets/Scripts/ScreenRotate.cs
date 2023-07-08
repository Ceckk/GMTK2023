using UnityEngine;

public class ScreenRotate : MonoBehaviour
{
    public float power = 50;
    private Vector3 _mousePos;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mousePos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            var delta = Input.mousePosition - _mousePos;
            _mousePos = Input.mousePosition;
            transform.Rotate(0, 0, delta.y * power * Time.deltaTime);
        }
    }
}
