using UnityEngine;

public class ScreenMove : MonoBehaviour
{
    public float power = 10;
    private Vector3 _mousePos;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mousePos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            var delta = _mousePos - Input.mousePosition;
            _mousePos = Input.mousePosition;
            transform.Rotate(0, 0, delta.y * power * Time.deltaTime);
        }
    }


}
