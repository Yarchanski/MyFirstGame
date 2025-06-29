using System.Collections; // Пространство имен для работы с коллекциями и IEnumerator.
using System.Collections.Generic; // Пространство имен для работы с типами коллекций, например, List.
using UnityEngine; // Пространство имен Unity для базового функционала.

public class Turret : MonoBehaviour // Класс для управления поведением турели.
{
    public int health = 100; // Здоровье турели.
    public Sprite sprite1; // Спрайт турели в обычном состоянии.
    public Sprite sprite2; // Спрайт турели в состоянии атаки.
    private SpriteRenderer spriteRenderer; // Компонент для управления отображением спрайтов.
    public GameObject shotEffectPrefab; // Префаб эффекта выстрела.
    private Coroutine shotCoroutine; // Корутин для управления атаками.
    public Transform spawnBullet; // Точка, откуда будет создаваться эффект выстрела.
    private bool canTrigger = true; // Флаг, разрешающий или запрещающий повторное срабатывание триггера.
    private AudioSource audioSource; // Аудиокомпонент для воспроизведения звука.

    void Start() // Метод, вызываемый при запуске объекта.
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Получаем компонент SpriteRenderer.
        spriteRenderer.sprite = sprite1; // Устанавливаем обычный спрайт.
        audioSource = GetComponent<AudioSource>(); // Получаем компонент AudioSource.
    }

    void Update() // Метод, вызываемый каждый кадр.
    {
        if (health < 1) // Если здоровье меньше 1.
        {
            Destroy(gameObject); // Уничтожаем объект турели.
        }
    }

    void OnTriggerStay2D(Collider2D collision) // Вызывается каждый кадр, пока объект находится внутри триггера.
    {
        if (canTrigger && collision.gameObject.CompareTag("Enemy")) // Если триггер можно активировать и объект с тегом "Enemy".
        {
            audioSource.Play(); // Проигрываем звук.
            Instantiate(shotEffectPrefab, spawnBullet.position, Quaternion.identity); // Создаём эффект выстрела.
            StartCoroutine(DisableTriggerTemporarily(5f)); // Запускаем корутин с задержкой 5 секунд.
        }
    }

    private IEnumerator DisableTriggerTemporarily(float delay) // Корутин для временного отключения триггера.
    {
        spriteRenderer.sprite = sprite2; // Меняем спрайт на атакующий.
        canTrigger = false; // Запрещаем срабатывание триггера.
        yield return new WaitForSeconds(delay); // Ждём указанное время.
        spriteRenderer.sprite = sprite1; // Возвращаем обычный спрайт.
        canTrigger = true; // Снова разрешаем срабатывание триггера.
    }
}
