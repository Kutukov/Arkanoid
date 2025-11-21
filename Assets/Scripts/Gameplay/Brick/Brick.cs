using UnityEngine;
using System.Collections;
public abstract class Brick : MonoBehaviour, IDamageable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] protected int hp = 1;
    [SerializeField] protected int scoreValue = 100;
    [SerializeField] private AudioClip dieClip;
    [SerializeField, Range(0f, 1f)] private float powerUpChance = 0.2f;
    public bool IsDead => hp <= 0;
    public virtual int ScoreValue => scoreValue;
    public virtual void TakeHit(int amount = 1)
    {
        if (IsDead) return;
        hp -= amount;
        PlayHitFeedback();
        if (IsDead) Die();
    }
    protected virtual void Die()
    {

        GameEvents.RaiseScoreChanged(scoreValue);
        if (dieClip) AudioSource.PlayClipAtPoint(dieClip, transform.position);
        //TrySpawnPowerUp();
        Destroy(gameObject);
    }
    private void TrySpawnPowerUp()
    {
        if (Random.value <= powerUpChance)
        {
            PowerUpManager manager = FindFirstObjectByType<PowerUpManager>();
            manager?.SpawnRandom(transform.position);
        }
    }
    protected virtual void Awake()
    {


    }
    protected virtual void PlayHitFeedback()
    {

    }
    public void InitializeSize(float width, float height)
    {

        var sr = GetComponent<SpriteRenderer>();
        var size = sr.sprite.bounds.size;
        float scaleX = width / size.x;
        float scaleY = height / size.y;

        transform.localScale = new Vector3(scaleX, scaleY, 1f);
    }
}
