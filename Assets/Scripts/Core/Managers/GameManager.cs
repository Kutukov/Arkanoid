
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
    [SerializeField] private Vector2 SpawnBallLocation = new Vector2(0, 0f);
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private int score = 0;
    private Ball currentBall;
    private ObjectPool<Ball> ballPool;

    private void Start()
    {
        StartGame();
    }
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
        GameEvents.OnPauseGame += PauseGame;
        GameEvents.OnLevelCleared += WinGame;
    }

    private void OnDisable()
    {
        GameEvents.OnScoreChanged -= OnScoreChanged;
        GameEvents.OnBallLost -= OnBallLost;
        GameEvents.OnPauseGame -= PauseGame;
        GameEvents.OnLevelCleared -= WinGame;
    }

    public void StartGame()
    {
        score = 0;
        lives = 3;
        State = GameState.Playing;
        SpawnBall(SpawnBallLocation);
        GameEvents.RaiseGameStarted();
        UpdateUI();
    }

    public void PauseGame()
    {
        Debug.Log("Pause");
        State = GameState.Paused;
        Time.timeScale = 0f;
        // broadcast to pausable systems
    }

    public void ResumeGame()
    {
        State = GameState.Playing;
        Time.timeScale = 1f;
    }

    private void OnScoreChanged(int delta)
    {
        score += delta;
        UpdateUI();

    }

    private void OnBallLost()
    {
        lives--;
        UpdateUI();
        if (lives <= 0) EndGame();
        else SpawnBall(SpawnBallLocation);
    }

    private void EndGame()
    {
        State = GameState.GameOver;
        GameEvents.RaiseGameOver();
        GameOver();

    }
    private void GameOver()
    {

        StartGame();
    }


    private void SpawnBall(Vector2 pos)
    {
        currentBall = ballPool.Get();
        currentBall.transform.position = pos;
        currentBall.gameObject.SetActive(true);

    }
    private void WinGame()
    {
        lives = 3;
        State = GameState.Playing;
        currentBall.gameObject.SetActive(false);
        SpawnBall(SpawnBallLocation);
        GameEvents.RaiseGameStarted();
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (livesText) livesText.text = $"Жизни: {lives}";
        if (scoreText) scoreText.text = $"Очки: {score}";
    }
}