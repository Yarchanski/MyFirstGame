using System.Collections; // ������������ ���� ��� ������ � ����������� � IEnumerator.
using System.Collections.Generic; // ������������ ���� ��� ������ � ������ ���������, ��������, List.
using UnityEngine; // ������������ ���� Unity ��� �������� �����������.
using UnityEngine.Localization.Settings; // ������������ ���� ��� ������ � ������������.
using UnityEngine.SceneManagement; // ������������ ���� ��� ���������� �������.

public class GameManager : MonoBehaviour // �������� ����������� ����� ��� ����.
{
    private SpawnEnemy difficulty; // ���� ��� �������� ���������� � ��������� (�� ������������ � ������� ����).
    private string language; // ���� ��� �������� �������� ����� (�� ������������ � ������� ����).

    void Start() // �����, ���������� ��� ������� �������.
    {
        int savedLanguage = PlayerPrefs.GetInt("SelectedLanguage", 0); // �������� ����������� ������ �����, 0 � �������� �� ���������.
        if (savedLanguage < LocalizationSettings.AvailableLocales.Locales.Count) // ���������, ��� ������ �� ������� �� ������� ��������� �������.
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[savedLanguage]; // ������������� ��������� ����.
        }
    }

    void Update() // �����, ���������� ������ ���� (������, ��� ��� ������ �� �������������� � Update).
    {
    }

    public void startButton() // �����, ���������� ��� ������� ������ "Start".
    {
        SceneManager.LoadScene(1); // ��������� ����� � �������� 1.
    }

    public static int NumberOfEnemies = 7; // ����������� ���� ��� ���������� ������.
    public static float StartSpawnerInterval = 1f; // ����������� ���� ��� ��������� ������ ������.

    public void ToMainMenu() // ����� ��� �������� � ������� ����.
    {
        SceneManager.LoadScene(0); // ��������� ����� � �������� 0.
    }

    public void ExitGame() // ����� ��� ������ �� ����.
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // ������������� ���� � ��������� Unity.
#else
        Application.Quit(); // ��������� ����������.
#endif
    }

    public void ToSettings() // ����� ��� �������� � ���������.
    {
        SceneManager.LoadScene(2); // ��������� ����� � �������� 2.
    }

    public void Easy() // ����� ��� ��������� ������ ��������� "�����".
    {
        PlayerPrefs.SetInt("NumberOfEnemies", 4); // ��������� ���������� ������.
        PlayerPrefs.SetFloat("StartSpawnerInterval", 2f); // ��������� �������� ������ ������.
        PlayerPrefs.Save(); // ��������� ��������� �� ����.
    }

    public void Midle() // ����� ��� ��������� ������ ��������� "������".
    {
        PlayerPrefs.SetInt("NumberOfEnemies", 7); // ��������� ���������� ������.
        PlayerPrefs.SetFloat("StartSpawnerInterval", 1f); // ��������� �������� ������ ������.
        PlayerPrefs.Save(); // ��������� ��������� �� ����.
    }

    public void Hard() // ����� ��� ��������� ������ ��������� "������".
    {
        PlayerPrefs.SetInt("NumberOfEnemies", 10); // ��������� ���������� ������.
        PlayerPrefs.SetFloat("StartSpawnerInterval", 0.7f); // ��������� �������� ������ ������.
        PlayerPrefs.Save(); // ��������� ��������� �� ����.
    }

    public void SetLanguageToRussian() // ����� ��� ��������� �������� �����.
    {
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++) // ���������� ��������� ������.
        {
            if (LocalizationSettings.AvailableLocales.Locales[i].Identifier.Code == "ru") // ���� ������� ������ � ����� "ru".
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[i]; // ������������� ������� ����.
                PlayerPrefs.SetInt("SelectedLanguage", i); // ��������� ������ �����.
                return; // ������� �� ������.
            }
        }
        Debug.LogWarning("������� ���� �� ������ � ������ �������!"); // ������� ��������������, ���� ���� �� ������.
    }

    public void SetLanguageToEnglish() // ����� ��� ��������� ����������� �����.
    {
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++) // ���������� ��������� ������.
        {
            if (LocalizationSettings.AvailableLocales.Locales[i].Identifier.Code == "en") // ���� ������� ������ � ����� "en".
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[i]; // ������������� ���������� ����.
                PlayerPrefs.SetInt("SelectedLanguage", i); // ��������� ������ �����.
                return; // ������� �� ������.
            }
        }
        Debug.LogWarning("���������� ���� �� ������ � ������ �������!"); // ������� ��������������, ���� ���� �� ������.
    }
}
