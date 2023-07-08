using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Vector2 speed;

    private float _leftEdge;
    private float _originalY;

    private void Start()
    {
        _leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 2f;
        _originalY = transform.position.y;
    }

    private void Update()
    {
        var pos = transform.position;
        pos.x += (speed.x - GameManager.Instance.GameSpeed) * Time.deltaTime;
        pos.y += speed.y * Time.deltaTime;
        transform.position = pos;

        if (transform.position.x < _leftEdge)
        {
            Destroy(gameObject);
        }
    }

}
