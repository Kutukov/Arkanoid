
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
public enum GameState { Menu, Playing, Paused, GameOver }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState State { get; private set; } = GameState.Menu;

    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform ballParent;
    [SerializeField] private int lives = 3;
    [SerializeField] private Vector2 spawnBallLocation = new Vector2(0, 0f);
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private int score = 0;
    private Ball currentBall;
    private ObjectPool<Ball> ballPool;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;

        ballPool = new ObjectPool<Ball>(ballPrefab.GetComponent<Ball>(), ballParent, 1);
    }

    private void OnEnable()
    {
        GameEvents.OnScoreChanged += OnScoreChanged;
        GameEvents.OnBallLost += OnBallLost;
        GameEvents.OnPauseGame += TogglePause;
        GameEvents.OnLevelCleared += GameWin;
    }

    private void OnDisable()
    {
        GameEvents.OnScoreChanged -= OnScoreChanged;
        GameEvents.OnBallLost -= OnBallLost;
        GameEvents.OnPauseGame -= TogglePause;
        GameEvents.OnLevelCleared -= GameWin;
    }

    private void Start()
    {
        StartGame();
    }

    private void SetState(GameState newState)
    {
        if (State == newState) return;

        State = newState;

        switch (State)
        {
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
            case GameState.GameOver:
            case GameState.Menu:
                Time.timeScale = 0f;
                break;
        }

    }

    public void TogglePause()
    {
        if (State == GameState.Playing)
            SetState(GameState.Paused);
        else if (State == GameState.Paused)
            SetState(GameState.Playing);
    }

    public void StartGame()
    {
        score = 0;
        lives = 3;

        SetState(GameState.Playing);
        SpawnBall(spawnBallLocation);
        GameEvents.RaiseGameStarted();
        UpdateUI();
    }

    private void OnBallLost()
    {
        if (State != GameState.Playing) return;

        lives--;
        UpdateUI();

        if (lives <= 0) EndGame();
        else SpawnBall(spawnBallLocation);
    }

    private void EndGame()
    {
        SetState(GameState.GameOver);
        GameEvents.RaiseGameOver();
    }

    private void GameWin()
    {
        lives = 3;
        SetState(GameState.Playing);

        if (currentBall)
            currentBall.gameObject.SetActive(false);

        SpawnBall(spawnBallLocation);
        GameEvents.RaiseGameStarted();
        UpdateUI();
    }

    private void OnScoreChanged(int delta)
    {
        if (State != GameState.Playing) return;

        score += delta;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (livesText) livesText.text = $"Жизни: {lives}";
        if (scoreText) scoreText.text = $"Очки: {score}";
    }

    private void SpawnBall(Vector2 pos)
    {
        currentBall = ballPool.Get();
        currentBall.transform.position = pos;
        currentBall.gameObject.SetActive(true);
    }
}