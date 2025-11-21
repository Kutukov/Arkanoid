using UnityEngine;

public class Ball : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float initialSpeed = 6f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private AudioClip bounceClip;
    [SerializeField] private float maxAngleOffset = 10f;

    public float Speed { get; private set; }
    private Vector2 direction;

    private void OnEnable()
    {



    }
    void Start()
    {
        direction = Vector2.down;
    }

    // Update is called once per frame
    void Update()
    {



    }
    void FixedUpdate()
    {


        Vector2 minBounds = Camera.main.ViewportToWorldPoint(Vector2.zero);
        Vector2 maxBounds = Camera.main.ViewportToWorldPoint(Vector2.one);

        // Верхняя граница
        if (rb.position.y > maxBounds.y)
        {
            Bounce(Vector2.down);
        }
        // Правая граница
        if (rb.position.x > maxBounds.x)
        {
            Bounce(Vector2.left);
        }
        // Левая граница
        if (rb.position.x < minBounds.x)
        {
            Bounce(Vector2.right);
        }
        // Нижняя граница
        if (rb.position.y < minBounds.y)
        {
            GameEvents.RaiseBallLost();
            gameObject.SetActive(false);
        }
        Vector2 move = direction * Speed * Time.fixedDeltaTime;
        Vector2 newPos = rb.position + move;

        rb.MovePosition(newPos);
    }
    public void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        Speed = initialSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        var contact = collision.contacts[0];
        Vector2 normal = contact.normal;

        if (collision.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeHit(1);
        }
        Bounce(contact.normal);
        // audio
        if (bounceClip) AudioSource.PlayClipAtPoint(bounceClip, transform.position);
    }

    private void Bounce(Vector2 normal)
    {
        direction = Vector2.Reflect(direction, normal).normalized;
        // добавляем небольшое случайное отклонение угла (±10°)
        float angleOffset = Random.Range(-maxAngleOffset, maxAngleOffset);

        // поворачиваем вектор на angleOffset
        direction = Quaternion.Euler(0f, 0f, angleOffset) * direction;
        direction.Normalize();
    }
}
