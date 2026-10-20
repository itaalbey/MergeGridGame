using TMPro;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    [SerializeField] private TMP_Text _currentTimeText;
    [SerializeField] private TMP_Text _bestTimeText;

    private const string BestTimeKey = "BestTime";

    private float _elapsed;
    private bool _isRunning;
    private float _bestTime = -1f;

    public bool IsRunning => _isRunning;

    private void Start()
    {
        _bestTime = PlayerPrefs.GetFloat(BestTimeKey, -1f);

        UpdateCurrentText();
        UpdateBestText();
    }

    private void Update()
    {
        if (!_isRunning) return;

        _elapsed += Time.deltaTime;
        UpdateCurrentText();
    }

    public void StartTimer()
    {
        if (_isRunning) return; 
        _elapsed = 0f;
        _isRunning = true;
        UpdateCurrentText();
    }

    public void StopTimer()
    {
        if (!_isRunning) return;
        _isRunning = false;

        if (_bestTime < 0f || _elapsed < _bestTime)
        {
            _bestTime = _elapsed;
            PlayerPrefs.SetFloat(BestTimeKey, _bestTime);
            PlayerPrefs.Save();
            UpdateBestText();
        }
    }
    public void ResetTimer()
    {
        _isRunning = false;
        _elapsed = 0f;
        UpdateCurrentText();
    }
    private void UpdateCurrentText()
    {
        if (_currentTimeText == null) return;
        _currentTimeText.text = "Current Time  " + FormatTime(_elapsed);
    }

    private void UpdateBestText()
    {
        if (_bestTimeText == null) return;
        _bestTimeText.text = _bestTime < 0f ? " " : "Best Time  " + FormatTime(_bestTime);
    }

    private static string FormatTime(float seconds)
    {
        int totalSec = Mathf.FloorToInt(seconds);
        int minutes = totalSec / 60;
        int secs = totalSec % 60;
        int millis = Mathf.FloorToInt((seconds - totalSec) * 100f);
        return $"{minutes:00}:{secs:00}.{millis:00}";
    }
}