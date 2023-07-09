using System;
using UnityEngine;

public class ScreenMove : MonoBehaviour
{
    public float power = 3;
    public float powerUsage = 5;
    public bool usingPower;
    public GameObject circle;
    public float circleMoveSensitivity = 0.5f;
    public float worldMoveSensitivity = 2;
    private Vector3 _offset;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _offset = circle.transform.localPosition - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            var lastCirclePos = circle.transform.localPosition;
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var pos = mousePos + _offset;
            pos.z = 0;

            var delta = (pos - circle.transform.localPosition) * circleMoveSensitivity;
            pos = circle.transform.localPosition + delta;
            circle.transform.localPosition = new Vector3(pos.x, pos.y);

            float radius = 0.5f;
            Vector3 centerPosition = Vector3.zero;
            float distance = Vector3.Distance(pos, centerPosition);

            if (distance > radius)
            {
                Vector3 fromOriginToObject = pos - centerPosition;
                fromOriginToObject *= radius / distance;
                pos = centerPosition + fromOriginToObject;
            }

            var delta2 = (pos - lastCirclePos) * worldMoveSensitivity;
            transform.position += delta2;

            circle.transform.localPosition = pos;
            _offset = pos - mousePos;
        }
    }

    public void ResetPositions()
    {
        transform.position = Vector3.zero;
        circle.transform.localPosition = Vector3.zero;
    }
}
