using UnityEngine;

public class ScreenSlow : MonoBehaviour
{
    public float power = 3;
    public float powerUsage = 5;
    private Vector3 _mousePos;
    private bool _mouseDown;
    public bool usingPower;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _mousePos = Input.mousePosition;
            _mouseDown = true;
        }

        if (Input.GetMouseButton(1))
        {
            var cost = powerUsage * Time.deltaTime;
            if (_mouseDown && GameManager.Instance.powerAmount >= cost)
            {
                GameManager.Instance.speedOffset = -3;
                GameManager.Instance.powerAmount -= cost;
            }
            else
            {
                _mouseDown = false;
            }

            _mousePos = Input.mousePosition;
            usingPower = true;
        }
        else
        {
            GameManager.Instance.speedOffset = 0;
            usingPower = false;
        }
    }
}
