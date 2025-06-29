using System.Collections; // Подключает пространство имен для работы с коллекциями, например, для IEnumerator.
using System.Collections.Generic; // Подключает пространство имен для работы с коллекциями типа List.
using UnityEngine; // Подключает пространство имен Unity для базового функционала, такого как Instantiate и MonoBehaviour.

public class SpawnEnemy : MonoBehaviour // Объявляет класс, который будет отвечать за создание врагов.
{
    [SerializeField] // Атрибут для отображения переменных в редакторе Unity.
    private GameObject[] spawnEnemy; // Массив объектов врагов, которые могут быть заспавнены.

    [SerializeField] // Атрибут для отображения переменных в редакторе Unity.
    private Transform[] spawnPoint; // Массив точек для спауна врагов.

    public float startSpawnerInterval; // Интервал между появлениями врагов в секундах (значение по умолчанию установлено в редакторе).
    private float spawnerInterval; // Время, которое прошло с последнего спауна, используется для отслеживания времени.

    public int numberOfEnemies; // Общее количество врагов, которое должно быть заспаунено.
    private int nowTheEnemies = 0; // Текущее количество заспауненных врагов.
    private int randEnemy; // Случайный индекс для выбора типа врага.
    private int randPoint; // Случайный индекс для выбора точки спауна.

    // Start is called before the first frame update
    void Start()
    {
        // Получаем настройки из PlayerPrefs (если они есть) или устанавливаем значения по умолчанию.
        numberOfEnemies = PlayerPrefs.GetInt("NumberOfEnemies", 4); // 4 - значение по умолчанию для количества врагов.
        startSpawnerInterval = PlayerPrefs.GetFloat("StartSpawnerInterval", 2f); // 2f - значение по умолчанию для интервала между спаунами.
    }

    // Update is called once per frame
    void Update()
    {
        // Если интервал для спауна истек, и количество врагов меньше максимального.
        if (spawnerInterval <= 0 && nowTheEnemies < numberOfEnemies)
        {
            randEnemy = Random.Range(0, spawnEnemy.Length); // Генерируем случайный индекс для врага.
            randPoint = Random.Range(0, spawnPoint.Length); // Генерируем случайный индекс для точки спауна.

            // Создаем нового врага на случайной точке.
            GameObject enemy = Instantiate(spawnEnemy[randEnemy], spawnPoint[randPoint].transform.position, Quaternion.identity);

            // Проверяем, имеет ли объект компонент Enemy, и если да, подписываемся на событие уничтожения врага.
            if (enemy.TryGetComponent<Enemy>(out var enemyComponent))
            {
                enemyComponent.OnEnemyDestroyed += DecreaseEnemyCount; // При уничтожении врага уменьшится счетчик заспауненных врагов.
            }

            spawnerInterval = startSpawnerInterval; // Сбрасываем интервал для следующего спауна.
            nowTheEnemies++; // Увеличиваем количество заспауненных врагов.
        }
        else
        {
            // Если интервал еще не истек, уменьшаем время.
            spawnerInterval -= Time.deltaTime;
        }
    }

    private void DecreaseEnemyCount()
    {
        // Метод, который уменьшается количество заспауненных врагов, когда один из них уничтожается.
        nowTheEnemies--;
    }
}
