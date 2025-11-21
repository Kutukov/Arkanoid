using UnityEngine;

public class HardBrick : Brick
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void TakeHit(int amount = 1)
    {
        base.TakeHit(amount);
        // Можно менять цвет через SpriteRenderer
        var sr = GetComponent<SpriteRenderer>();
        if (sr) sr.color = Color.Lerp(Color.gray, Color.white, 1 / (float)hp);
    }
}
