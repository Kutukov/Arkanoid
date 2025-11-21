using UnityEngine;
[CreateAssetMenu(menuName = "Arkanoid/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public int rows = 5;
    public int cols = 8;
    public float brickWidth = 1f;
    public float brickHeight = 1f;
    public float brickSpacing = 0.1f;
    public float emptyChance = 0.2f;
    public Vector2 startOffset = new Vector2(-3.5f, 4f);
}
