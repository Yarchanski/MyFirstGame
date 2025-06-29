using System.Collections; // Подключает пространство имен для работы с коллекциями, например, IEnumerator.
using System.Collections.Generic; // Подключает пространство имен для работы с типами коллекций, такими как List.
using TMPro; // Подключает пространство имен TextMeshPro для работы с текстовыми элементами.
using UnityEngine; // Подключает пространство имен Unity для базового функционала.

public class Tower_health : MonoBehaviour // Объявляет класс, который будет прикреплен к объекту башни.
{
    public int health = 100; // Здоровье башни, по умолчанию 100.
    private bool isPlayerInside = false; // Флаг, указывающий, находится ли игрок в зоне взаимодействия.
    public Sprite sprite1; // Спрайт для здоровья выше 66.
    public Sprite sprite2; // Спрайт для здоровья от 33 до 66.
    public Sprite sprite3; // Спрайт для здоровья ниже 33.
    private SpriteRenderer spriteRenderer; // Компонент для изменения спрайта объекта.
    private bool isUnderAttack = false; // Флаг, указывающий, атакуется ли башня.
    private Coroutine damageCoroutine; // Ссылка на корутину, которая уменьшает здоровье.
    public Transform player; // Ссылка на объект игрока.
    public float interactionDistance = 2f; // Максимальная дистанция для взаимодействия.
    private NewBehaviourScript playerScript; // Ссылка на скрипт игрока.
    public TextMeshProUGUI gearText; // UI-элемент для отображения количества "шестеренок".
    private Coroutine flashCoroutine; // Ссылка на корутину для визуального мигания текста.
    private bool isFlashing = false; // Флаг, указывающий, выполняется ли мигание текста.
    public bool isMain; // Флаг, указывающий, является ли эта башня главной.

    public void Start() // Метод вызывается при старте объекта.
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Получает компонент SpriteRenderer текущего объекта.
        UpdateSprite(); // Обновляет спрайт в зависимости от текущего здоровья.
        playerScript = player.GetComponent<NewBehaviourScript>(); // Получает скрипт игрока для взаимодействия.
    }

    void Update() // Метод вызывается каждый кадр.
    {
        // InteractWithPlayer(); // Закомментированный вызов метода (не используется).
        UpdateSprite(); // Обновляет спрайт башни.
        if (health < 1 && !isMain) // Если здоровье ниже 1 и башня не является главной.
        {
            Destroy(gameObject); // Уничтожает объект.
        }
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E)) // Если игрок внутри зоны и нажата клавиша E.
        {
            if (playerScript.gearCount >= 10 && health < 100 && isMain) // Если у игрока достаточно "шестеренок", здоровье меньше 100, и башня главная.
            {
                health += 10; // Увеличивает здоровье на 10.
                playerScript.gearCount -= 10; // Уменьшает количество "шестеренок" игрока.
                UpdateSprite(); // Обновляет спрайт.
            }
            else // Если условия не выполнены.
            {
                flashCoroutine = StartCoroutine(FlashGearText()); // Запускает мигание текста.
            }
        }
    }

    void UpdateSprite() // Метод для обновления спрайта башни.
    {
        if (health > 66) // Если здоровье больше 66.
        {
            spriteRenderer.sprite = sprite1; // Устанавливает первый спрайт.
        }
        else if (health > 33) // Если здоровье от 33 до 66.
        {
            spriteRenderer.sprite = sprite2; // Устанавливает второй спрайт.
        }
        else // Если здоровье меньше или равно 33.
        {
            spriteRenderer.sprite = sprite3; // Устанавливает третий спрайт.
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // Метод вызывается, когда объект входит в триггер.
    {
        if (collision.gameObject.CompareTag("Player")) // Если объект - игрок.
        {
            isPlayerInside = true; // Устанавливает флаг, что игрок внутри.
        }
    }

    private void OnTriggerExit2D(Collider2D collision) // Метод вызывается, когда объект покидает триггер.
    {
        if (collision.gameObject.CompareTag("Player")) // Если объект - игрок.
        {
            isPlayerInside = false; // Сбрасывает флаг, что игрок внутри.
        }
    }

    void OnCollisionExit2D(Collision2D collision) // Метод вызывается, когда объект завершает столкновение.
    {
        if (collision.gameObject.CompareTag("Enemy")) // Если объект - враг.
        {
            isUnderAttack = false; // Сбрасывает флаг атаки.
            if (damageCoroutine != null) // Если корутина запущена.
            {
                StopCoroutine(damageCoroutine); // Останавливает корутину.
            }
        }
    }

    IEnumerator DamageOverTime() // Корутина для уменьшения здоровья с течением времени.
    {
        while (isUnderAttack) // Пока башня атакуется.
        {
            health -= 10; // Уменьшает здоровье на 10.
            UpdateSprite(); // Обновляет спрайт.
            yield return new WaitForSeconds(3f); // Ждет 3 секунды.
        }
    }

    IEnumerator FlashGearText() // Корутина для мигания текста.
    {
        isFlashing = true; // Устанавливает флаг мигания.
        Color originalColor = gearText.color; // Сохраняет оригинальный цвет текста.
        Color flashColor = Color.red; // Устанавливает цвет мигания (красный).

        for (int i = 0; i < 4; i++) // Цикл мигания.
        {
            gearText.color = flashColor; // Меняет цвет на красный.
            yield return new WaitForSeconds(0.1f); // Ждет 0.1 секунды.
            gearText.color = originalColor; // Возвращает оригинальный цвет.
            yield return new WaitForSeconds(0.1f); // Ждет 0.1 секунды.
        }
        isFlashing = false; // Сбрасывает флаг мигания.
    }
}
