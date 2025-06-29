using System.Collections; // ������������ ���� ��� ������ � ����������� � IEnumerator.
using System.Collections.Generic; // ������������ ���� ��� ������ � ������ ���������, ��������, List.
using UnityEngine; // ������������ ���� Unity ��� �������� �����������.

public class Bullet : MonoBehaviour // ����� ��� ���������� ���������� ����.
{
    public float speed = 5f; // �������� �������� ����.

    void Start() // �����, ���������� ��� ������� �������.
    {
        Destroy(gameObject, 5f); // ���������� ���� ����� 5 ������, ���� ��� ������ �� �������.
    }

    void Update() // �����, ���������� ������ ����.
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime); // ���������� ���� ������ �� ��������� `speed`.
    }

    void OnTriggerEnter2D(Collider2D collision) // ����������, ����� ���� ������ � �������.
    {
        if (collision.gameObject.CompareTag("Enemy")) // ���� ���� ����������� � ��������, � �������� ��� "Enemy".
        {
            Destroy(gameObject); // ���������� ����.
        }
    }
}
