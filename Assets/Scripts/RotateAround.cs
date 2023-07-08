using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public float pushPower = 100;
    public float cooldown = 0.1f;
    private float _power;
    private float _time = 0;
    private float _totalRot;

    void Update()
    {
        _time -= Time.deltaTime;
        if (_time <= 0)
        {
            _power = -_totalRot;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _time = cooldown;
            _power = pushPower;
        }

        if (Input.GetMouseButtonDown(1))
        {
            _time = cooldown;
            _power = -pushPower;
        }

        var rot = _power * Time.deltaTime;
        _totalRot += rot;
        transform.RotateAround(Player.Instance.transform.position, Vector3.forward, rot);
    }
}
