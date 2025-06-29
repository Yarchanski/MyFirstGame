using System.Collections; // Пространство имен для работы с коллекциями и корутинами.
using System.Collections.Generic; // Пространство имен для работы с коллекциями, такими как List.
using TMPro; // Пространство имен для работы с TextMeshPro.
using UnityEngine; // Пространство имен Unity для базового функционала.

public class Process : MonoBehaviour // Класс для управления отображением таймера.
{
    public TextMeshProUGUI timer; // Ссылка на текстовый объект, отображающий таймер.
    private float timeElapsed = 0f; // Время, прошедшее с момента запуска таймера.
    private bool stopWatchIsRunning = true; // Флаг, управляющий работой таймера.

    void Start() // Метод, вызываемый при запуске объекта.
    {
        // Можно использовать для инициализации, если потребуется.
    }

    void Update() // Метод, вызываемый каждый кадр.
    {
        if (stopWatchIsRunning) // Если таймер работает.
        {
            timeElapsed += Time.deltaTime; // Увеличиваем прошедшее время на время, прошедшее с последнего кадра.
            DisplayTime(timeElapsed); // Обновляем отображение времени.
        }
    }

    void DisplayTime(float timeToDisplay) // Метод для отображения времени на экране.
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); // Подсчитываем количество минут.
        float seconds = Mathf.FloorToInt(timeToDisplay % 60); // Подсчитываем количество секунд.
        float milliseconds = Mathf.FloorToInt((timeToDisplay * 1000) % 1000); // Подсчитываем миллисекунды (не используется для отображения).
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds); // Обновляем текст в формате "мм:сс".
    }
}
