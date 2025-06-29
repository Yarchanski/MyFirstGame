using System.Collections; // ������������ ���� ��� ������ � ����������� � IEnumerator.
using System.Collections.Generic; // ������������ ���� ��� ������ � ������ ���������, ��������, List.
using UnityEngine; // ������������ ���� Unity ��� �������� �����������.

public class Turret : MonoBehaviour // ����� ��� ���������� ���������� ������.
{
    public int health = 100; // �������� ������.
    public Sprite sprite1; // ������ ������ � ������� ���������.
    public Sprite sprite2; // ������ ������ � ��������� �����.
    private SpriteRenderer spriteRenderer; // ��������� ��� ���������� ������������ ��������.
    public GameObject shotEffectPrefab; // ������ ������� ��������.
    private Coroutine shotCoroutine; // ������� ��� ���������� �������.
    public Transform spawnBullet; // �����, ������ ����� ����������� ������ ��������.
    private bool canTrigger = true; // ����, ����������� ��� ����������� ��������� ������������ ��������.
    private AudioSource audioSource; // �������������� ��� ��������������� �����.

    void Start() // �����, ���������� ��� ������� �������.
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // �������� ��������� SpriteRenderer.
        spriteRenderer.sprite = sprite1; // ������������� ������� ������.
        audioSource = GetComponent<AudioSource>(); // �������� ��������� AudioSource.
    }

    void Update() // �����, ���������� ������ ����.
    {
        if (health < 1) // ���� �������� ������ 1.
        {
            Destroy(gameObject); // ���������� ������ ������.
        }
    }

    void OnTriggerStay2D(Collider2D collision) // ���������� ������ ����, ���� ������ ��������� ������ ��������.
    {
        if (canTrigger && collision.gameObject.CompareTag("Enemy")) // ���� ������� ����� ������������ � ������ � ����� "Enemy".
        {
            audioSource.Play(); // ����������� ����.
            Instantiate(shotEffectPrefab, spawnBullet.position, Quaternion.identity); // ������ ������ ��������.
            StartCoroutine(DisableTriggerTemporarily(5f)); // ��������� ������� � ��������� 5 ������.
        }
    }

    private IEnumerator DisableTriggerTemporarily(float delay) // ������� ��� ���������� ���������� ��������.
    {
        spriteRenderer.sprite = sprite2; // ������ ������ �� ���������.
        canTrigger = false; // ��������� ������������ ��������.
        yield return new WaitForSeconds(delay); // ��� ��������� �����.
        spriteRenderer.sprite = sprite1; // ���������� ������� ������.
        canTrigger = true; // ����� ��������� ������������ ��������.
    }
}
