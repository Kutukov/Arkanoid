
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelConfig config;
    [SerializeField] private Transform bricksParent;
    [SerializeField] private Brick[] brickPrefabs; // assign various brick prefabs


    private readonly List<Brick> bricks = new List<Brick>();


    private void Start()
    {
        BuildFromConfig();
        GameEvents.OnScoreChanged += CheckLevelComplete;
        GameEvents.OnGameOver += OnLevelLoose;
    }


    private void OnDestroy()
    {
        GameEvents.OnScoreChanged -= CheckLevelComplete;
        GameEvents.OnGameOver -= OnLevelLoose;
    }


    private void BuildFromConfig()
    {
        // Use reflection to map names to prefabs (or a registry could be used)
        int totalCells = config.rows * config.cols;
        int emptyCells = Mathf.RoundToInt(totalCells * config.emptyChance); // сколько пустых
        int brickCells = totalCells - emptyCells;
        List<bool> cells = Enumerable.Repeat(true, brickCells)   // кирпичи
                           .Concat(Enumerable.Repeat(false, emptyCells)) // пустые
                           .OrderBy(_ => Random.value) // перемешиваем
                           .ToList();
        int index = 0;
        for (int y = 0; y < config.rows; y++)
        {
            for (int x = 0; x < config.cols; x++)
            {
                if (!cells[index])
                {
                    index++;
                    continue; // пропускаем пустую клетку
                }
                var prefab = brickPrefabs[Random.Range(0, brickPrefabs.Length)];

                float width = config.brickWidth;
                float height = config.brickHeight;

                Vector2 pos = new Vector2(
                    bricksParent.position.x + x * (width + config.brickSpacing),
                    bricksParent.position.y - y * (height + config.brickSpacing)
                );

                // Создаём кирпич
                var go = Instantiate(prefab, pos, Quaternion.identity, bricksParent);
                var brick = go.GetComponent<Brick>();
                brick.InitializeSize(config.brickWidth, config.brickHeight);


                bricks.Add(brick);
                index++;
            }
        }

    }


    private void OnLevelNext()
    {
        bricks.Clear();
        BuildFromConfig();
    }
    private void OnLevelLoose()
    {
        bricks.Clear();
        BuildFromConfig();

    }


    public void CheckLevelComplete(int value)
    {
        if (bricks.All(b => b.IsDead == true))
        {
            GameEvents.RaiseLevelCleared();
            OnLevelNext();

        }
    }
}
