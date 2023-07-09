using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform _camTransform;

    [SerializeField] private float _defaultShakeDuration = 1f;
    [SerializeField] private float _defaultShakeAmount = 0.7f;

    private float _shakeDuration = 0f;
    private float _shakeAmount = 0f;

    private Vector3 _originalPos;

    void Awake()
    {
        _camTransform = GetComponent(typeof(Transform)) as Transform;
        enabled = false;
    }

    void OnEnable()
    {
        _originalPos = _camTransform.localPosition;
    }

    void Update()
    {
        if (_shakeDuration > 0)
        {
            _camTransform.localPosition = _originalPos + Random.insideUnitSphere * _shakeAmount;

            _shakeDuration -= Time.deltaTime;
        }
        else
        {
            _shakeDuration = 0f;
            _camTransform.localPosition = _originalPos;
            enabled = false;
        }
    }

    public void Shake(float? amount = null, float? duration = null)
    {
        _shakeAmount = amount != null ? (float)amount : _defaultShakeAmount;
        _shakeDuration = duration != null ? (float)duration : _defaultShakeDuration;

        enabled = true;
    }

    public IEnumerator ShakeCoroutine(float? amount = null, float? duration = null)
    {
        Shake(amount, duration);
        yield return new WaitForSeconds(duration != null ? (float)duration : _defaultShakeDuration);
    }
}
