using UnityEngine;

public class ScreenRotate : MonoBehaviour
{
    public float power = 50;
    public float powerUsage = 5;
    private Vector3 _mousePos;
    public bool usingPower;
    public Transform _rotationTarget;

    void Start()
    {
        _mousePos = Input.mousePosition;
    }

    void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     _mousePos = Input.mousePosition;
        // }

        // if (Input.GetMouseButton(0))
        {
            var cost = powerUsage * Time.deltaTime;
            // if (GameManager.Instance.powerAmount >= cost)
            {
                var delta = Input.mousePosition - _mousePos;
                transform.RotateAround(_rotationTarget.position, Vector3.forward, delta.y * power * Time.deltaTime);

                GameManager.Instance.powerAmount -= cost;
            }

            _mousePos = Input.mousePosition;
            usingPower = true;
        }
        // else
        // {
        //     usingPower = false;
        // }
    }
}
