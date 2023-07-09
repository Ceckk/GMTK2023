using UnityEngine;

public class ScreenMove : MonoBehaviour
{
    public float power = 3;
    public float powerUsage = 5;
    private Vector3 _mousePos;
    private bool _active;
    public bool usingPower;

    void Start()
    {
        _mousePos = Input.mousePosition;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mousePos = Input.mousePosition;
            _active = true;
        }

        if (Input.GetMouseButton(0))
        {
            var cost = powerUsage * Time.deltaTime;
            if (_active && GameManager.Instance.powerAmount >= cost)
            {
                var delta = Input.mousePosition - _mousePos;
                var pos = transform.position;
                pos += delta * power * Time.deltaTime;
                transform.position = pos;

                GameManager.Instance.powerAmount -= cost;
                _active = true;
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
