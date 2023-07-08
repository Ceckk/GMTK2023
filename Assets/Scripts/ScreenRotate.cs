using UnityEngine;

public class ScreenRotate : MonoBehaviour
{
    public float power = 50;
    public float powerUsage = 5;
    private Vector3 _mousePos;
    public bool usingPower;
    public Transform _rotationTarget;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mousePos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            if (GameManager.Instance.powerAmount > 0)
            {
                var delta = Input.mousePosition - _mousePos;
                transform.RotateAround(_rotationTarget.position, Vector3.forward, delta.y * power * Time.deltaTime);

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
