using UnityEngine;

public class CellView : MonoBehaviour
{
    public ObjectBeh Item { get; private set; }
    public bool IsEmpty => Item == null;

    public void SetItem(ObjectBeh item)
    {
        Item = item;
        var rt = (RectTransform)item.transform;
        rt.SetParent(transform, false);
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    public void ClearItem()
    {
        Item = null;
    }
}