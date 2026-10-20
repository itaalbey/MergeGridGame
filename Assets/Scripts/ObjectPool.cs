using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private readonly Dictionary<GameObject, Queue<GameObject>> _pool = new();
    private readonly Dictionary<GameObject, GameObject> _instanceToPrefab = new();
    private readonly HashSet<GameObject> _active = new();

    public GameObject GetObject(GameObject prefab, Transform parent = null)
    {
        if (!_pool.TryGetValue(prefab, out Queue<GameObject> queue))
        {
            queue = new Queue<GameObject>();
            _pool[prefab] = queue;
        }

        GameObject obj;
        if (queue.Count > 0)
        {
            obj = queue.Dequeue();
        }
        else
        {
            obj = Instantiate(prefab, transform);
            _instanceToPrefab[obj] = prefab;
        }

        obj.transform.SetParent(parent, false);
        obj.SetActive(true);
        _active.Add(obj);
        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        if (obj == null) return;

        if (!_instanceToPrefab.TryGetValue(obj, out GameObject prefab))
        {
            Destroy(obj);
            return;
        }

        _active.Remove(obj);
        obj.SetActive(false);
        obj.transform.SetParent(transform, false);
        _pool[prefab].Enqueue(obj);
    }

    public void ReturnAll()
    {
        var snapshot = new List<GameObject>(_active);
        _active.Clear();

        foreach (var obj in snapshot)
        {
            if (obj == null) continue;

            if (!_instanceToPrefab.TryGetValue(obj, out GameObject prefab))
            {
                Destroy(obj);
                continue;
            }

            obj.SetActive(false);
            obj.transform.SetParent(transform, false);
            _pool[prefab].Enqueue(obj);
        }
    }
}