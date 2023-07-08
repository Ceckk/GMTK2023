using UnityEngine;

public class ScreenMove : MonoBehaviour
{
    public float power = 3;
    private Vector3 _mousePos;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _mousePos = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            var delta = Input.mousePosition - _mousePos;
            _mousePos = Input.mousePosition;
            var pos = transform.position;
            pos += delta * power * Time.deltaTime;
            transform.position = pos;
        }
    }
}
