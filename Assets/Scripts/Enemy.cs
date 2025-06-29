using System.Collections; // Подключает пространство имен для работы с коллекциями, например, для IEnumerator.
using System.Collections.Generic; // Подключает пространство имен для работы с коллекциями типа List.
using UnityEngine; // Подключает пространство имен Unity для базового функционала, такого как Rigidbody2D и Collider2D.

public class Enemy : MonoBehaviour // Объявляет класс для врага, который будет прикреплен к объекту врага.
{
    public delegate void EnemyDestroyedHandler(); // Делегат для обработки события уничтожения врага.
    public event EnemyDestroyedHandler OnEnemyDestroyed; // Событие, которое будет вызываться при уничтожении врага.

    private bool isMain = false; // Флаг, который указывает, является ли враг основным.
    private bool freezed = false; // Флаг, указывающий, заморожен ли враг.
    public float speed = 5f; // Скорость движения врага.
    public int damageAmount = 10; // Сколько урона наносит враг.
    public float damageInterval = 3f; // Интервал между ударами врага.
    public bool isJump = true; // Флаг, определяющий, может ли враг воздействовать на игрока через прыжок.
    private bool isDead = false; // Флаг, указывающий, мертв ли враг.

    private Rigidbody2D rb; // Компонент Rigidbody2D для движения врага.
    public GameObject destructionEffectPrefab; // Префаб эффекта разрушения (взрыв, например).
    private NewBehaviourScript playerScript; // Ссылка на скрипт игрока для взаимодействия.
    private Coroutine damageCoroutine; // Переменная для хранения ссылки на корутину, которая наносит урон.
    private AudioSource audioSource; // Источник звука для проигрывания звуковых эффектов.

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Получаем компонент Rigidbody2D.
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Замораживаем вращение, чтобы враг не поворачивался.
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // Ищем объект игрока.
        playerScript = player.GetComponent<NewBehaviourScript>(); // Получаем скрипт игрока для взаимодействия.
        audioSource = GetComponent<AudioSource>(); // Получаем компонент AudioSource для воспроизведения звуков.
    }

    void Update()
    {
        rb.velocity = new Vector2(-speed, rb.velocity.y); // Двигаем врага влево, оставляя вертикальную скорость без изменений.
    }

    void OnCollisionEnter2D(Collision2D collision) // Метод вызывается при столкновении с другим объектом.
    {
        if (collision.gameObject.CompareTag("Tower")) // Если столкновение с объектом "Tower".
        {
            Tower_health towerHealth = collision.gameObject.GetComponent<Tower_health>(); // Получаем компонент здоровья башни.
            if (towerHealth != null) // Если башня существует.
            {
                damageCoroutine = StartCoroutine(DamageTowerOverTime(towerHealth)); // Запускаем корутину для нанесения урона башне.
            }
        }

        if (collision.gameObject.CompareTag("Trap")) // Если столкновение с объектом "Trap".
        {
            Turret health = collision.gameObject.GetComponent<Turret>(); // Получаем компонент здоровья турели.
            if (health != null) // Если турель существует.
            {
                damageCoroutine = StartCoroutine(DamageTurretOverTime(health)); // Запускаем корутину для нанесения урона турели.
            }
        }

        if (collision.gameObject.CompareTag("Bullet")) // Если столкновение с пулькой.
        {
            OnEnemyDestroyed?.Invoke(); // Вызываем событие уничтожения врага.
            Destroy(gameObject); // Уничтожаем объект врага.
            Instantiate(destructionEffectPrefab, transform.position, Quaternion.identity); // Создаем эффект разрушения.
        }
    }

    void OnCollisionExit2D(Collision2D collision) // Метод вызывается, когда объект выходит из столкновения.
    {
        if (collision.gameObject.CompareTag("Tower") && damageCoroutine != null) // Если враг больше не касается башни.
        {
            StopCoroutine(damageCoroutine); // Останавливаем корутину нанесения урона.
            damageCoroutine = null; // Обнуляем ссылку на корутину.
        }
    }

    void OnTriggerEnter2D(Collider2D collision) // Метод вызывается при входе в триггер.
    {
        if (collision.gameObject.CompareTag("Player")) // Если враг столкнулся с игроком.
        {
            death(); // Вызываем метод смерти врага.
            if (isJump) // Если враг может прыгать.
            {
                playerScript.rb.velocity = new Vector2(playerScript.rb.velocity.x, 5f); // Подбрасываем игрока вверх.
            }
        }

        if (collision.gameObject.CompareTag("Bullet")) // Если враг столкнулся с пулькой.
        {
            death(); // Вызываем метод смерти врага.
        }
    }

    IEnumerator DamageTowerOverTime(Tower_health tower) // Корутина для нанесения урона башне с течением времени.
    {
        while (true) // Бесконечный цикл, пока враг атакует башню.
        {
            tower.health -= damageAmount; // Уменьшаем здоровье башни.
            yield return new WaitForSeconds(damageInterval); // Ждем интервал между ударами.
        }
    }

    IEnumerator DamageTurretOverTime(Turret turret) // Корутина для нанесения урона турели с течением времени.
    {
        while (true) // Бесконечный цикл, пока враг атакует турель.
        {
            turret.health -= damageAmount; // Уменьшаем здоровье турели.
            yield return new WaitForSeconds(damageInterval); // Ждем интервал между ударами.
        }
    }

    void death() // Метод, который вызывается, когда враг умирает.
    {
        if (isDead) return; // Если враг уже мертв, ничего не делаем.

        isDead = true; // Устанавливаем флаг мертвости.
        OnEnemyDestroyed?.Invoke(); // Вызываем событие уничтожения врага.

        // Создаем временный объект для воспроизведения звука.
        GameObject soundObject = new GameObject("DeathSound");
        AudioSource tempAudioSource = soundObject.AddComponent<AudioSource>();
        tempAudioSource.clip = audioSource.clip; // Используем тот же звуковой эффект.
        tempAudioSource.Play(); // Воспроизводим звук.

        // Удаляем временный объект после завершения звука.
        Destroy(soundObject, tempAudioSource.clip.length);

        // Проверка на выпадение объекта с шансом 40%.
        if (Random.Range(0, 100) < 40)
        {
            Instantiate(destructionEffectPrefab, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
        }

        Destroy(gameObject); // Уничтожаем объект врага.
    }

    public void toFreeze() // Метод для замораживания врага.
    {
        if (freezed) return; // Если враг уже заморожен, ничего не делаем.

        freezed = true; // Устанавливаем флаг заморозки.
        speed = 0f; // Останавливаем движение врага.

        Animator animator = GetComponent<Animator>(); // Получаем компонент Animator.
        if (animator != null)
        {
            animator.enabled = false; // Отключаем анимации.
        }

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>(); // Получаем компонент SpriteRenderer.
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(0.5f, 0.8f, 1f); // Меняем цвет врага, чтобы показать замороженность.
        }
    }
}
