using System.Collections; // Подключает пространство имен для работы с коллекциями и IEnumerator.
using System.Collections.Generic; // Подключает пространство имен для работы с типами коллекций, например, List.
using TMPro; // Подключает TextMeshPro для работы с текстовыми элементами.
using UnityEngine; // Пространство имен Unity для базового функционала.
using UnityEngine.SceneManagement; // Пространство имен для управления сценами.
using UnityEngine.UI; // Пространство имен для работы с UI-элементами.

public class Pause_menu : MonoBehaviour // Класс для управления паузой и меню паузы.
{
    public GameObject pauseMenu; // Ссылка на объект меню паузы.
    public NewBehaviourScript playerScript; // Ссылка на скрипт игрока.
    public TextMeshProUGUI timer; // UI-элемент для отображения времени.
    private int sec = 0; // Секунды для таймера.
    private int min = 0; // Минуты для таймера.
    [SerializeField] private int delta = 0; // Поле для возможных временных изменений (пока не используется).
    private bool isPaused = false; // Флаг, указывающий, находится ли игра на паузе.

    // Ссылки на кнопки
    public Button button1; // Ссылка на первую кнопку.
    public Button button2; // Ссылка на вторую кнопку.
    public Button button3; // Ссылка на третью кнопку.

    void Start() // Метод, вызываемый при запуске объекта.
    {
        pauseMenu.SetActive(false); // Скрывает меню паузы.
    }

    void Update() // Метод, вызываемый каждый кадр.
    {
        Pause(); // Проверяет ввод для управления паузой.
    }

    public void Pause() // Метод для переключения состояния паузы.
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Если нажата клавиша Escape.
        {
            if (isPaused) // Если игра уже на паузе.
            {
                isPaused = false; // Снимает паузу.
                pauseMenu.SetActive(false); // Скрывает меню паузы.
                Time.timeScale = 1f; // Возвращает игровую скорость к нормальной.
                playerScript.PlayAudio(); // Воспроизводит звуки.

                // Включает кнопки снова.
                button1.interactable = true;
                button2.interactable = true;
                button3.interactable = true;
            }
            else // Если игра не на паузе.
            {
                isPaused = true; // Ставит игру на паузу.
                pauseMenu.SetActive(true); // Показывает меню паузы.
                Time.timeScale = 0f; // Останавливает игровое время.
                foreach (var audioSource in FindObjectsOfType<AudioSource>()) // Перебирает все аудиокомпоненты.
                {
                    audioSource.Pause(); // Останавливает все звуки.
                }

                // Отключает кнопки.
                button1.interactable = false;
                button2.interactable = false;
                button3.interactable = false;
            }
        }
    }

    public void ContinueClick() // Метод для продолжения игры после паузы.
    {
        isPaused = false; // Снимает паузу.
        pauseMenu.SetActive(false); // Скрывает меню паузы.
        Time.timeScale = 1f; // Возвращает игровую скорость к нормальной.
        playerScript.PlayAudio(); // Воспроизводит звуки.

        // Включает кнопки снова.
        button1.interactable = true;
        button2.interactable = true;
        button3.interactable = true;
    }

    public void RestartClick() // Метод для перезапуска текущей сцены.
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Загружает текущую сцену заново.
    }

    void DisplayTime(float timeToDisplay) // Метод для отображения времени в формате "минуты:секунды".
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); // Вычисляет минуты.
        float seconds = Mathf.FloorToInt(timeToDisplay % 60); // Вычисляет секунды.
        float milliseconds = Mathf.FloorToInt((timeToDisplay * 1000) % 1000); // Вычисляет миллисекунды (не используется в выводе).
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds); // Устанавливает текст таймера.
    }

    public void ToMainMenu() // Метод для перехода в главное меню.
    {
        SceneManager.LoadScene(0); // Загружает сцену с индексом 0 (главное меню).
    }
}
