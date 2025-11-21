using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float clampX = 3.5f;
    public float moveSpeed = 30f;
    private InputSystem_Actions controls;
    void Start()
    {
        // Получаем мировые координаты левого и правого края
        Vector3 left = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 right = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));

        // Ширина платформы (чтобы не вылезала за экран)
        float halfWidth = GetComponent<SpriteRenderer>().bounds.extents.x;

        // Clamp с учётом ширины платформы
        clampX = Mathf.Abs(right.x) - halfWidth;
    }
    private void Awake()
    {

        controls = new InputSystem_Actions();

    }
    private void OnEnable()
    {

        controls.Enable();

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 screenPos = controls.Player.Move.ReadValue<Vector2>();
        // конвертируем в world
        Vector3 world = Camera.main.ScreenToWorldPoint(screenPos);

        float targetX = Mathf.Clamp(world.x, -clampX, clampX);

        float newX = Mathf.MoveTowards(
            transform.position.x,
            targetX,
            moveSpeed * Time.deltaTime
        );

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

    }
}
