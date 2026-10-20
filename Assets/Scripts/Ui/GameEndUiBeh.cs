using UnityEngine;

public class GameEndUiBeh : MonoBehaviour
{
    [SerializeField] private GameObject _gameEndCanvas;
    [SerializeField] private SpawnObject _spawner;
    [SerializeField] private TimerController _timer;
   

    private void Start()
    {
        _gameEndCanvas.SetActive(false);
    }

    public void GameEnd()
    {
        _gameEndCanvas.SetActive(true);
    }

    public void OnResturtButtonClick()
    {
        _gameEndCanvas.SetActive(false);
        _timer.ResetTimer();
        _spawner.ClearAll();   
    }

    public void OnExitButtonClick()
    {
        Application.Quit();
    }
}