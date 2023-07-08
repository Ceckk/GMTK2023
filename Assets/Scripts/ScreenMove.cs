using UnityEngine;

public class ScreenMove : MonoBehaviour
{
    public float pushPower = 50;
    public float pullPower = 25;
    public float cooldown = 0.1f;
    private Vector3 _mousePos;
    private Vector3 _target;
    private float _power;
    private float _time = 0;

    void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     _mousePos = Input.mousePosition;
        // }
        // if (Input.GetMouseButton(0))
        // {
        //     var delta = _mousePos - Input.mousePosition;
        //     _mousePos = Input.mousePosition;
        //     transform.Rotate(0, 0, delta.y * power * Time.deltaTime);
        // }

        _time -= Time.deltaTime;
        if (_time <= 0)
        {
            _target = Vector3.zero;
            _power = pullPower;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _target = transform.position + Vector3.up * pushPower;
            _time = cooldown;
            _power = pushPower;
        }

        if (Input.GetMouseButtonDown(1))
        {
            _target = transform.position - Vector3.up * pushPower;
            _time = cooldown;
            _power = pushPower;
        }

        transform.position = Vector3.MoveTowards(transform.position, _target, _power * Time.deltaTime);
    }
}
