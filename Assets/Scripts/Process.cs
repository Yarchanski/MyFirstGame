using System.Collections; // ������������ ���� ��� ������ � ����������� � ����������.
using System.Collections.Generic; // ������������ ���� ��� ������ � �����������, ������ ��� List.
using TMPro; // ������������ ���� ��� ������ � TextMeshPro.
using UnityEngine; // ������������ ���� Unity ��� �������� �����������.

public class Process : MonoBehaviour // ����� ��� ���������� ������������ �������.
{
    public TextMeshProUGUI timer; // ������ �� ��������� ������, ������������ ������.
    private float timeElapsed = 0f; // �����, ��������� � ������� ������� �������.
    private bool stopWatchIsRunning = true; // ����, ����������� ������� �������.

    void Start() // �����, ���������� ��� ������� �������.
    {
        // ����� ������������ ��� �������������, ���� �����������.
    }

    void Update() // �����, ���������� ������ ����.
    {
        if (stopWatchIsRunning) // ���� ������ ��������.
        {
            timeElapsed += Time.deltaTime; // ����������� ��������� ����� �� �����, ��������� � ���������� �����.
            DisplayTime(timeElapsed); // ��������� ����������� �������.
        }
    }

    void DisplayTime(float timeToDisplay) // ����� ��� ����������� ������� �� ������.
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); // ������������ ���������� �����.
        float seconds = Mathf.FloorToInt(timeToDisplay % 60); // ������������ ���������� ������.
        float milliseconds = Mathf.FloorToInt((timeToDisplay * 1000) % 1000); // ������������ ������������ (�� ������������ ��� �����������).
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds); // ��������� ����� � ������� "��:��".
    }
}
