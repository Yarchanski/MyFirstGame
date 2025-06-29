using System.Collections; // ������������ ���� ��� ������ � ����������� � IEnumerator.
using System.Collections.Generic; // ������������ ���� ��� ������ � ������ ���������, ��������, List.
using UnityEngine; // ������������ ���� Unity ��� �������� �����������.

public class Freeze : MonoBehaviour // ����� ��� ���������� ���������� ������.
{
    void Start() // �����, ���������� ��� ������� �������.
    {
        // �������������, ���� �����������.
    }

    void Update() // �����, ���������� ������ ����.
    {
        // ����� �������� ��������� �������, ���� �����������.
    }

    void OnTriggerEnter2D(Collider2D collision) // ���������� ��� ����� ������� � �������.
    {
        if (collision.gameObject.CompareTag("Enemy")) // ���������, �������� �� ������ ������.
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>(); // �������� ��������� Enemy.
            if (enemy != null) // ���������, ���������� �� ��������� Enemy.
            {
                enemy.toFreeze(); // �������� ����� ��������� � �����.
                Destroy(gameObject); // ���������� ������ Freeze ����� ������������.
            }
        }
    }
}
