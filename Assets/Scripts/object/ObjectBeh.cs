using UnityEngine;
using UnityEngine.UI;

public class ObjectBeh : MonoBehaviour
{
    [SerializeField] private ObjectSO _objSO;
    [SerializeField] private Image _image;

    public int CurrentLvl { get; private set; }
    public ObjectSO Chain => _objSO;
    public bool IsMaxLevel => _objSO != null && CurrentLvl >= _objSO.ObjectsLvlUps.Count - 1;

    

    public void Initialize(int lvl)
    {
        if (_objSO == null) 
        { 
            return; 
        }

        if (lvl < 0 || lvl >= _objSO.ObjectsLvlUps.Count)
        {
            return;
        }

        CurrentLvl = lvl;

        if (GameManager.Instance.GameStarted == false && CurrentLvl == 1)
        {
            GameManager.Instance.StartGame();
        }

        if(IsMaxLevel)
        {
            GameManager.Instance.PlayVictory();
        }

        _image.sprite = _objSO.ObjectsLvlUps[lvl].Icon;
    }

    public bool CanMergeWith(ObjectBeh other)
    {
        if (other == null || other == this) return false;
        if (IsMaxLevel)
        {
            return false;
        }
        return other._objSO == _objSO && other.CurrentLvl == CurrentLvl;
    }
}