using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObjectDragHandler : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private float _dragScaleMultiplier = 1.35f;
    [SerializeField] private float _scaleDuration = 0.1f;
    [SerializeField] private float _returnDuration = 0.2f;
    [SerializeField] public RectTransform _dragLayer;

    [SerializeField] private ObjectPool _pool;

    private ObjectBeh _beh;
    private Image _image;
    private RectTransform _rt;
    private CellView _originCell;
    private Vector3 _baseScale = new Vector3(0.5f, 0.5f,0.5f);

    private void Awake()
    {
        _beh = GetComponent<ObjectBeh>();
        _image = GetComponent<Image>();
        _rt = (RectTransform)transform;
    }
    public void Setup(RectTransform dragLayer, ObjectPool pool)
    {
        _dragLayer = dragLayer;
        _pool = pool;
    }
    public void OnBeginDrag(PointerEventData e)
    {
        _originCell = GetComponentInParent<CellView>();
        if (_originCell != null) _originCell.ClearItem();

        Vector2 size = _rt.rect.size;

        _rt.SetParent(_dragLayer, worldPositionStays: true);
        _rt.SetAsLastSibling();

        _rt.anchorMin = new Vector2(0.5f, 0.5f);
        _rt.anchorMax = new Vector2(0.5f, 0.5f);
        _rt.pivot = new Vector2(0.5f, 0.5f);
        _rt.sizeDelta = size;

        _image.raycastTarget = false;

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _dragLayer, e.position, e.pressEventCamera, out localPos);
        _rt.localPosition = localPos;

        StartCoroutine(ScaleRoutine(_baseScale * _dragScaleMultiplier));
        var canvas = _dragLayer.GetComponentInParent<Canvas>();
    }

    public void OnDrag(PointerEventData e)
    {
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_dragLayer, e.position, e.pressEventCamera, out localPos);
        _rt.localPosition = localPos;
    }
    public void OnEndDrag(PointerEventData e)
    {
        _image.raycastTarget = true;

        var targetCell = FindCellUnder(e.position);
        bool placed = TryMergeOrPlace(targetCell);

        if (!placed && _originCell != null)
            _originCell.SetItem(_beh);

        StartCoroutine(ScaleRoutine(_baseScale));
    }

    private bool TryMergeOrPlace(CellView cell)
    {
        if (cell == null) return false;

        if (cell.IsEmpty)
        {
            cell.SetItem(_beh);
            return true;
        }

        if (_beh.CanMergeWith(cell.Item))
        {
            var victim = cell.Item;
            cell.ClearItem();

            _pool.ReturnObject(victim.gameObject);

            _beh.Initialize(_beh.CurrentLvl + 1);
            cell.SetItem(_beh);

            AudioManager.Instance?.PlayMerge();
            return true;
        }

        return false;
    }

    private CellView FindCellUnder(Vector2 screenPos)
    {
        var results = new System.Collections.Generic.List<RaycastResult>();
        var ped = new PointerEventData(EventSystem.current) { position = screenPos };
        EventSystem.current.RaycastAll(ped, results);
        foreach (var r in results)
        {
            var cell = r.gameObject.GetComponentInParent<CellView>();
            if (cell != null) return cell;
        }
        return null;
    }

    private IEnumerator ScaleRoutine(Vector3 target)
    {
        var start = transform.localScale;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / _scaleDuration;
            transform.localScale = Vector3.Lerp(start, target, t);
            yield return null;
        }
        transform.localScale = target;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        if (_image != null) _image.raycastTarget = true;
        if (_rt != null)
        {
            _rt.anchorMin = _rt.anchorMax = new Vector2(0.5f, 0.5f);
            _rt.pivot = new Vector2(0.5f, 0.5f);
            transform.localScale = _baseScale;
        }
    }
}