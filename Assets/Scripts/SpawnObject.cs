using System.Linq;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private GameObject _cellsParent;
    [SerializeField] private ObjectPool _pool;
    [SerializeField] private RectTransform _dragLayer;

    private CellView[] _cells;

    private void Awake()
    {
        _cells = _cellsParent.GetComponentsInChildren<CellView>().ToArray();
    }

    public void OnSpawnButtonClick()
    {
        var free = System.Array.FindAll(_cells, c => c.IsEmpty);
        if (free.Length == 0)
        {
            return;
        }

        var cell = free[Random.Range(0, free.Length)];

        var go = _pool.GetObject(_itemPrefab, _cellsParent.transform);

        go.GetComponent<ObjectDragHandler>().Setup(_dragLayer, _pool);
        go.GetComponent<ObjectBeh>().Initialize(0);

        cell.SetItem(go.GetComponent<ObjectBeh>());

        AudioManager.Instance.PlaySpawn();
    }

    public void ClearAll()
    {
        foreach (var cell in _cells)
            cell.ClearItem();

        _pool.ReturnAll();
    }
}