using UnityEngine;

public class ScreenAutoRotate : MonoBehaviour
{
    public float powerUp = 10;
    public float powerDown = 1;
    public Transform _rotationTarget;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            transform.RotateAround(_rotationTarget.position, Vector3.forward, powerUp * Time.deltaTime);
        }
        else
        {
            transform.RotateAround(_rotationTarget.position, Vector3.forward, -powerDown * Time.deltaTime);
        }
    }
}
