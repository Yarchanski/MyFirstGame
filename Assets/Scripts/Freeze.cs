using System.Collections; // Пространство имен для работы с коллекциями и IEnumerator.
using System.Collections.Generic; // Пространство имен для работы с типами коллекций, например, List.
using UnityEngine; // Пространство имен Unity для базового функционала.

public class Freeze : MonoBehaviour // Класс для управления заморозкой врагов.
{
    void Start() // Метод, вызываемый при запуске объекта.
    {
        // Инициализация, если потребуется.
    }

    void Update() // Метод, вызываемый каждый кадр.
    {
        // Можно добавить поведение объекта, если потребуется.
    }

    void OnTriggerEnter2D(Collider2D collision) // Вызывается при входе объекта в триггер.
    {
        if (collision.gameObject.CompareTag("Enemy")) // Проверяем, является ли объект врагом.
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>(); // Получаем компонент Enemy.
            if (enemy != null) // Проверяем, существует ли компонент Enemy.
            {
                enemy.toFreeze(); // Вызываем метод заморозки у врага.
                Destroy(gameObject); // Уничтожаем объект Freeze после срабатывания.
            }
        }
    }
}
