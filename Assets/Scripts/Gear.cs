using System.Collections; // Пространство имен для работы с корутинами.
using UnityEngine; // Пространство имен Unity для базового функционала.

public class Gear : MonoBehaviour // Класс для управления поведением объекта "Gear".
{
    private SpriteRenderer spriteRenderer; // Управляет визуальным отображением объекта.
    private bool isBlinking = false; // Флаг, указывающий, активно ли мигание.
    private AudioSource audioSource; // Источник звука для объекта.
    private bool isCollected = true; // Флаг, определяющий, был ли объект собран.

    void Start() // Метод, вызываемый при запуске объекта.
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Получаем компонент SpriteRenderer для управления спрайтом.
        audioSource = GetComponent<AudioSource>(); // Получаем компонент AudioSource для воспроизведения звука.

        // Запускаем корутину, которая уничтожит объект через заданное время.
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay() // Корутина для уничтожения объекта с задержкой.
    {
        yield return new WaitForSeconds(5f); // Ждём 5 секунд перед началом мигания.

        StartCoroutine(Blink()); // Запускаем корутину мигания.

        yield return new WaitForSeconds(5f); // Ждём ещё 5 секунд, пока идёт мигание.

        isCollected = false; // Обозначаем, что объект не был собран.
        Destroy(gameObject); // Уничтожаем объект.
    }

    IEnumerator Blink() // Корутина для мигания объекта.
    {
        isBlinking = true; // Устанавливаем флаг, что мигание активно.
        while (isBlinking)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; // Переключаем видимость объекта.
            yield return new WaitForSeconds(0.2f); // Ждём 0.2 секунды перед переключением.
        }
    }

    private void OnDestroy() // Метод, вызываемый при уничтожении объекта.
    {
        if (isCollected) // Если объект был собран.
        {
            // Создаём временный объект для проигрывания звука.
            GameObject soundObject = new GameObject("DeathSound");
            AudioSource tempAudioSource = soundObject.AddComponent<AudioSource>();
            tempAudioSource.clip = audioSource.clip; // Копируем звуковую дорожку из оригинального источника.
            tempAudioSource.Play(); // Проигрываем звук.

            // Уничтожаем временный объект после окончания воспроизведения звука.
            Destroy(soundObject, audioSource.clip.length);
        }
        isBlinking = false; // Останавливаем мигание.
    }
}
