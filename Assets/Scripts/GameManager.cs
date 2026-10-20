using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private TimerController _timer;
    [SerializeField] private GameEndUiBeh _ui;

    public bool GameStarted { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        GameStarted = false;
    }

    public void StartGame()
    {
        _timer.StartTimer();
        GameStarted = true;
    }
    private void EndGame()
    {
        _timer.StopTimer();
        GameStarted = false;
        _ui.GameEnd();

    }
    public void PlayVictory()
    {
        EndGame();
        AudioManager.Instance.PlayVictory();
    }
}
