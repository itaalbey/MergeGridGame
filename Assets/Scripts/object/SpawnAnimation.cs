using System.Collections;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SpawnAnimation : MonoBehaviour
{
    [SerializeField] private float _duration = 0.25f;
    [SerializeField] private float _overshoot = 1.25f;
    [SerializeField] private float _startRotation = -12f;
    [SerializeField] private float _targetScale = 0.5f;

    private RectTransform _rt;
    private Coroutine _routine;

    public float TargetScale => _targetScale;

    private void Awake()
    {
        _rt = (RectTransform)transform;
    }

    private void OnEnable()
    {
        Play();
    }

    public void Play()
    {
        if (_routine != null) StopCoroutine(_routine);
        _routine = StartCoroutine(PlayRoutine());
    }

    public void Stop()
    {
        if (_routine != null) { StopCoroutine(_routine); _routine = null; }
        _rt.localScale = Vector3.one * _targetScale;
        _rt.localRotation = Quaternion.identity;
    }

    private IEnumerator PlayRoutine()
    {
        _rt.localScale = Vector3.zero;
        _rt.localRotation = Quaternion.Euler(0, 0, _startRotation);

        float peak = _targetScale * _overshoot;
        float half = _duration * 0.5f;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / half;
            float s = Mathf.Lerp(0f, peak, Mathf.SmoothStep(0f, 1f, t));
            _rt.localScale = Vector3.one * s;
            _rt.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(_startRotation, 0f, t));
            yield return null;
        }

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / half;
            float s = Mathf.Lerp(peak, _targetScale, Mathf.SmoothStep(0f, 1f, t));
            _rt.localScale = Vector3.one * s;
            yield return null;
        }

        _rt.localScale = Vector3.one * _targetScale;
        _rt.localRotation = Quaternion.identity;
        _routine = null;
    }

    private void OnDisable()
    {
        if (_routine != null) { StopCoroutine(_routine); _routine = null; }
        _rt.localScale = Vector3.one * _targetScale;
        _rt.localRotation = Quaternion.identity;
    }
}