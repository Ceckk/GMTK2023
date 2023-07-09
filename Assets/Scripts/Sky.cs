using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Sky : MonoBehaviour
{
    public float offset = 0.05f;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        float speed = GameManager.Instance.GameSpeed / transform.localScale.x + offset;
        meshRenderer.material.mainTextureOffset += Vector2.right * speed * Time.deltaTime;
    }

}
