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
            var cost = powerUsage * Time.deltaTime;
            if (GameManager.Instance.powerAmount >= cost)
            {
                var delta = Input.mousePosition - _mousePos;
                var pos = transform.position;
                pos += delta * power * Time.deltaTime;
                transform.position = pos;

                GameManager.Instance.powerAmount -= cost;
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
