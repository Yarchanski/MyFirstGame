using System.Collections; // ���������� ������������ ���� ��� ������ � ����������� � IEnumerator.
using System.Collections.Generic; // ���������� ������������ ���� ��� ������ � ������ ���������, ��������, List.
using TMPro; // ���������� TextMeshPro ��� ������ � ���������� ����������.
using UnityEngine; // ������������ ���� Unity ��� �������� �����������.
using UnityEngine.SceneManagement; // ������������ ���� ��� ���������� �������.
using UnityEngine.UI; // ������������ ���� ��� ������ � UI-����������.

public class Pause_menu : MonoBehaviour // ����� ��� ���������� ������ � ���� �����.
{
    public GameObject pauseMenu; // ������ �� ������ ���� �����.
    public NewBehaviourScript playerScript; // ������ �� ������ ������.
    public TextMeshProUGUI timer; // UI-������� ��� ����������� �������.
    private int sec = 0; // ������� ��� �������.
    private int min = 0; // ������ ��� �������.
    [SerializeField] private int delta = 0; // ���� ��� ��������� ��������� ��������� (���� �� ������������).
    private bool isPaused = false; // ����, �����������, ��������� �� ���� �� �����.

    // ������ �� ������
    public Button button1; // ������ �� ������ ������.
    public Button button2; // ������ �� ������ ������.
    public Button button3; // ������ �� ������ ������.

    void Start() // �����, ���������� ��� ������� �������.
    {
        pauseMenu.SetActive(false); // �������� ���� �����.
    }

    void Update() // �����, ���������� ������ ����.
    {
        Pause(); // ��������� ���� ��� ���������� ������.
    }

    public void Pause() // ����� ��� ������������ ��������� �����.
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // ���� ������ ������� Escape.
        {
            if (isPaused) // ���� ���� ��� �� �����.
            {
                isPaused = false; // ������� �����.
                pauseMenu.SetActive(false); // �������� ���� �����.
                Time.timeScale = 1f; // ���������� ������� �������� � ����������.
                playerScript.PlayAudio(); // ������������� �����.

                // �������� ������ �����.
                button1.interactable = true;
                button2.interactable = true;
                button3.interactable = true;
            }
            else // ���� ���� �� �� �����.
            {
                isPaused = true; // ������ ���� �� �����.
                pauseMenu.SetActive(true); // ���������� ���� �����.
                Time.timeScale = 0f; // ������������� ������� �����.
                foreach (var audioSource in FindObjectsOfType<AudioSource>()) // ���������� ��� ���������������.
                {
                    audioSource.Pause(); // ������������� ��� �����.
                }

                // ��������� ������.
                button1.interactable = false;
                button2.interactable = false;
                button3.interactable = false;
            }
        }
    }

    public void ContinueClick() // ����� ��� ����������� ���� ����� �����.
    {
        isPaused = false; // ������� �����.
        pauseMenu.SetActive(false); // �������� ���� �����.
        Time.timeScale = 1f; // ���������� ������� �������� � ����������.
        playerScript.PlayAudio(); // ������������� �����.

        // �������� ������ �����.
        button1.interactable = true;
        button2.interactable = true;
        button3.interactable = true;
    }

    public void RestartClick() // ����� ��� ����������� ������� �����.
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // ��������� ������� ����� ������.
    }

    void DisplayTime(float timeToDisplay) // ����� ��� ����������� ������� � ������� "������:�������".
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); // ��������� ������.
        float seconds = Mathf.FloorToInt(timeToDisplay % 60); // ��������� �������.
        float milliseconds = Mathf.FloorToInt((timeToDisplay * 1000) % 1000); // ��������� ������������ (�� ������������ � ������).
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds); // ������������� ����� �������.
    }

    public void ToMainMenu() // ����� ��� �������� � ������� ����.
    {
        SceneManager.LoadScene(0); // ��������� ����� � �������� 0 (������� ����).
    }
}
