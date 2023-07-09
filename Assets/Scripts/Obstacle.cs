using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Vector2 speed;
    private float pushPower = 20;
    private float pullPower = 2;

    private float _leftEdge;
    private float _originalY;
    private float _time;
    private float cooldown = 0.1f;
    private float _offsetY;

    private void Start()
    {
        _leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 2f;
        _originalY = transform.position.y;
    }

    private void Update()
    {
        // _time -= Time.deltaTime;
        // if (_time <= 0)
        // {
        //     _offsetY = (_originalY - transform.position.y) * pullPower;
        // }

        // // if (Input.GetMouseButtonDown(0))
        // if (Input.GetKeyDown(KeyCode.W))
        // {
        //     _time = cooldown;
        //     _offsetY = pushPower;
        // }

        // // if (Input.GetMouseButtonDown(1))
        // if (Input.GetKeyDown(KeyCode.S))
        // {
        //     _time = cooldown;
        //     _offsetY = -pushPower;
        // }

        var pos = transform.position;
        pos.x += (speed.x - GameManager.Instance.GameSpeed) * Time.deltaTime;
        pos.y += (_offsetY + speed.y) * Time.deltaTime;
        _originalY += speed.y * Time.deltaTime;
        transform.position = pos;
        transform.rotation = Quaternion.identity;

        if (transform.position.x < _leftEdge)
        {
            Despawn();
        }

    }

    protected virtual void Despawn()
    {
        Destroy(gameObject);
    }
}
