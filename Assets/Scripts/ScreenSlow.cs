using UnityEngine;

public class ScreenSlow : MonoBehaviour
{
    public float power = 3;
    public float powerUsage = 5;
    private bool _active;
    public bool usingPower;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _active = true;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            var cost = powerUsage * Time.deltaTime;
            if (_active && GameManager.Instance.powerAmount >= cost)
            {
                GameManager.Instance.speedOffset = -3;
                GameManager.Instance.powerAmount -= cost;
            }
            else
            {
                GameManager.Instance.speedOffset = 0;
                _active = false;
            }

            usingPower = true;
        }
        else
        {
            GameManager.Instance.speedOffset = 0;
            usingPower = false;
        }
    }
}
