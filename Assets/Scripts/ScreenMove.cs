using UnityEngine;

public class ScreenMove : MonoBehaviour
{
    public float power = 3;
    public float powerUsage = 5;
    private Vector3 _mousePos;
    public bool usingPower;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _mousePos = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            if (GameManager.Instance.powerAmount > 0)
            {
                var delta = Input.mousePosition - _mousePos;
                var pos = transform.position;
                pos += delta * power * Time.deltaTime;
                transform.position = pos;

                GameManager.Instance.powerAmount -= powerUsage * Time.deltaTime;
            }

            _mousePos = Input.mousePosition;
            usingPower = true;
        }
        else
        {
            usingPower = false;
        }
    }
}
