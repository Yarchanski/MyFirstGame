using System.Collections;               // Пространство имён для работы с коллекциями.
using System.Collections.Generic;      // Пространство имён для работы с обобщёнными коллекциями.
using TMPro;                           // Пространство имён для использования TextMeshPro.
using UnityEditor;                     // Пространство имён для инструментов редактора Unity (используется только в редакторе).
using UnityEngine;                     // Пространство имён для Unity API.
using UnityEngine.UI;                  // Пространство имён для работы с UI-элементами.
using UnityEngine.UIElements;          // Пространство имён для работы с системой UIElements (не используется в этом коде).

public class NewBehaviourScript : MonoBehaviour // Класс наследует MonoBehaviour, что делает его компонентом Unity.
{
    public GameObject Wall;             // Объект стены для спавна.
    public GameObject Turret;           // Объект турели для спавна.
    public GameObject Freeze;           // Объект замораживающего элемента для спавна.
    private Coroutine flashCoroutine;   // Ссылка на корутину для подсветки текста.
    public float speed;                 // Скорость движения игрока.
    public float jumpForce;             // Сила прыжка игрока.
    public Rigidbody2D rb;              // Ссылка на компонент Rigidbody2D для физики.
    public Vector2 moveVector;          // Вектор направления движения.
    public Animator anim;               // Ссылка на компонент Animator для анимации.
    public SpriteRenderer sr;           // Ссылка на SpriteRenderer для управления спрайтом.
    public bool onGround = false;       // Флаг, указывающий, находится ли игрок на земле.
    public Transform GroundCheck;       // Точка проверки наличия земли.
    public float checkRadius;           // Радиус проверки земли.
    public LayerMask Ground;            // Слой, обозначающий землю.
    private int jumpCount = 0;          // Счётчик прыжков в воздухе.
    public int maxJumpValue = 2;        // Максимальное количество прыжков.
    public int gearCount = 5;           // Количество шестерёнок (игровой ресурс).
    public TextMeshProUGUI gearText;    // UI-элемент для отображения количества шестерёнок.
    private bool isFlashing = false;    // Флаг, указывающий, мигает ли текст.
    public float offsetX = 1f;          // Смещение по X для спавна объектов.
    private AudioSource audioSource;    // Источник звука.

    private void Start()                // Метод, вызываемый при старте сцены.
    {
        rb = GetComponent<Rigidbody2D>();    // Получаем компонент Rigidbody2D.
        anim = GetComponent<Animator>();     // Получаем компонент Animator.
        sr = GetComponent<SpriteRenderer>(); // Получаем компонент SpriteRenderer.
        gearText.text = gearCount.ToString(); // Устанавливаем начальный текст для отображения шестерёнок.
        sr.sortingOrder = 10;                // Устанавливаем порядок отрисовки для спрайта.
        Time.timeScale = 1f;                 // Устанавливаем нормальную скорость игры.
        audioSource = GetComponent<AudioSource>(); // Получаем компонент AudioSource.
        audioSource.Play();                  // Проигрываем звук.
    }

    private void Update() // Метод, вызываемый каждый кадр.
    {
        walk();                               // Обрабатываем движение.
        Flip();                               // Поворачиваем спрайт.
        Jump();                               // Обрабатываем прыжки.
        CheckingGround();                     // Проверяем, находится ли игрок на земле.
        gearText.text = gearCount.ToString(); // Обновляем текст количества шестерёнок.
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f); // Получаем сохранённую громкость.
        AudioListener.volume = savedVolume;  // Применяем громкость.
    }

    void walk() // Метод для обработки движения.
    {
        moveVector.x = Input.GetAxis("Horizontal"); // Получаем горизонтальное движение от ввода.
        if (transform.position.x >= 11.25 && moveVector.x > 0) // Ограничиваем движение справа.
        {
            moveVector.x = 0;
        }
        anim.SetFloat("moveX", Mathf.Abs(moveVector.x)); // Передаём значение скорости анимации.
        rb.velocity = new Vector2(moveVector.x * speed, rb.velocity.y); // Задаём скорость Rigidbody.
    }

    void Flip() // Метод для смены направления спрайта.
    {
        if (moveVector.x > 0) sr.flipX = false; // Если движение вправо, не переворачиваем.
        else if (moveVector.x < 0) sr.flipX = true; // Если влево, переворачиваем.
    }

    void Jump() // Метод для обработки прыжков.
    {
        if (Input.GetKeyDown(KeyCode.S)) // Если нажата клавиша S.
        {
            Physics2D.IgnoreLayerCollision(6, 7, true); // Игнорируем столкновения между слоями.
            Invoke("IgnoreLayerOff", 0.3f); // Отключаем игнорирование через 0.3 секунды.
        }
        if (Input.GetKeyDown(KeyCode.Space)) // Если нажата клавиша пробел.
        {
            if (onGround) // Если на земле, выполняем прыжок.
            {
                rb.velocity = new Vector2(rb.velocity.x, 0); // Обнуляем вертикальную скорость.
                rb.AddForce(Vector2.up * jumpForce); // Добавляем прыжковую силу.
            }
            else if (++jumpCount < maxJumpValue) // Если можно прыгнуть ещё раз.
            {
                rb.velocity = new Vector2(0, 15); // Обновляем вертикальную скорость.
            }
        }
        if (onGround) jumpCount = 0; // Сбрасываем счётчик прыжков при касании земли.
    }

    void CheckingGround() // Проверяем, находится ли игрок на земле.
    {
        onGround = Physics2D.OverlapCircle(GroundCheck.position, checkRadius, Ground); // Проверяем пересечение с землёй.
        anim.SetBool("onGround", onGround); // Передаём состояние земли в анимацию.
    }

    void IgnoreLayerOff() // Метод для отключения игнорирования коллизий.
    {
        Physics2D.IgnoreLayerCollision(6, 7, false);
    }

    void OnTriggerEnter2D(Collider2D collision) // Метод для обработки триггеров.
    {
        if (collision.gameObject.CompareTag("Gear")) // Если объект с тегом "Gear".
        {
            if (!collision.gameObject.activeInHierarchy) return; // Если объект неактивен, ничего не делаем.
            gearCount++; // Увеличиваем счётчик шестерёнок.
            collision.gameObject.SetActive(false); // Деактивируем объект.
            Destroy(collision.gameObject); // Уничтожаем объект.
        }
    }

    public void SpawnWall() // Метод для спавна стены.
    {
        if (gearCount > 4) // Если хватает шестерёнок.
        {
            gearCount -= 5; // Уменьшаем счётчик шестерёнок.
            Vector3 spawnPosition = transform.position + new Vector3(offsetX, 0, 0); // Определяем позицию.
            Instantiate(Wall, spawnPosition, Quaternion.identity); // Создаём объект.
        }
        else flashCoroutine = StartCoroutine(FlashGearText()); // Иначе запускаем мигание текста.
    }

    public void SpawnTurret() // Метод для спавна турели.
    {
        if (gearCount > 4)
        {
            gearCount -= 5;
            Vector3 spawnPosition = transform.position + new Vector3(offsetX, 0, 0);
            Instantiate(Turret, spawnPosition, Quaternion.identity);
        }
        else flashCoroutine = StartCoroutine(FlashGearText());
    }

    public void SpawnFreeze() // Метод для спавна элемента заморозки.
    {
        if (gearCount > 4)
        {
            gearCount -= 5;
            Vector3 spawnPosition = transform.position + new Vector3(offsetX, 0, 0);
            Instantiate(Freeze, spawnPosition, Quaternion.identity);
        }
        else flashCoroutine = StartCoroutine(FlashGearText());
    }

    IEnumerator FlashGearText() // Корутина для мигания текста.
    {
        isFlashing = true; // Устанавливаем флаг мигания.
        Color originalColor = gearText.color; // Сохраняем исходный цвет текста.
        Color flashColor = Color.red; // Цвет мигания.

        for (int i = 0; i < 4; i++) // Мигание 4 раза.
        {
            gearText.color = flashColor;
            yield return new WaitForSeconds(0.1f);
            gearText.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
        isFlashing = false; // Сбрасываем флаг.
    }

    public void PlayAudio() // Метод для воспроизведения звука.
    {
        if (audioSource != null) audioSource.Play();
    }

    public void PauseAudio() // Метод для паузы зв
    {
        if (audioSource != null)
        {
            audioSource.Pause();
        }
    }
}