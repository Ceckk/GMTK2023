using UnityEngine;

public class ScreenAutoRotate : MonoBehaviour
{
    public float powerUp = 10;
    public float powerDown = 1;
    public Transform _rotationTarget;
    private float _totalRotation;
    private float rotMax = 30;
    private float rotMin = -30;

    void Update()
    {
        var rot = 0f;
        if (Input.GetMouseButton(0))
        {
            rot = powerUp * Time.deltaTime;
        }
        else
        {
            rot = -powerDown * Time.deltaTime;
        }

        if (_totalRotation + rot > rotMax)
        {
            rot = rotMax - _totalRotation;
        }
        else if (_totalRotation + rot < rotMin)
        {
            rot = rotMin - _totalRotation;
        }

        transform.RotateAround(_rotationTarget.position, Vector3.forward, rot);
        _totalRotation += rot;
        Debug.Log(_totalRotation);
    }
}
