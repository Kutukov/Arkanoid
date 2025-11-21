using UnityEngine;

public class NewMonoBehaviourScript : Brick
{
    [SerializeField] private float radius = 2f;
    [SerializeField] private int damage = 1;
    [SerializeField] private GameObject deathVFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    protected override void Die()
    {


        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (var hit in hits)
        {
            if (hit == null) continue;
            if (hit.TryGetComponent<IDamageable>(out var d))
            {
                d.TakeHit(damage);
            }
        }
        if (deathVFX)
        {
            GameObject vfxGO = Instantiate(deathVFX, transform.position, Quaternion.identity);
            ParticleSystem vfx = vfxGO.GetComponent<ParticleSystem>();
            vfx.Play();
            Destroy(vfx.gameObject, vfx.main.duration + vfx.main.startLifetime.constantMax);
        }
        base.Die();
    }
    protected override void Awake()
    {
        base.Awake();
    }
    // Update is called once per frame

}
