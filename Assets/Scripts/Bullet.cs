using System.Collections; // Пространство имен для работы с коллекциями и IEnumerator.
using System.Collections.Generic; // Пространство имен для работы с типами коллекций, например, List.
using UnityEngine; // Пространство имен Unity для базового функционала.

public class Bullet : MonoBehaviour // Класс для управления поведением пули.
{
    public float speed = 5f; // Скорость движения пули.

    void Start() // Метод, вызываемый при запуске объекта.
    {
        Destroy(gameObject, 5f); // Уничтожает пулю через 5 секунд, если она никуда не попадет.
    }

    void Update() // Метод, вызываемый каждый кадр.
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime); // Перемещает пулю вправо со скоростью `speed`.
    }

    void OnTriggerEnter2D(Collider2D collision) // Вызывается, когда пуля входит в триггер.
    {
        if (collision.gameObject.CompareTag("Enemy")) // Если пуля столкнулась с объектом, у которого тег "Enemy".
        {
            Destroy(gameObject); // Уничтожает пулю.
        }
    }
}
