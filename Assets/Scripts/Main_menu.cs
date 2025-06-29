using System.Collections; // Пространство имен для работы с коллекциями и IEnumerator.
using System.Collections.Generic; // Пространство имен для работы с типами коллекций, например, List.
using UnityEngine; // Пространство имен Unity для базового функционала.
using UnityEngine.Localization.Settings; // Пространство имен для работы с локализацией.
using UnityEngine.SceneManagement; // Пространство имен для управления сценами.

public class GameManager : MonoBehaviour // Основной управляющий класс для игры.
{
    private SpawnEnemy difficulty; // Поле для хранения информации о сложности (не используется в текущем коде).
    private string language; // Поле для хранения текущего языка (не используется в текущем коде).

    void Start() // Метод, вызываемый при запуске объекта.
    {
        int savedLanguage = PlayerPrefs.GetInt("SelectedLanguage", 0); // Получаем сохраненный индекс языка, 0 — значение по умолчанию.
        if (savedLanguage < LocalizationSettings.AvailableLocales.Locales.Count) // Проверяем, что индекс не выходит за пределы доступных локалей.
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[savedLanguage]; // Устанавливаем выбранный язык.
        }
    }

    void Update() // Метод, вызываемый каждый кадр (пустой, так как ничего не обрабатывается в Update).
    {
    }

    public void startButton() // Метод, вызываемый при нажатии кнопки "Start".
    {
        SceneManager.LoadScene(1); // Загружает сцену с индексом 1.
    }

    public static int NumberOfEnemies = 7; // Статическое поле для количества врагов.
    public static float StartSpawnerInterval = 1f; // Статическое поле для интервала спавна врагов.

    public void ToMainMenu() // Метод для перехода в главное меню.
    {
        SceneManager.LoadScene(0); // Загружает сцену с индексом 0.
    }

    public void ExitGame() // Метод для выхода из игры.
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Останавливает игру в редакторе Unity.
#else
        Application.Quit(); // Закрывает приложение.
#endif
    }

    public void ToSettings() // Метод для перехода в настройки.
    {
        SceneManager.LoadScene(2); // Загружает сцену с индексом 2.
    }

    public void Easy() // Метод для установки уровня сложности "Легко".
    {
        PlayerPrefs.SetInt("NumberOfEnemies", 4); // Сохраняет количество врагов.
        PlayerPrefs.SetFloat("StartSpawnerInterval", 2f); // Сохраняет интервал спавна врагов.
        PlayerPrefs.Save(); // Сохраняет изменения на диск.
    }

    public void Midle() // Метод для установки уровня сложности "Средне".
    {
        PlayerPrefs.SetInt("NumberOfEnemies", 7); // Сохраняет количество врагов.
        PlayerPrefs.SetFloat("StartSpawnerInterval", 1f); // Сохраняет интервал спавна врагов.
        PlayerPrefs.Save(); // Сохраняет изменения на диск.
    }

    public void Hard() // Метод для установки уровня сложности "Сложно".
    {
        PlayerPrefs.SetInt("NumberOfEnemies", 10); // Сохраняет количество врагов.
        PlayerPrefs.SetFloat("StartSpawnerInterval", 0.7f); // Сохраняет интервал спавна врагов.
        PlayerPrefs.Save(); // Сохраняет изменения на диск.
    }

    public void SetLanguageToRussian() // Метод для установки русского языка.
    {
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++) // Перебираем доступные локали.
        {
            if (LocalizationSettings.AvailableLocales.Locales[i].Identifier.Code == "ru") // Если найдена локаль с кодом "ru".
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[i]; // Устанавливаем русский язык.
                PlayerPrefs.SetInt("SelectedLanguage", i); // Сохраняем индекс языка.
                return; // Выходим из метода.
            }
        }
        Debug.LogWarning("Русский язык не найден в списке локалей!"); // Выводим предупреждение, если язык не найден.
    }

    public void SetLanguageToEnglish() // Метод для установки английского языка.
    {
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++) // Перебираем доступные локали.
        {
            if (LocalizationSettings.AvailableLocales.Locales[i].Identifier.Code == "en") // Если найдена локаль с кодом "en".
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[i]; // Устанавливаем английский язык.
                PlayerPrefs.SetInt("SelectedLanguage", i); // Сохраняем индекс языка.
                return; // Выходим из метода.
            }
        }
        Debug.LogWarning("Английский язык не найден в списке локалей!"); // Выводим предупреждение, если язык не найден.
    }
}
