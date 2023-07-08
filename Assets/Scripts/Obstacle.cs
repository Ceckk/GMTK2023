using UnityEngine;

public class Obstacle : MonoBehaviour
{
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
        pos.x += -GameManager.Instance.GameSpeed * Time.deltaTime;
        transform.position += Vector3.left * GameManager.Instance.GameSpeed * Time.deltaTime;

        if (transform.position.x < _leftEdge)
        {
            Destroy(gameObject);
        }
    }

}
